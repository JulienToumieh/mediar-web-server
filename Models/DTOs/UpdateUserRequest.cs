using Microsoft.AspNetCore.Http;

namespace Mediar.Models.DTOs
{
    public class UpdateUserRequest
    {
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? NewPassword { get; set; }
    }
}
