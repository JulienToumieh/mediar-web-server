using Mediar.Models;
using Mediar.Repositories;

namespace Mediar.Services
{
    public class MediaService : IMediaService
    {
        private readonly IMediaRepository _mediaRepository;
        private readonly IAuthService _authService;
        private readonly IWebHostEnvironment _env;

        public MediaService(IMediaRepository mediaRepository, IAuthService authService, IWebHostEnvironment env)
        {
            _mediaRepository = mediaRepository;
            _authService = authService;
            _env = env;
        }

        public async Task<IEnumerable<Media>> GetMediaByAlbumId(int albumId)
        {
            var media = await _mediaRepository.GetByAlbumIdAsync(albumId);
            return media;
        }

        public async Task CreateMedia(Media media)
        {
            await _mediaRepository.CreateAsync(media);
        }

        public async Task<Media?> GetById(int mediaId)
        {
            return await _mediaRepository.GetByIdAsync(mediaId);
        }

        public async Task DeleteMedia(int mediaId)
        {
            var media = await _mediaRepository.GetByIdAsync(mediaId);

            if (media == null)
                return;

            var mediaFilePath = Path.Combine(_env.WebRootPath, media.Url.TrimStart('/'));
            if (File.Exists(mediaFilePath))
            {
                File.Delete(mediaFilePath);
            }
            await _mediaRepository.DeleteAsync(mediaId);
        }
    }
}
