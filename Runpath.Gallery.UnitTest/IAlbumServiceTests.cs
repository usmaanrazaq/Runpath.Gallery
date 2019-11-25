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
    public class IAlbumServiceTests
    {

        private IAlbumService _albumService;
        private Mock<IHttpClientCaller<Album>> _client;
        private Mock<IAlbumMapper> _mapper;
        private Mock<IValidator> _validator;
        private readonly string AlbumEndpoint = "/albums";

        [SetUp]
        public void Setup()
        {
            _client = new Mock<IHttpClientCaller<Album>>();
            _client.Setup(x => x.GetApiCollection(It.IsAny<string>())).ReturnsAsync(MockAlbums());
            _mapper = new Mock<IAlbumMapper>();
            _validator = new Mock<IValidator>();

        }

        [Test]
        public async Task WhenICallAlbumService_WithValidUserId_IGetAlbums()
        {
            _validator.Setup(x => x.IsValid(It.IsAny<int>())).Returns(true);
            _albumService = new AlbumService(_client.Object, _mapper.Object, _validator.Object);
            List<AlbumDto> albums = await _albumService.GetUserAlbums(1);
            _client.Verify(x => x.GetApiCollection($"{AlbumEndpoint}?userId={1}"), Times.Once());
            _mapper.Verify(x => x.ToAlbumDto(It.IsAny<Album>()), Times.Exactly(3));
            Assert.AreEqual(3, albums.Count);
        }

        [Test]
        public async Task WhenICallAlbumService_ToGetAllAlbums_IGetAlbums()
        {
            _albumService = new AlbumService(_client.Object, _mapper.Object, _validator.Object);
            List<AlbumDto> albums = await _albumService.GetAlbums();
            _client.Verify(x => x.GetApiCollection($"{AlbumEndpoint}"), Times.Once());
            _mapper.Verify(x => x.ToAlbumDto(It.IsAny<Album>()), Times.Exactly(3));
            Assert.AreEqual(3, albums.Count);
        }


        [Test]
        public async Task WhenICallAlbumService_WithInvalidUserId_IGetDataValidationException()
        {
            _validator.Setup(x => x.IsValid(It.IsAny<int>())).Returns(false);
            _albumService = new AlbumService(_client.Object, _mapper.Object, _validator.Object);
            Assert.ThrowsAsync<DataValidationException>(() => _albumService.GetUserAlbums(-1));

        }




        private ICollection<Album> MockAlbums()
        {
            return new List<Album>
            {
                new Album
                {
                    Id = 1,
                    UserId = 1,
                    Title = "Album 1"
                },
                new Album
                {
                    Id = 2,
                    UserId = 1,
                    Title = "Album 1"
                },
                new Album
                {
                    Id = 3,
                    UserId = 1,
                    Title = "Album 1"
                },

            };
        }
    }
}
