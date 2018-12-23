namespace NetNativeLibLoader.Loader
{
    internal class LinuxPlatformLoader : UnixPlatformLoader
    {
        /// <inheritdoc />
        protected override bool UseCLibrary => false;
    }
}