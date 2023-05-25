using System;

namespace Tomat.Tetron.Avalonia.ViewModels.Pages;

public sealed class CefPageViewModel : ViewModel {
    public static CefDownloader.CefVersion? CefVersion => CefDownloader.GetCefVersion();

    public bool IsInstalled { get; set; }

    public string FormattedCefVersion {
        get {
            if (CefVersion is null)
                return "<unknown>";

            return $"{CefVersion.Version} ({CefVersion.Commit})";
        }
    }

    public string OsArchName {
        get {
            if (CefVersion is null)
                return "<unknown>";

            //return $"{CefVersion.GetPlatformName()} ({CefVersion.GetArchitectureName()})";
            return $"{CefVersion.Platform.ToString()} ({CefVersion.Architecture.ToString()})";
        }
    }

    public string FormattedChromiumVersion {
        get {
            if (CefVersion is null)
                return "<unknown>";

            return CefVersion.ChromiumVersion;
        }
    }

    public void Download() { }
}
