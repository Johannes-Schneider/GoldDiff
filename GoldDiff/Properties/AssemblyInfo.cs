using System.Reflection;
using System.Resources;

[assembly: AssemblyProduct("GoldDiff")]
[assembly: AssemblyCopyright("Copyright © Johannes Schneider 2020")]
[assembly: NeutralResourcesLanguage("en-US")]

[assembly: AssemblyVersion(ThisAssembly.Git.SemVer.Major + "." + ThisAssembly.Git.SemVer.Minor + "." + ThisAssembly.Git.SemVer.Patch)]
[assembly: AssemblyInformationalVersion(ThisAssembly.Git.SemVer.Major + "." +
                                        ThisAssembly.Git.SemVer.Minor + "." +
                                        ThisAssembly.Git.SemVer.Patch + " #" +
                                        ThisAssembly.Git.Commit)]