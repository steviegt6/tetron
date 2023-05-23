using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Tar;

namespace Tomat.Tetron;

/// <summary>
///     Downloads the CEF binaries for the current platform.
/// </summary>
public static class CefDownloader {
    /// <summary>
    ///     A CEF version.
    /// </summary>
    public class CefVersion {
        public const string BASE_URL = "https://cef-builds.spotifycdn.com/";
        public const string EXTENSION = ".tar.bz2";
        public const string NAME_FORMAT = "cef_binary_{0}+{1}+chromium-{2}_{3}{4}_minimal";

        /// <summary>
        ///     The platform this version is for.
        /// </summary>
        public OSPlatform Platform { get; }

        /// <summary>
        ///     The architecture this version is for.
        /// </summary>
        public Architecture Architecture { get; }

        /// <summary>
        ///     The CEF version.
        /// </summary>
        public string Version { get; }

        /// <summary>
        ///     The CEF commit.
        /// </summary>
        public string Commit { get; }

        /// <summary>
        ///     The Chromium version.
        /// </summary>
        public string ChromiumVersion { get; }

        /// <summary>
        ///     The CEF version string.
        /// </summary>
        /// <param name="platform">The platform this version is for.</param>
        /// <param name="architecture">
        ///     The architecture this version is for.
        /// </param>
        /// <param name="version">The CEF version.</param>
        /// <param name="commit">The CEF commit.</param>
        /// <param name="chromiumVersion">The Chromium version.</param>
        public CefVersion(OSPlatform platform, Architecture architecture, string version, string commit, string chromiumVersion) {
            Platform = platform;
            Architecture = architecture;
            Version = version;
            Commit = commit;
            ChromiumVersion = chromiumVersion;
        }

        public string GetPlatformName() {
            var platform = Platform.ToString().ToLower();
            if (platform == "osx")
                return "macosx";

            return platform;
        }

        public string GetArchitectureName() {
            return Architecture switch {
                Architecture.X86 => "32",
                Architecture.X64 => "64",
                Architecture.Arm => "arm",
                Architecture.Arm64 => "arm64",
                _ => throw new PlatformNotSupportedException("Unsupported architecture."),
            };
        }

        public string GetUniqueName() {
            return string.Format(NAME_FORMAT, Version, Commit, ChromiumVersion, GetPlatformName(), GetArchitectureName());
        }

        public string GetUrl() {
            return (BASE_URL + GetUniqueName() + EXTENSION).Replace("+", "%2B");
        }
    }

    public static readonly List<CefVersion> VERSIONS = new() {
        /*new CefVersion(OSPlatform.Windows, Architecture.X64, "113.1.5", "ge452d82", "113.0.5672.93"),
        new CefVersion(OSPlatform.Windows, Architecture.Arm64, "113.1.5", "ge452d82", "113.0.5672.93"),
        new CefVersion(OSPlatform.Linux, Architecture.X64, "113.1.5", "ge452d82", "113.0.5672.93"),

        new CefVersion(OSPlatform.OSX, Architecture.X64, "113.1.5", "ge452d82", "113.0.5672.93"),
        new CefVersion(OSPlatform.OSX, Architecture.Arm64, "113.1.4", "g327635f", "113.0.5672.63"),

        new CefVersion(OSPlatform.Linux, Architecture.X64, "113.1.5", "ge452d82", "113.0.5672.93"),
        new CefVersion(OSPlatform.Linux, Architecture.Arm64, "113.1.5", "ge452d82", "113.0.5672.93"),
        new CefVersion(OSPlatform.Linux, Architecture.Arm, "111.2.7", "gebf5d6a", "111.0.5563.148"),*/

        // CefNet expects:
        // 105.3.33+gd83f874+chromium-105.0.5195.102
        new CefVersion(OSPlatform.Windows, Architecture.X64, "105.3.33", "gd83f874", "105.0.5195.102"),
        new CefVersion(OSPlatform.Windows, Architecture.Arm64, "105.3.33", "gd83f874", "105.0.5195.102"),
        new CefVersion(OSPlatform.Linux, Architecture.X64, "105.3.33", "gd83f874", "105.0.5195.102"),

        new CefVersion(OSPlatform.OSX, Architecture.X64, "105.3.33", "gd83f874", "105.0.5195.102"),
        new CefVersion(OSPlatform.OSX, Architecture.Arm64, "105.3.33", "gd83f874", "105.0.5195.102"),

        new CefVersion(OSPlatform.Linux, Architecture.X64, "105.3.33", "gd83f874", "105.0.5195.102"),
        new CefVersion(OSPlatform.Linux, Architecture.Arm64, "105.3.33", "gd83f874", "105.0.5195.102"),
        new CefVersion(OSPlatform.Linux, Architecture.Arm, "105.3.33", "gd83f874", "105.0.5195.102"),
    };

    public static CefVersion? GetCefVersion() {
        return VERSIONS.FirstOrDefault(x => RuntimeInformation.IsOSPlatform(x.Platform) && x.Architecture == RuntimeInformation.ProcessArchitecture);
    }

    public static bool CheckCefBinary(CefVersion version, string directory) {
        var dirInfo = new DirectoryInfo(directory);

        // Assume everything's there...
        if (dirInfo.EnumerateDirectories("*", SearchOption.TopDirectoryOnly).Any(x => x.Name == version.GetUniqueName()))
            return true;

        return false;
    }

    public static void DownloadCefBinary(CefVersion version, string archivePath) {
        Directory.CreateDirectory(Path.GetDirectoryName(archivePath)!);
        if (File.Exists(archivePath))
            File.Delete(archivePath);

        using var client = new HttpClient();
        using var stream = client.GetStreamAsync(version.GetUrl()).GetAwaiter().GetResult();
        using var fileStream = new FileStream(archivePath, FileMode.Create);
        stream.CopyTo(fileStream);
    }

    public static void ExtractCefBinary(string archivePath, string outputDir) {
        using var fileStream = new FileStream(archivePath, FileMode.Open);
        using var bzip2Stream = new BZip2InputStream(fileStream);
        using var tarArchive = TarArchive.CreateInputTarArchive(bzip2Stream, Encoding.UTF8);
        tarArchive.ExtractContents(outputDir);
    }

    public static string GetCefBinaryPath(CefVersion version, string directory) {
        var dirInfo = new DirectoryInfo(directory);
        return Path.Combine(dirInfo.FullName, version.GetUniqueName());
    }
}
