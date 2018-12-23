using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using FARPROC = System.IntPtr;
using HMODULE = System.IntPtr;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace NetNativeLibLoader.Loader
{
    internal static class kernel32
    {
        [DllImport("kernel32", SetLastError = true)]
        public static extern HMODULE LoadLibrary(string fileName);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true), Pure]
        public static extern FARPROC GetProcAddress(HMODULE module, string procName);

        [DllImport("kernel32", SetLastError = true)]
        public static extern int FreeLibrary(HMODULE module);
    }
}