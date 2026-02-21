using Mediar.Models;
using Mediar.Models.DTOs;
using Mediar.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mediar.Controllers.Api
{
    [ApiController]
    [Route("api/albums")]
    public class AlbumListApiController : ControllerBase
    {
        private readonly IAlbumService _albumService;
        private readonly IWebHostEnvironment _env;

        public AlbumListApiController(
            IAlbumService albumService,
            IWebHostEnvironment env)
        {
            _albumService = albumService;
            _env = env;
        }

        [HttpGet]
        [Authorize(Roles = "view,edit,delete,admin")]
        public async Task<IActionResult> GetAlbums()
        {
            var albums = await _albumService.GetAllAlbums();
            return Ok(albums);
        }

        [HttpPost]
        [Authorize(Roles = "edit,delete,admin")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateAlbum(
            [FromForm] CreateAlbumRequest request)
        {
            string coverImageUrl = "/media/albums/default-cover-image.png";

            if (request.CoverImage != null && request.CoverImage.Length > 0)
            {
                string uploadsFolder = Path.Combine(
                    _env.WebRootPath,
                    "media",
                    "albums",
                    "cover-images");

                Directory.CreateDirectory(uploadsFolder);

                string fileName = Guid.NewGuid() + Path.GetExtension(request.CoverImage.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await request.CoverImage.CopyToAsync(stream);

                coverImageUrl = $"/media/albums/cover-images/{fileName}";
            }

            var album = new Album
            {
                Name = request.Name,
                Description = request.Description,
                CoverImageUrl = coverImageUrl,
                DateCreated = DateTime.UtcNow
            };

            await _albumService.CreateAlbum(album);

            return CreatedAtAction(
                nameof(GetAlbums),
                new { id = album.Id },
                album);
        }

    }
}
