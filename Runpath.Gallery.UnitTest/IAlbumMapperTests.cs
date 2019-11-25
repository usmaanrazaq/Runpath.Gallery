using NUnit.Framework;
using Runpath.Gallery.Domain.Models;
using Runpath.Gallery.Dto;
using Runpath.Gallery.Service.Mapper;
using System;

namespace Runpath.Gallery.UnitTest
{
    public class IAlbumMapperTests
    {
        private IAlbumMapper _mapper;
        [SetUp]
        public void Setup()
        {
            _mapper = new AlbumMapper();
        }

        [Test]
        public void WhenIMapAlbum_WithValidObject_IGetDto()
        {
            Album album = GetAlbum();
            AlbumDto dto = _mapper.ToAlbumDto(album);
            Assert.AreEqual(album.Title, dto.Title);
            Assert.AreEqual(album.Id, dto.Id);
        }

        [Test]
        public void WhenIMapAlbumWithInvalidObject_IGetException()
        {
            var exception = Assert.Throws<NullReferenceException>(
                () => _mapper.ToAlbumDto(null),
                "This should have thrown!");
        }

        private Album GetAlbum()
        {
            return new Album
            {
                Id = 2,
                Title = "Test Album",
                UserId = 2
            };
        }
    }
}