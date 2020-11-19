using System.Diagnostics;
using System.Reflection;

namespace GoldDiff.Shared
{
    public static class ApplicationConstants
    {
        public static string Version { get; } = FileVersionInfo.GetVersionInfo("GoldDiff.exe").ProductVersion;

        public static string RepositoryUrl { get; } = "https://github.com/Johannes-Schneider/GoldDiff";
    }
}