using Runpath.Gallery.Domain.Exceptions;
using Runpath.Gallery.Domain.Models;
using Runpath.Gallery.Dto;
using Runpath.Gallery.Service.External;
using Runpath.Gallery.Service.Mapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Runpath.Gallery.Service
{
    public interface IPhotoService
    {
        Task<List<PhotoDto>> GetAlbumPhotos(int albumId);
        Task<List<PhotoDto>> GetPhotos();
    }

    public class PhotoService : IPhotoService
    {
        private readonly IHttpClientCaller<Photo> _photoApi;
        private readonly IPhotoMapper _mapper;
        private readonly string photoEndpoint = "/photos";
        private readonly IValidator _validator;

        public PhotoService(IHttpClientCaller<Photo> photoApi, IPhotoMapper mapper, IValidator validator)
        {
            _photoApi = photoApi;
            _mapper = mapper;
            _validator = validator;
        }
        public async Task<List<PhotoDto>> GetAlbumPhotos(int albumId)
        {
            if (!_validator.IsValid(albumId))
            {
                throw new DataValidationException("AlbumId");
            }

            ICollection<Photo> photos = await _photoApi.GetApiCollection($"{photoEndpoint}?albumId=" + albumId);
            return photos.Select(photo => _mapper.ToPhotoDto(photo)).ToList();
        }

        public async Task<List<PhotoDto>> GetPhotos()
        {
            ICollection<Photo> photos = await _photoApi.GetApiCollection($"{photoEndpoint}");
            return photos.Select(photo => _mapper.ToPhotoDto(photo)).ToList();
        }
    }
}
