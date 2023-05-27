type ProgressBarOptions = {
  current?: number;
  total?: number;
  busy?: boolean;
};

type LoadStatusOptions = {
  text: string;
  progress: ProgressBarOptions;
};

class LoadStatus {
  statusElement: HTMLElement;
  statusTextElement: HTMLParagraphElement;
  progressBarElement: HTMLProgressElement;
  busyDivElement: HTMLDivElement;

  constructor(
    statusElement: HTMLElement,
    statusTextElement: HTMLParagraphElement,
    progressBarElement: HTMLProgressElement,
    busyDivElement: HTMLDivElement
  ) {
    this.statusElement = statusElement;
    this.statusTextElement = statusTextElement;
    this.progressBarElement = progressBarElement;
    this.busyDivElement = busyDivElement;
  }

  apply(): void {
    if (!statusContainer) {
      throw new Error("Could not find `status-container`!");
    }

    statusContainer.appendChild(this.statusElement);
  }

  remove(): void {
    if (!statusContainer) {
      throw new Error("Could not find `status-container`!");
    }

    statusContainer.removeChild(this.statusElement);
  }

  updateStatus(status: LoadStatusOptions): void {
    this.statusTextElement.textContent = status.text;
    this.updateProgress(status.progress);
  }

  updateProgress(progress: ProgressBarOptions): void {
    if (!progress.busy && progress.current && progress.total) {
      this.progressBarElement.value = progress.current;
      this.progressBarElement.max = progress.total;
      this.busyDivElement.ariaBusy = "false";
    } else {
      this.progressBarElement.removeAttribute("value");
      this.progressBarElement.removeAttribute("max");
      this.busyDivElement.ariaBusy = "true";
    }
  }
}

let statusContainer: HTMLElement | null;
let loadStatusCount = 0;

function createLoadStatus(options: LoadStatusOptions): LoadStatus {
  if (!statusContainer) {
    throw new Error("Could not find `status-container`!");
  }

  const loadStatusId = `load-status-${loadStatusCount++}`;
  const statusElement = document.createElement("div");
  statusElement.id = loadStatusId;
  statusElement.classList.add("load-status");

  const statusTextElement = document.createElement("p");
  statusTextElement.classList.add("load-status-text");
  statusTextElement.textContent = options.text;
  statusElement.appendChild(statusTextElement);

  const progressBarElement = document.createElement("progress");
  progressBarElement.classList.add("load-status-progress");

  const busyDivElement = document.createElement("div");
  busyDivElement.setAttribute("aria-describedby", loadStatusId);
  statusElement.appendChild(busyDivElement);

  if (
    !options.progress.busy &&
    options.progress.current &&
    options.progress.total
  ) {
    progressBarElement.value = options.progress.current;
    progressBarElement.max = options.progress.total;
  } else {
    busyDivElement.ariaBusy = "true";
  }

  statusElement.appendChild(progressBarElement);

  return new LoadStatus(
    statusElement,
    statusTextElement,
    progressBarElement,
    busyDivElement
  );
}

function bootstrap(): void {
  statusContainer = document.getElementById("status-container");
  if (!statusContainer) {
    throw new Error("Could not find `status-container`!");
  }

  const testStatus = createLoadStatus({
    text: "Testing...",
    progress: { current: 1, total: 2 },
  });

  const testStatus2 = createLoadStatus({
    text: "I have a busy progress bar",
    progress: { busy: true },
  });

  const testStatus3 = createLoadStatus({
    text: "I have a not-busy progress bar",
    progress: { current: 36, total: 100 },
  });

  const testStatus4 = createLoadStatus({
    text: "AAAAAAAAAA",
    progress: { current: 36, total: 100 },
  });

  const testStatus5 = createLoadStatus({
    text: "BBBBBBBBBB",
    progress: { current: 36, total: 100 },
  });

  testStatus.apply();
  testStatus2.apply();
  testStatus3.apply();
  testStatus4.apply();
  testStatus5.apply();
}

window.addEventListener("DOMContentLoaded", bootstrap);
