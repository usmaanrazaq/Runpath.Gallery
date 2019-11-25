using Runpath.Gallery.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Runpath.Gallery.Service
{
    public interface IGalleryService
    {
        Task<GalleryUserDto> GetUsersGallery(int userId);
        Task<GalleryDto> GetGallery();
    }

    public class GalleryService : IGalleryService
    {
        private readonly IAlbumService _albumService;
        private readonly IPhotoService _photoService;
        public GalleryService(IAlbumService albumService, IPhotoService photoService)
        {
            _photoService = photoService;
            _albumService = albumService;
        }

        public async Task<GalleryDto> GetGallery()
        {
            GalleryDto gallery = new GalleryDto();
            gallery.Albums = await _albumService.GetAlbums();
            List<PhotoDto> photos = await _photoService.GetPhotos();

            foreach (var album in gallery.Albums)
            {
                album.Photos = photos.Where(x => x.AlbumId == album.Id).ToList();
            }

            return gallery;
        }

        public async Task<GalleryUserDto> GetUsersGallery(int userId)
        {
            GalleryUserDto gallery = new GalleryUserDto(userId);
            gallery.Albums = await _albumService.GetUserAlbums(userId);
            foreach (var album in gallery.Albums)
            {
                try
                {
                    album.Photos = await _photoService.GetAlbumPhotos(album.Id);
                }
                catch (Exception ex)
                {

                    gallery.Errors.Add($"Album Photo could not be retrieved {album?.Title}" + ex.Message);
                }
            }

            return gallery;
        }
    }
}
