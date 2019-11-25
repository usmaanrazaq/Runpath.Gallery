using Moq;
using NUnit.Framework;
using Runpath.Gallery.Domain.Exceptions;
using Runpath.Gallery.Dto;
using Runpath.Gallery.Service;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Runpath.Gallery.UnitTest
{
    public class IGalleryServiceTests
    {
        private Mock<IAlbumService> _albumService;
        private Mock<IPhotoService> _photoService;
        private IGalleryService _galleryService;

        [SetUp]
        public void Setup()
        {
            _photoService = new Mock<IPhotoService>();
            _albumService = new Mock<IAlbumService>();
            _galleryService = new GalleryService(_albumService.Object, _photoService.Object);

        }

        [Test]
        public async Task WhenICallGalleryService_ToGetAll_ICallCorrectMethods()
        {
            _albumService.Setup(x => x.GetAlbums()).ReturnsAsync(MockAlbums());
            _photoService.Setup(x => x.GetPhotos()).ReturnsAsync(MockPhotos());
            _galleryService = new GalleryService(_albumService.Object, _photoService.Object);
            await _galleryService.GetGallery();
            _albumService.Verify(x => x.GetAlbums(), Times.Once);
            _photoService.Verify(x => x.GetPhotos(), Times.Once);
        }


        [Test]
        public async Task WhenICallGalleryService_AndMapToPhotosAlbums_IGetCorrectPhotos()
        {
            _albumService.Setup(x => x.GetAlbums()).ReturnsAsync(MockAlbums());
            _photoService.Setup(x => x.GetPhotos()).ReturnsAsync(MockPhotos());
            _galleryService = new GalleryService(_albumService.Object, _photoService.Object);
            GalleryDto gallery = await _galleryService.GetGallery();

            Assert.AreEqual(1, gallery.Albums.Where(x => x.Id == 1).Select(p => p.Photos.Count).First());
            Assert.AreEqual(2, gallery.Albums.Where(x => x.Id == 2).Select(p => p.Photos.Count).First());
            Assert.AreEqual(1, gallery.Albums.Where(x => x.Id == 3).Select(p => p.Photos.Count).First());

        }

        [Test]
        public async Task WhenICallGalleryService_WithValidUserId_IGetAlbumsWithPhotos()
        {
            _albumService.Setup(x => x.GetUserAlbums(It.IsAny<int>())).ReturnsAsync(MockAlbums());
            _photoService.Setup(x => x.GetAlbumPhotos(It.IsAny<int>())).ReturnsAsync(MockPhotos());
            _galleryService = new GalleryService(_albumService.Object, _photoService.Object);
            GalleryUserDto gallery = await _galleryService.GetUsersGallery(1);

            _albumService.Verify(x => x.GetUserAlbums(1), Times.Once);
            _photoService.Verify(x => x.GetAlbumPhotos(It.IsAny<int>()), Times.Exactly(3));
            Assert.AreEqual(1, gallery.UserId);
            Assert.AreEqual(3, gallery.Albums.Count);
            foreach (var album in gallery.Albums)
            {
                Assert.AreEqual(4, album.Photos.Count);
            }
        }

        [Test]
        public async Task WhenICallGalleryService_WithInvalidAlbumIds_IGetErrors()
        {
            _albumService.Setup(x => x.GetUserAlbums(It.IsAny<int>())).ReturnsAsync(MockAlbums());
            _photoService.Setup(x => x.GetAlbumPhotos(It.IsAny<int>())).ThrowsAsync(new DataValidationException("test"));
            _galleryService = new GalleryService(_albumService.Object, _photoService.Object);
            GalleryUserDto gallery = await _galleryService.GetUsersGallery(1);
            Assert.AreEqual(3, gallery.Albums.Count);
            Assert.AreEqual(3, gallery.Errors.Count);
        }

        private List<PhotoDto> MockPhotos()
        {
            return new List<PhotoDto>
            {
                new PhotoDto()
                {
                    Id = 1,
                    AlbumId = 2,
                },
                new PhotoDto()
                {
                    Id = 2,
                    AlbumId = 3,
                },
                new PhotoDto()
                {
                    Id = 3,
                    AlbumId = 1,
                },
                new PhotoDto()
                {
                    Id = 4,
                    AlbumId = 2,
                }
            };
        }

        private List<AlbumDto> MockAlbums()
        {

            return new List<AlbumDto>
            {
                new AlbumDto
                {
                    Id = 1,
                    Title = "Album 1"
                },
                new AlbumDto
                {
                    Id = 2,
                    Title = "Album 1"
                },
                new AlbumDto
                {
                    Id = 3,
                    Title = "Album 1"
                },

            };
        }
    }
}