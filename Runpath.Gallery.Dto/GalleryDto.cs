using System.Collections.Generic;

namespace Runpath.Gallery.Dto
{
    public class GalleryDto
    {
        public GalleryDto()
        {
            Errors = new List<string>();
        }
        public List<AlbumDto> Albums { get; set; }

        public List<string> Errors { get; set; }
    }
}
