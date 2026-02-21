using Mediar.Models;
using Mediar.Models.DTOs;
using Mediar.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mediar.Controllers.Api
{
    [ApiController]
    [Route("api/album")]
    public class AlbumApiController : ControllerBase
    {
        private readonly IAlbumService _albumService;
        private readonly IMediaService _mediaService;
        private readonly IWebHostEnvironment _env;

        public AlbumApiController(
            IAlbumService albumService,
            IMediaService mediaService,
            IWebHostEnvironment env)
        {
            _albumService = albumService;
            _mediaService = mediaService;
            _env = env;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "view,edit,delete,admin")]
        public async Task<IActionResult> GetAlbum(int id)
        {
            var album = await _albumService.GetAlbumDetails(id);
            if (album == null)
                return NotFound();

            var media = await _mediaService.GetMediaByAlbumId(id);

            return Ok(new
            {
                album,
                media
            });
        }

        [HttpPost("{albumId}/media")]
        [Authorize(Roles = "edit,delete,admin")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateMedia(
            int albumId,
            [FromForm] IFormFile? mediaImage)
        {
            string imageUrl = "/media/albums/default-cover-image.png";

            if (mediaImage != null && mediaImage.Length > 0)
            {
                string uploadsFolder = Path.Combine(
                    _env.WebRootPath,
                    "media",
                    "albums",
                    albumId.ToString());

                Directory.CreateDirectory(uploadsFolder);

                string fileName = Guid.NewGuid() + Path.GetExtension(mediaImage.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await mediaImage.CopyToAsync(stream);

                imageUrl = $"/media/albums/{albumId}/{fileName}";
            }

            var media = new Media
            {
                AlbumId = albumId,
                Url = imageUrl
            };

            await _mediaService.CreateMedia(media);

            return CreatedAtAction(nameof(GetAlbum), new { id = albumId }, media);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "edit,delete,admin")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateAlbum(
            int id,
            [FromForm] UpdateAlbumRequest request)
        {
            var album = await _albumService.GetAlbumDetails(id);
            if (album == null)
                return NotFound();

            album.Name = request.Name ?? album.Name;
            album.Description = request.Description ?? album.Description;

            if (request.CoverImage != null)
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

                album.CoverImageUrl = $"/media/albums/cover-images/{fileName}";
            }

            await _albumService.UpdateAlbum(album);
            return Ok(album);
        }
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "delete,admin")]
        public async Task<IActionResult> DeleteAlbum(int id)
        {
            var album = await _albumService.GetAlbumDetails(id);
            if (album == null)
                return NotFound();

            await _albumService.DeleteAlbum(id);
            return NoContent();
        }

        [HttpDelete("media/{id}")]
        [Authorize(Roles = "delete,admin")]
        public async Task<IActionResult> DeleteMedia(int id)
        {
            var media = await _mediaService.GetById(id);
            if (media == null)
                return NotFound();

            await _mediaService.DeleteMedia(id);
            return NoContent();
        }


    }
}
