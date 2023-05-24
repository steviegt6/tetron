using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using CefNet;

namespace Tomat.Tetron.Avalonia;

internal partial class Program {
    private const string cef_archive_name = "cef.tar.bz2";
    private const string cef_directory_name = "cef";
    private const bool external_message_pump = false;
    private const int message_pump_delay = 10;

    private static CefAppImpl? app;
    private static DispatcherTimer? messagePump;

    private static void InitCef(string[] args) {
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

        var externalMessagePump = external_message_pump || args.Contains("--external-message-pump");

        var settings = new CefSettings {
            MultiThreadedMessageLoop = !externalMessagePump,
            ExternalMessagePump = externalMessagePump,
            NoSandbox = true,
            WindowlessRenderingEnabled = true,
            ResourcesDirPath = cefResourcesDir,
            LocalesDirPath = cefLocalesDir,
        };

        App.FrameworkInitialized += App_FrameworkInitialized;
        App.FrameworkShutdown += App_FrameworkShutdown;

        app = new CefAppImpl();
        app.ScheduleMessagePumpWorkCallback = OnScheduleMessagePumpWork;

        app.Initialize(cefReleaseDir, settings);
    }

    private static string MakeAbsolutePath(string path) {
        // TODO: This is really evil...
        return Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location)!, path);
    }

    private static void App_FrameworkInitialized(object? sender, EventArgs e) {
        if (!CefNetApplication.Instance.UsesExternalMessageLoop)
            return;

        messagePump = new DispatcherTimer(
            TimeSpan.FromMilliseconds(message_pump_delay),
            DispatcherPriority.Normal,
            (_, _) => {
                CefApi.DoMessageLoopWork();
                Dispatcher.UIThread.RunJobs();
            }
        );
        messagePump.Start();
    }

    private static void App_FrameworkShutdown(object? sender, EventArgs e) {
        messagePump?.Stop();
    }

    private static async void OnScheduleMessagePumpWork(long delayMs) {
        await Task.Delay((int)delayMs);
        Dispatcher.UIThread.Post(CefApi.DoMessageLoopWork);
    }
}
