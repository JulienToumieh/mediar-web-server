using Mediar.Models;
using Microsoft.EntityFrameworkCore;

namespace Mediar.Repositories
{
    public class MediaRepository : IMediaRepository
    {
        private readonly AppDbContext _context;

        public MediaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Media?> GetByIdAsync(int mediaId)
        {
            return await _context.Media
                .FirstOrDefaultAsync(m => m.Id == mediaId);
        }

        public async Task<IEnumerable<Media>> GetByAlbumIdAsync(int albumId)
        {
            return await _context.Media
                .Where(m => m.AlbumId == albumId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Media>> GetAllAsync()
        {
            return await _context.Media
                .ToListAsync();
        }

        public async Task CreateAsync(Media media)
        {
            await _context.Media.AddAsync(media);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int mediaId)
        {
            var media = await _context.Media
                .FirstOrDefaultAsync(m => m.Id == mediaId);
            
            if (media != null)
            {
                _context.Media.Remove(media);
                await _context.SaveChangesAsync();
            }
        }
    }
}