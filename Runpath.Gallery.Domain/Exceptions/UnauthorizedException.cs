using System;

namespace Runpath.Gallery.Domain.Exceptions
{
    [Serializable]
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException()
        {
        }
    }
}
