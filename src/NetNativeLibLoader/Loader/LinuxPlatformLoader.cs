namespace NetNativeLibLoader.Loader
{
    public class LinuxPlatformLoader : UnixPlatformLoader
    {
        /// <inheritdoc />
        protected override bool UseCLibrary => false;
    }
}