using System;
using System.IO;
using System.Reflection;

namespace NetNativeLibLoader.PathResolver
{
    public class LocalPathResolver : IPathResolver
    {
        private readonly string _entryAssemblyDirectory;
        private readonly string _executingAssemblyDirectory;
        private readonly string _currentDirectory;

        public LocalPathResolver()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            _entryAssemblyDirectory = entryAssembly is null
                ? null
                : Directory.GetParent(entryAssembly.Location).FullName;

            _executingAssemblyDirectory = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;

            _currentDirectory = Directory.GetCurrentDirectory();
        }

        public ResolvePathResult Resolve(string library)
        {
            // First, check next to the entry executable
            if (!(_entryAssemblyDirectory is null))
            {
                var result = ScanPathForLibrary(_entryAssemblyDirectory, library);
                if (result.IsSuccess)
                {
                    return result;
                }
            }

            if (!(_executingAssemblyDirectory is null))
            {
                var result = ScanPathForLibrary(_executingAssemblyDirectory, library);
                if (result.IsSuccess)
                {
                    return result;
                }
            }

            // Then, check the current directory
            if (!(_currentDirectory is null))
            {
                var result = ScanPathForLibrary(_currentDirectory, library);
                if (result.IsSuccess)
                {
                    return result;
                }
            }

            return ResolvePathResult.FromError(new FileNotFoundException("No local copy of the given library could be found.", library));
        }

        private ResolvePathResult ScanPathForLibrary(string path, string library)
        {
            var libraryLocation = Path.GetFullPath(Path.Combine(path, library));
            if (File.Exists(libraryLocation))
            {
                return ResolvePathResult.FromSuccess(libraryLocation);
            }

            // Check the local library directory
            libraryLocation = Path.GetFullPath(Path.Combine(path, "lib", library));
            if (File.Exists(libraryLocation))
            {
                return ResolvePathResult.FromSuccess(libraryLocation);
            }

            // Check platform-specific directory
            var bitness = Environment.Is64BitProcess ? "x64" : "x86";
            libraryLocation = Path.GetFullPath(Path.Combine(path, "lib", bitness, library));
            if (File.Exists(libraryLocation))
            {
                return ResolvePathResult.FromSuccess(libraryLocation);
            }

            return ResolvePathResult.FromError(new FileNotFoundException("No local copy of the given library could be found.", library));
        }
    }
}