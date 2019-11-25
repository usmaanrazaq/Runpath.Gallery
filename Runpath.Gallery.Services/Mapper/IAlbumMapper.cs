using Runpath.Gallery.Domain.Models;
using Runpath.Gallery.Dto;

namespace Runpath.Gallery.Service.Mapper
{
    public interface IAlbumMapper
    {
        AlbumDto ToAlbumDto(Album album);
    }

    public class AlbumMapper : IAlbumMapper
    {
        public AlbumDto ToAlbumDto(Album album)
        {
            return new AlbumDto
            {
                Id = album.Id,
                Title = album.Title,
            };
        }
    }
}
