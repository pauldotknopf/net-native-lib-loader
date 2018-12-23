namespace NetNativeLibLoader.PathResolver
{
    public interface IPathResolver
    {
        ResolvePathResult Resolve(string library);
    }
}