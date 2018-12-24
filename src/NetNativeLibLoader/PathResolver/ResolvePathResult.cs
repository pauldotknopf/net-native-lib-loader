//
//  ResolvePathResut.cs
//
//  Copyright (c) 2018 Firwood Software
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

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