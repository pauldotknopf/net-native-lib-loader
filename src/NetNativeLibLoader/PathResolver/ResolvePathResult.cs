using System;

namespace NetNativeLibLoader.PathResolver
{
    public struct ResolvePathResult
    {
        public string Path { get; }

        public string ErrorReason { get; }

        public bool IsSuccess { get; }

        public Exception Exception { get; }

        private ResolvePathResult
        (
            string path,
            string errorReason,
            bool isSuccess,
            Exception exception
        )
        {
            Path = path;
            ErrorReason = errorReason;
            IsSuccess = isSuccess;
            Exception = exception;
        }

        public static ResolvePathResult FromSuccess(string resolvedPath)
        {
            return new ResolvePathResult(resolvedPath, null, true, null);
        }

        public static ResolvePathResult FromError(string errorReason)
        {
            return new ResolvePathResult(null, errorReason, false, null);
        }

        public static ResolvePathResult FromError(Exception exception)
        {
            return new ResolvePathResult(null, exception.Message, false, exception);
        }
    }
}