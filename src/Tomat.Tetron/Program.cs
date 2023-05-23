using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CefNet;

namespace Tomat.Tetron;

internal static class Program {
    private const string cef_archive_name = "cef.tar.bz2";
    private const string cef_directory_name = "cef";
    private const bool external_message_pump = false;
    private const int message_pump_delay = 10;

    [STAThread]
    internal static void Main() {
        var cefVersion = CefDownloader.GetCefVersion();
        if (cefVersion is null)
            throw new PlatformNotSupportedException("No CEF version available for this platform.");

        Console.WriteLine($"Expecting CEF version: {cefVersion.Version}+{cefVersion.Commit}+chromium_{cefVersion.ChromiumVersion}");
        Console.WriteLine($"Expecting CEF platform: {cefVersion.GetPlatformName()}{cefVersion.GetArchitectureName()}");

        var cefDir = MakeAbsolutePath(cef_directory_name);
        var cefArchive = MakeAbsolutePath(cef_archive_name);
        Directory.CreateDirectory(cefDir);

        if (CefDownloader.CheckCefBinary(cefVersion, cefDir)) {
            Console.WriteLine("Appropriate directory found; assuming CEF binaries are present!");
        }
        else {
            Console.WriteLine("CEF binaries not found; downloading...");
            Console.WriteLine("Using download URL: " + cefVersion.GetUrl());
            CefDownloader.DownloadCefBinary(cefVersion, cefArchive);
            Console.WriteLine("Downloaded; extracting CEF binary...");
            CefDownloader.ExtractCefBinary(cefArchive, cefDir);
            Console.WriteLine("Extracted, deleting archive...");
            File.Delete(cefArchive);
        }

        var cefBinaryDir = CefDownloader.GetCefBinaryPath(cefVersion, cefDir);
        var cefResourcesDir = Path.Combine(cefBinaryDir, "Resources");
        var cefLocalesDir = Path.Combine(cefResourcesDir, "locales");
        var cefReleaseDir = Path.Combine(cefBinaryDir, "Release");
        if (!Directory.Exists(cefBinaryDir))
            throw new DirectoryNotFoundException("CEF binary directory not found: " + cefBinaryDir);

        var icudtlDatPath = Path.Combine(cefReleaseDir, "icudtl.dat");
        if (!File.Exists(icudtlDatPath))
            File.Copy(Path.Combine(cefResourcesDir, "icudtl.dat"), icudtlDatPath);

        var settings = new CefSettings {
            MultiThreadedMessageLoop = !external_message_pump,
            ExternalMessagePump = external_message_pump,
            ResourcesDirPath = Path.Combine(cefBinaryDir, "Resources"),
            LocalesDirPath = Path.Combine(cefBinaryDir, "Resources", "locales"),
        };

        var app = new CefAppImpl();
        app.ScheduleMessagePumpWorkCallback = async (delayMs) => {
            await Task.Delay((int) delayMs);
            CefApi.DoMessageLoopWork();
        };

        Timer? messagePump = null;

        try {
            app.Initialize(Path.Combine(cefBinaryDir, "Release"), settings);

            if (external_message_pump)
                messagePump = new Timer(_ => CefApi.DoMessageLoopWork(), null, message_pump_delay, message_pump_delay);

            using var ev = new ManualResetEvent(false);
            app.SignalForShutdown(() => ev.Set());
            ev.WaitOne();
        }
        finally {
            messagePump?.Dispose();
            app.Shutdown();
            app.Dispose();
        }
    }

    private static string MakeAbsolutePath(string path) {
        // TODO: This is really evil...
        return Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location)!, path);
    }
}
