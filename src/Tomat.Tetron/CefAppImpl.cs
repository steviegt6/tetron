using System;
using CefNet;

namespace Tomat.Tetron;

public sealed class CefAppImpl : CefNetApplication {
    public Action<long> ScheduleMessagePumpWorkCallback { get; set; }

    protected override void OnBeforeCommandLineProcessing(string processType, CefCommandLine commandLine) {
        base.OnBeforeCommandLineProcessing(processType, commandLine);

        Console.WriteLine("OnBeforeCommandLineProcessing");
        Console.WriteLine($"Process type: {processType}");
        Console.WriteLine($"Command line: {commandLine}");

        // TODO: Append command-line switches:
        // commandLine.AppendSwitch("name-no-starting-dashes");
    }

    protected override void OnContextCreated(CefBrowser browser, CefFrame frame, CefV8Context context) {
        base.OnContextCreated(browser, frame, context);

        // TODO: JavaScript can be executed with frame.ExecuteJavaScript();
    }

    protected override void OnScheduleMessagePumpWork(long delayMs) {
        base.OnScheduleMessagePumpWork(delayMs);
        ScheduleMessagePumpWorkCallback?.Invoke(delayMs);
    }
}
