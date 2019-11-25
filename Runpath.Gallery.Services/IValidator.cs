namespace Runpath.Gallery.Service
{
    public interface IValidator
    {
        bool IsValid(int? value);
    }

    public class Validator : IValidator
    {
        public bool IsValid(int? value)
        {
            if (value.HasValue && value.Value > 0)
            {
                return true;
            }

            return false;
        }
    }


}
