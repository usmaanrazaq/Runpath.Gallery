using Moq;
using NUnit.Framework;
using Runpath.Gallery.Domain.Exceptions;
using Runpath.Gallery.Domain.Models;
using Runpath.Gallery.Dto;
using Runpath.Gallery.Service;
using Runpath.Gallery.Service.External;
using Runpath.Gallery.Service.Mapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Runpath.Gallery.UnitTest
{
    public class IPhotoServiceTests
    {
        private IPhotoService _photoService;
        private Mock<IHttpClientCaller<Photo>> _client;
        private Mock<IPhotoMapper> _mapper;
        private Mock<IValidator> _validator;
        private readonly string photoEndpoint = "/photos";

        [SetUp]
        public void Setup()
        {
            _client = new Mock<IHttpClientCaller<Photo>>();
            _client.Setup(x => x.GetApiCollection(It.IsAny<string>())).ReturnsAsync(MockPhotos());
            _mapper = new Mock<IPhotoMapper>();
            _validator = new Mock<IValidator>();

        }

        [Test]
        public async Task WhenICallPhotoService_WithValidPhotoId_IGetPhotos()
        {
            _validator.Setup(x => x.IsValid(It.IsAny<int>())).Returns(true);
            _photoService = new PhotoService(_client.Object, _mapper.Object, _validator.Object);
            List<PhotoDto> photos = await _photoService.GetAlbumPhotos(1);
            _client.Verify(x => x.GetApiCollection($"{photoEndpoint}?albumId=1"), Times.Once());
            _mapper.Verify(x => x.ToPhotoDto(It.IsAny<Photo>()), Times.Exactly(4));
            Assert.AreEqual(4, photos.Count);
        }

        [Test]
        public async Task WhenICallPhotoService_ToGetAllPhotos_IGetPhotos()
        {
            _photoService = new PhotoService(_client.Object, _mapper.Object, _validator.Object);
            List<PhotoDto> photos = await _photoService.GetPhotos();
            _client.Verify(x => x.GetApiCollection($"{photoEndpoint}"), Times.Once());
            _mapper.Verify(x => x.ToPhotoDto(It.IsAny<Photo>()), Times.Exactly(4));
            Assert.AreEqual(4, photos.Count);
        }


        [Test]
        public async Task WhenICallAlbumService_WithInvalidUserId_IGetDataValidationException()
        {
            _validator.Setup(x => x.IsValid(It.IsAny<int>())).Returns(false);
            _photoService = new PhotoService(_client.Object, _mapper.Object, _validator.Object);
            Assert.ThrowsAsync<DataValidationException>(() => _photoService.GetAlbumPhotos(-1));

        }

        private ICollection<Photo> MockPhotos()
        {
            return new List<Photo>
            {
              new Photo()
              {
                  Id = 1,
              },
              new Photo()
              {
                   Id = 2,
              },
              new Photo()
              {
                   Id = 3,
              },
              new Photo()
              {
                   Id = 4
              }
            };
        }
    }
}
