using System;

namespace Runpath.Gallery.Domain.Exceptions
{
    [Serializable]
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException()
        {
        }
    }
}