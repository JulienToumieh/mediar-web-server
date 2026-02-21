using Mediar.Models;
using Microsoft.EntityFrameworkCore;

namespace Mediar.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly AppDbContext _context;

        public AlbumRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Album?> GetByIdAsync(int albumId)
        {
            return await _context.Albums
                .FirstOrDefaultAsync(a => a.Id == albumId);
        }

        public async Task<IEnumerable<Album>> GetAllAsync()
        {
            return await _context.Albums
                .ToListAsync();
        }

        public async Task CreateAsync(Album album)
        {
            await _context.Albums.AddAsync(album);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Album album)
        {
            _context.Albums.Update(album);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int albumId)
        {
            var album = await _context.Albums
                .FirstOrDefaultAsync(a => a.Id == albumId);
            
            if (album != null)
            {
                _context.Albums.Remove(album);
                await _context.SaveChangesAsync();
            }
        }
    }
}