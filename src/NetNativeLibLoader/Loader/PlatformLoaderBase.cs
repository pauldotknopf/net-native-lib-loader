using System;
using System.Runtime.InteropServices;

namespace NetNativeLibLoader.Loader
{
    public abstract class PlatformLoaderBase : IPlatformLoader
    {
        public IntPtr LoadLibrary(string path) => LoadLibraryInternal(path);

        protected abstract IntPtr LoadLibraryInternal(string path);

        public abstract IntPtr LoadSymbol(IntPtr library, string symbolName);

        public abstract bool CloseLibrary(IntPtr library);

        public static IPlatformLoader SelectPlatformLoader()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new WindowsPlatformLoader();
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return new LinuxPlatformLoader();
            }

            /*
                Temporary hack until BSD is added to RuntimeInformation. OSDescription should contain the output from
                "uname -srv", which will report something along the lines of FreeBSD or OpenBSD plus some more info.
            */
            bool isBSD = RuntimeInformation.OSDescription.ToUpperInvariant().Contains("BSD");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || isBSD)
            {
                return new BSDPlatformLoader();
            }

            throw new PlatformNotSupportedException($"Cannot load native libraries on this platform: {RuntimeInformation.OSDescription}");
        } 
    }
}