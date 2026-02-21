using System.ComponentModel.DataAnnotations;

namespace Mediar.Models.DTOs
{
    public class RegisterRequest
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
        
        [Required]  
        public string Permission { get; set; } = null!;
    }
}
