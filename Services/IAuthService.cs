using Mediar.Models;
using Mediar.Repositories;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace Mediar.Services
{
    public interface IAuthService
    {
        string HashPassword(string password);
        bool ValidatePassword(string enteredPassword, string storedPassword);
        string GenerateJWTToken(int id, string name, string email, string Permission);
    }
}
