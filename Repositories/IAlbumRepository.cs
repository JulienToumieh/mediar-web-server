using Mediar.Models;

namespace Mediar.Repositories
{
    public interface IAlbumRepository
    {
        Task<Album?> GetByIdAsync(int albumId);
        Task<IEnumerable<Album>> GetAllAsync();
        Task CreateAsync(Album album);
        Task UpdateAsync(Album album);
        Task DeleteAsync(int albumId);
    }
}
