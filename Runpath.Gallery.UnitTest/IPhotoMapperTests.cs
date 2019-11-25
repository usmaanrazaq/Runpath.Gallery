using NUnit.Framework;
using Runpath.Gallery.Domain.Models;
using Runpath.Gallery.Dto;
using Runpath.Gallery.Service.Mapper;
using System;

namespace Runpath.Gallery.UnitTest
{
    public class IPhotoMapperTests
    {
        private IPhotoMapper _mapper;
        [SetUp]
        public void Setup()
        {
            _mapper = new PhotoMapper(); ;
        }

        [Test]
        public void WhenIMapAlbum_WithValidObject_IGetDto()
        {
            Photo photo = GetPhoto();
            PhotoDto dto = _mapper.ToPhotoDto(photo);
            Assert.AreEqual(photo.Title, dto.Title);
            Assert.AreEqual(photo.Id, dto.Id);
            Assert.AreEqual(photo.ThumbnailUrl, dto.ThumbnailUrl);
            Assert.AreEqual(photo.Url, dto.Url);
        }

        [Test]
        public void WhenIMapAlbumWithInvalidObject_IGetException()
        {
            var exception = Assert.Throws<NullReferenceException>(
                () => _mapper.ToPhotoDto(null),
                "This should have thrown!");
        }

        private Photo GetPhoto()
        {
            return new Photo
            {
                Id = 2,
                Title = "Test Album",
                ThumbnailUrl = "test thumbnail",
                Url = "some url"
            };
        }
    }
}