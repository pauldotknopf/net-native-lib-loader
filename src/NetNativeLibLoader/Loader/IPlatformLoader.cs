using System;

namespace NetNativeLibLoader.Loader
{
    public interface IPlatformLoader
    {
        IntPtr LoadLibrary(string path);

        IntPtr LoadSymbol(IntPtr library, string symbolName);

        bool CloseLibrary(IntPtr library);
    }
}