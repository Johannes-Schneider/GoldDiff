using System.Diagnostics;
using System.Reflection;
using GoldDiff.Shared.Utility;

namespace GoldDiff.Shared
{
    public static class ApplicationConstants
    {
        public static StringVersion Version { get; }
        
        public static string InformationalVersion { get; } = FileVersionInfo.GetVersionInfo("GoldDiff.exe").ProductVersion;

        public static string RepositoryName { get; } = @"Johannes-Schneider/GoldDiff";
        
        public static string RepositoryUrl { get; } = $"https://github.com/{RepositoryName}";

        static ApplicationConstants()
        {
            var assemblyVersion = FileVersionInfo.GetVersionInfo("GoldDiff.exe").FileVersion;
            Version = StringVersion.TryParse(assemblyVersion, out var version) ? version! : StringVersion.Zero;
        }
    }
}