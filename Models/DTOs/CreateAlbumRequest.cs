using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Mediar.Models.DTOs
{
    public class CreateAlbumRequest
    {
        [Required]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public IFormFile? CoverImage { get; set; }
    }
}
