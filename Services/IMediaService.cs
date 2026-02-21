using Mediar.Models;
using Mediar.Repositories;

namespace Mediar.Services
{
    public interface IMediaService
    {
        Task<IEnumerable<Media>> GetMediaByAlbumId(int albumId);
        Task CreateMedia(Media media);
        Task DeleteMedia(int mediaId);
        Task<Media?> GetById(int mediaId);
    }
}
