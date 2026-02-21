using Mediar.Models;
using Mediar.Repositories;
using Microsoft.AspNetCore.Mvc;
using Mediar.Services;
using Mediar.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Mediar.Controllers
{
    public class AlbumController : Controller
    {
        private readonly IMediaService _mediaService;
        private readonly IWebHostEnvironment _env;
        private readonly IAlbumService _albumService;

        public AlbumController(IAlbumService albumService, IMediaService mediaService, IWebHostEnvironment env)
        {
            _mediaService = mediaService;
            _albumService = albumService;
            _env = env;
        }

        [Authorize(Roles = "view,edit,delete,admin")]
        public async Task<IActionResult> Album(int id)
        {
            var album = await _albumService.GetAlbumDetails(id);
            var medias = await _mediaService.GetMediaByAlbumId(id);

            if (album == null)
                return NotFound();

            var viewModel = new AlbumDetailsViewModel
            {
                Album = album,
                Media = medias
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "edit,delete,admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMediaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var album = await _albumService.GetAlbumDetails(model.AlbumId);
                var medias = await _mediaService.GetMediaByAlbumId(model.AlbumId);

                if (album == null)
                    return NotFound();

                var viewModel = new AlbumDetailsViewModel
                {
                    Album = album,
                    Media = medias
                };

                return View(viewModel);
            }

            string imageUrl = "/media/albums/default-cover-image.png";

            if (model.MediaImage != null && model.MediaImage.Length > 0)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "media", "albums", model.AlbumId.ToString());

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.MediaImage.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.MediaImage.CopyToAsync(stream);
                }

                imageUrl = $"/media/albums/{model.AlbumId}/{fileName}";
            }

            Console.WriteLine(imageUrl);
            
            var media = new Media
            {
                Url = imageUrl,
                AlbumId = model.AlbumId
            };

            await _mediaService.CreateMedia(media);

            return RedirectToAction(nameof(Album), new { id = model.AlbumId });
        }

        [HttpPost]
        [Authorize(Roles = "delete,admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var album = await _albumService.GetAlbumDetails(id);
            if (album == null)
            {
                return NotFound();
            }

            await _albumService.DeleteAlbum(id);

            return RedirectToAction("AlbumList", "AlbumList");

        }

        [HttpPost]
        [Authorize(Roles = "edit,delete,admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string name, string description, IFormFile coverImage)
        {
            Console.WriteLine(id);
            var album = await _albumService.GetAlbumDetails(id);
            if (album == null)
            {
                return NotFound();
            }

            album.Name = name;
            album.Description = description;
            
            if (coverImage != null && coverImage.Length > 0)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "media", "albums", "cover-images");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(coverImage.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await coverImage.CopyToAsync(stream);
                }

                album.CoverImageUrl = $"/media/albums/cover-images/{fileName}";
            }

            await _albumService.UpdateAlbum(album);

            return RedirectToAction("Album", new { id = album.Id });
        }

        [HttpPost]
        [Authorize(Roles = "delete,admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMedia(int id)
        {
            var media = await _mediaService.GetById(id);

            if (media == null) 
                return NotFound();

            await _mediaService.DeleteMedia(id);

            return RedirectToAction("Album", new { id = media.AlbumId });
        }
    }
}