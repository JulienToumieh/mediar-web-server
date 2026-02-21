using Microsoft.AspNetCore.Http;

namespace Mediar.Models.DTOs
{
    public class UpdateAlbumRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? CoverImage { get; set; }
    }
}
