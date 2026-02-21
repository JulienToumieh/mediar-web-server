using Mediar.Models;
using Mediar.Repositories;

namespace Mediar.Services
{
    public interface IAlbumService
    {
        Task<IEnumerable<Album>> GetAllAlbums();
        Task<Album?> GetAlbumDetails(int albumId);
        Task CreateAlbum(Album album);
        Task DeleteAlbum(int albumId);
        Task UpdateAlbum(Album album);
    }
}
