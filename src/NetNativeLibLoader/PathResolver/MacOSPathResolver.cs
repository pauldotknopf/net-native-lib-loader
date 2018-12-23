using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NetNativeLibLoader.PathResolver
{
    internal class MacOSPathResolver : IPathResolver
    {
        private static readonly IReadOnlyList<string> EnvironmentVariables = new[]
        {
            "DYLD_FRAMEWORK_PATH",
            "DYLD_LIBRARY_PATH",
            "DYLD_FALLBACK_FRAMEWORK_PATH",
            "DYLD_FALLBACK_LIBRARY_PATH"
        };

        public ResolvePathResult Resolve(string library)
        {
            foreach (var variable in EnvironmentVariables)
            {
                var libraryPaths = Environment.GetEnvironmentVariable(variable)?.Split(':').Where(p => !string.IsNullOrEmpty(p));

                if (libraryPaths is null)
                {
                    continue;
                }

                foreach (var path in libraryPaths)
                {
                    var libraryLocation = Path.GetFullPath(Path.Combine(path, library));
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