using Mediar.Models;

namespace Mediar.Repositories
{
    public interface IMediaRepository
    {
        Task<Media?> GetByIdAsync(int mediaId);
        Task<IEnumerable<Media>> GetByAlbumIdAsync(int albumId);
        Task<IEnumerable<Media>> GetAllAsync();
        Task CreateAsync(Media media);
        Task DeleteAsync(int mediaId);
    }
}
