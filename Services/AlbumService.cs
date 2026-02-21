using Mediar.Models;
using Mediar.Repositories;

namespace Mediar.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IAuthService _authService;
        private readonly IMediaService _mediaService;
        private readonly IWebHostEnvironment _env;

        public AlbumService(IAlbumRepository albumRepository, IMediaService mediaService, IAuthService authService, IWebHostEnvironment env)
        {
            _albumRepository = albumRepository;
            _authService = authService;
            _mediaService = mediaService;
            _env = env;
        }

        public async Task<IEnumerable<Album>> GetAllAlbums()
        {
            var albums = await _albumRepository.GetAllAsync();
            return albums;
        }

        public async Task<Album?> GetAlbumDetails(int albumId)
        {
            var album = await _albumRepository.GetByIdAsync(albumId);
            return album;
        }

        public async Task CreateAlbum(Album album)
        {
            await _albumRepository.CreateAsync(album);
        }

        public async Task DeleteAlbum(int albumId)
        {
            var mediaList = await _mediaService.GetMediaByAlbumId(albumId);
            foreach (var media in mediaList)
            {
                await _mediaService.DeleteMedia(media.Id);
            }

            var album = await _albumRepository.GetByIdAsync(albumId);

            if (album == null)
                return;

            var mediaFilePath = Path.Combine(_env.WebRootPath, album.CoverImageUrl.TrimStart('/'));
            if (File.Exists(mediaFilePath))
            {
                File.Delete(mediaFilePath);
            }

            await _albumRepository.DeleteAsync(albumId);
        }

        public async Task UpdateAlbum(Album album)
        {
            await _albumRepository.UpdateAsync(album);
        }

    }
}
