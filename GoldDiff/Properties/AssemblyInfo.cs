using System.Reflection;
using System.Resources;

[assembly: AssemblyProduct("GoldDiff")]
[assembly: AssemblyCopyright("Copyright © Johannes Schneider 2020")]
[assembly: NeutralResourcesLanguage("en-US")]

[assembly: AssemblyInformationalVersion(ThisAssembly.Git.SemVer.Major + "." + ThisAssembly.Git.SemVer.Minor + "." + ThisAssembly.Git.SemVer.Patch + "." + ThisAssembly.Git.Commit)]
[assembly: AssemblyVersion(ThisAssembly.Git.SemVer.Major + "." + ThisAssembly.Git.SemVer.Minor + "." + ThisAssembly.Git.SemVer.Patch)]
[assembly: AssemblyFileVersion(ThisAssembly.Git.SemVer.Major + "." + ThisAssembly.Git.SemVer.Minor + "." + ThisAssembly.Git.SemVer.Patch)]