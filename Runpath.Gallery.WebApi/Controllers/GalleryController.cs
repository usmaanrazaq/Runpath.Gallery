using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Runpath.Gallery.Dto;
using Runpath.Gallery.Service;
using System.Threading.Tasks;

namespace Runpath.Gallery.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class GalleryController : AppBaseController
    {
        private readonly IGalleryService _galleryService;

        public GalleryController(ILoggerFactory loggerFactory, IGalleryService galleryService) : base(loggerFactory.CreateLogger(nameof(GalleryController)))
        {
            _galleryService = galleryService;
        }

        [HttpGet]
        public async Task<ActionResult<GalleryDto>> Get()
        {
            return await Execute("An error occurred trying to load the gallery.", async () =>
            {
                var result = await _galleryService.GetGallery();
                return Ok(result);
            });
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<GalleryUserDto>> Get(int userId)
        {
            return await Execute("An error occurred trying to load the users gallery.", async () =>
             {
                 var result = await _galleryService.GetUsersGallery(userId);
                 return Ok(result);
             });
        }
    }
}