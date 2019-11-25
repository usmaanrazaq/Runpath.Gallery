namespace Runpath.Gallery.Dto
{
    public class GalleryUserDto : GalleryDto
    {
        public GalleryUserDto(int userId) : base()
        {
            UserId = userId;
        }

        public int UserId { get; set; }
    }
}
