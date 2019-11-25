using Runpath.Gallery.Domain.Models;
using Runpath.Gallery.Dto;

namespace Runpath.Gallery.Service.Mapper
{
    public interface IPhotoMapper
    {
        PhotoDto ToPhotoDto(Photo photo);
    }

    public class PhotoMapper : IPhotoMapper
    {
        public PhotoDto ToPhotoDto(Photo photo)
        {
            return new PhotoDto
            {
                Id = photo.Id,
                ThumbnailUrl = photo.ThumbnailUrl,
                Title = photo.Title,
                Url = photo.Url,
                AlbumId = photo.AlbumId
            };
        }
    }
}
