namespace NetNativeLibLoader.Loader
{
    internal class BSDPlatformLoader : UnixPlatformLoader
    {
        protected override bool UseCLibrary => true;
    }
}