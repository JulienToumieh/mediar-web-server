using Mediar.Models;
using Mediar.Repositories;
using Microsoft.AspNetCore.Mvc;
using Mediar.Services;
using Mediar.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Mediar.Controllers
{
    public class AlbumListController : Controller
    {
        private readonly IAlbumService _albumService;
        private readonly IWebHostEnvironment _env;

        public AlbumListController(IAlbumService albumService, IWebHostEnvironment env)
        {
            _albumService = albumService;
            _env = env;
        }

        [Authorize(Roles = "view,edit,delete,admin")]
        public async Task<IActionResult> AlbumList()
        {
            var albums = await _albumService.GetAllAlbums();

            return View(albums);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "edit,delete,admin")]
        public async Task<IActionResult> Create(CreateAlbumViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var albums = await _albumService.GetAllAlbums();
                return View("AlbumList", albums);
            }

            string coverImageUrl = "/media/albums/default-cover-image.png";

            if (model.CoverImage != null && model.CoverImage.Length > 0)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "media", "albums", "cover-images");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.CoverImage.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.CoverImage.CopyToAsync(stream);
                }

                coverImageUrl = $"/media/albums/cover-images/{fileName}";
            }

            var album = new Album
            {
                Name = model.Name,
                Description = model.Description,
                CoverImageUrl = coverImageUrl,
                DateCreated = DateTime.UtcNow,
            };

            await _albumService.CreateAlbum(album);

            return RedirectToAction(nameof(AlbumList));
        }
    }
}