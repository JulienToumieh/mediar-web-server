using Mediar.Models;

namespace Mediar.ViewModels
{
    public class AlbumDetailsViewModel
    {
        public Album Album { get; set; } = null!;
        public IEnumerable<Media> Media { get; set; } = Enumerable.Empty<Media>();
    }
}
