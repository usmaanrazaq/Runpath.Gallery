using System.Collections.Generic;

namespace Runpath.Gallery.Dto
{
    public class AlbumDto
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public List<PhotoDto> Photos { get; set; }
    }
}
