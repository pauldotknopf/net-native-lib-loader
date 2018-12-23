using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace NetNativeLibLoader.Loader
{
    internal class WindowsPlatformLoader : PlatformLoaderBase
    {
        protected override IntPtr LoadLibraryInternal(string path)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path), "null library names or paths are not supported on Windows.");
            }

            var libraryHandle = kernel32.LoadLibrary(path);
            if (libraryHandle == IntPtr.Zero)
            {
                throw new Exception($"Library loading failed: {path}", new Win32Exception(Marshal.GetLastWin32Error()));
            }

            return libraryHandle;
        }

        public override IntPtr LoadSymbol(IntPtr library, string symbolName)
        {
            var symbolHandle = kernel32.GetProcAddress(library, symbolName);
            if (symbolHandle == IntPtr.Zero)
            {
                throw new Exception($"Symbol loading failed. Symbol name: {symbolName}", new Win32Exception(Marshal.GetLastWin32Error()));
            }

            return symbolHandle;
        }

        public override bool CloseLibrary(IntPtr library)
        {
            return kernel32.FreeLibrary(library) > 0;
        }
    }
}