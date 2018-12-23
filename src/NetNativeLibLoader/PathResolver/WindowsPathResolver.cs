using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NetNativeLibLoader.PathResolver
{
    public class WindowsPathResolver : IPathResolver
    {
        public ResolvePathResult Resolve(string library)
        {
            string libraryLocation;

            var entryAssembly = Assembly.GetEntryAssembly();
            if (!(entryAssembly is null) && Directory.GetParent(entryAssembly.Location) is var parentDirectory)
            {
                var executingDir = parentDirectory.FullName;

                libraryLocation = Path.GetFullPath(Path.Combine(executingDir, library));
                if (File.Exists(libraryLocation))
                {
                    return ResolvePathResult.FromSuccess(libraryLocation);
                }
            }

            var sysDir = Environment.SystemDirectory;
            libraryLocation = Path.GetFullPath(Path.Combine(sysDir, library));
            if (File.Exists(libraryLocation))
            {
                return ResolvePathResult.FromSuccess(libraryLocation);
            }

            var windowsDir = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            var sys16Dir = Path.Combine(windowsDir, "System");
            libraryLocation = Path.GetFullPath(Path.Combine(sys16Dir, library));
            if (File.Exists(libraryLocation))
            {
                return ResolvePathResult.FromSuccess(libraryLocation);
            }

            libraryLocation = Path.GetFullPath(Path.Combine(windowsDir, library));
            if (File.Exists(libraryLocation))
            {
                return ResolvePathResult.FromSuccess(libraryLocation);
            }

            var currentDir = Directory.GetCurrentDirectory();
            libraryLocation = Path.GetFullPath(Path.Combine(currentDir, library));
            if (File.Exists(libraryLocation))
            {
                return ResolvePathResult.FromSuccess(libraryLocation);
            }

            var pathVar = Environment.GetEnvironmentVariable("PATH");
            if (!(pathVar is null))
            {
                var pathDirs = pathVar.Split(';').Where(p => !string.IsNullOrEmpty(p));
                foreach (var path in pathDirs)
                {
                    libraryLocation = Path.GetFullPath(Path.Combine(path, library));
                    if (File.Exists(libraryLocation))
                    {
                        return ResolvePathResult.FromSuccess(libraryLocation);
                    }
                }
            }

            return ResolvePathResult.FromError(new FileNotFoundException("The specified library was not found in any of the loader search paths.", library));
        }
    }
}