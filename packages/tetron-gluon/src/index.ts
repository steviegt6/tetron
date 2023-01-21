import * as Gluon from "@gluon-framework/gluon";
import { Config } from "./config.js";
import getSwitches from "./switches.js";

const config = Config.read();
const gluon = await Gluon.open(config.get("url"), {
    windowSize: config.get("windowSize"),
    getSwitches: getSwitches,
    onLoad: () => {
        function send(event: string, data: unknown) {
            // eslint-disable-next-line @typescript-eslint/ban-ts-comment
            // @ts-ignore
            Gluon.ipc.send(event, data);
        }

        window.addEventListener("resize", (e: UIEvent) => {
            e.view?.outerWidth;

            send("window resize", { width: window.outerWidth, height: window.outerHeight });
        });
    },
    onClose: () => {
        Config.write(config);
    }
});

type WindowResizeData = {
    width: number;
    height: number;
};

gluon.ipc.on("window resize", ({ width, height }: WindowResizeData) => {
    console.log("resize", width, height);
    config.set<[number, number]>("windowSize", [width, height]);
});
