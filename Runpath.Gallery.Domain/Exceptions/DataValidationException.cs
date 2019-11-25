using System;

namespace Runpath.Gallery.Domain.Exceptions
{
    [Serializable]
    public class DataValidationException : Exception
    {

        public DataValidationException(string name) : base($"Invalid Data input, Property: {name}")
        {

        }


    }
}
