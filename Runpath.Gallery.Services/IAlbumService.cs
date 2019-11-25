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
    public interface IAlbumService
    {
        Task<List<AlbumDto>> GetUserAlbums(int userId);

        Task<List<AlbumDto>> GetAlbums();
    }

    public class AlbumService : IAlbumService
    {
        private readonly IHttpClientCaller<Album> _albumApi;
        private readonly IAlbumMapper _mapper;
        private readonly IValidator _validator;
        private readonly string AlbumEndpoint = "/albums";

        public AlbumService(IHttpClientCaller<Album> albumApi, IAlbumMapper mapper, IValidator validator)
        {
            _albumApi = albumApi;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<List<AlbumDto>> GetAlbums()
        {
            ICollection<Album> Albums = await _albumApi.GetApiCollection($"{AlbumEndpoint}");
            return Albums.Select(album => _mapper.ToAlbumDto(album)).ToList();
        }

        public async Task<List<AlbumDto>> GetUserAlbums(int userId)
        {
            if (!_validator.IsValid(userId))
            {
                throw new DataValidationException(nameof(userId));
            }
            ICollection<Album> Albums = await _albumApi.GetApiCollection($"{AlbumEndpoint}?userId={userId}");
            return Albums.Select(album => _mapper.ToAlbumDto(album)).ToList();
        }
    }
}
