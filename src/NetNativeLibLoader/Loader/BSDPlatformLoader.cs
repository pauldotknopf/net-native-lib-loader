namespace NetNativeLibLoader.Loader
{
    public class BSDPlatformLoader : UnixPlatformLoader
    {
        protected override bool UseCLibrary => true;
    }
}