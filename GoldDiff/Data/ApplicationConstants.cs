using System.Diagnostics;
using System.Reflection;

namespace GoldDiff.Data
{
    public static class ApplicationConstants
    {
        public static string Version { get; } = FileVersionInfo.GetVersionInfo(Assembly.GetAssembly(typeof(ApplicationConstants)).Location).ProductVersion;

        public static string RepositoryUrl { get; } = "https://github.com/Johannes-Schneider/GoldDiff";
    }
}