using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static Bullseye.Targets;
using static Build.Buildary.Directory;
using static Build.Buildary.Path;
using static Build.Buildary.Shell;
using static Build.Buildary.Runner;
using static Build.Buildary.Runtime;
using static Build.Buildary.Log;
using static Build.Buildary.File;
using static Build.Buildary.GitVersion;

namespace Build
{
    static class Program
    {
        static Task Main(string[] args)
        {
            var options = ParseOptions<Options>(args);
            
            var gitversion = GetGitVersion(ExpandPath("./"));
            var commandBuildArgs = $"--configuration {options.Configuration}  --version-suffix \"{gitversion.PreReleaseTag}\"";
            
            Info($"Version: {JsonConvert.SerializeObject(gitversion)}");
            
            Target("clean", () =>
            {
                CleanDirectory(ExpandPath("./output"));
            });
           
            Target("build", () =>
            {
                RunShell($"dotnet build {ExpandPath("./NetNativeLibLoader.sln")} {commandBuildArgs}");
            });

            
            Target("update-version", () =>
            {
                if (FileExists("./build/version.props"))
                {
                    DeleteFile("./build/version.props");
                }
                
                WriteFile("./build/version.props",
                    $@"<Project>
    <PropertyGroup>
        <VersionPrefix>{gitversion.Version}</VersionPrefix>
    </PropertyGroup>
</Project>");
            });
            
            Target("deploy", DependsOn("update-version", "clean"), () =>
            {
                RunShell($"dotnet pack {ExpandPath("./src/NetNativeLibLoader/NetNativeLibLoader.csproj")} --output {ExpandPath("./output")} {commandBuildArgs}");
            });
            
            Target("default", DependsOn("build", "deploy"));

            return Run(options);
        }

        // ReSharper disable ClassNeverInstantiated.Local
        class Options : RunnerOptions
        // ReSharper restore ClassNeverInstantiated.Local
        {
            [PowerArgs.ArgShortcut("config"), PowerArgs.ArgDefaultValue("Release")]
            public string Configuration { get; set; }
        }
    }
}