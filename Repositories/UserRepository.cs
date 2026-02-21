using Mediar.Models;
using Microsoft.VisualBasic;
using Npgsql;

namespace Mediar.Repositories
{
    public class UserRepository : IUserRepository
    {
        private IConfiguration _configuration;
        private readonly string _connectionString;

        private NpgsqlDataSource dataSource;  

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");;
            
            dataSource = NpgsqlDataSource.Create(_connectionString);
        }

        public async Task CreateAsync(User user)
        {
            await using (var command = dataSource.CreateCommand("INSERT INTO public.\"Users\" (\"Name\", \"Email\", \"Password\", \"Permission\") VALUES (@Name, @Email, @Password, @Permission)"))
            {
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@Permission", user.Permission);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new NpgsqlCommand("SELECT \"Id\", \"Name\", \"Email\", \"Password\", \"Permission\" FROM public.\"Users\" WHERE \"Id\" = @Id", connection);
                command.Parameters.AddWithValue("@Id", userId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var user = new User
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Password = reader.GetString(reader.GetOrdinal("Password")),
                            Permission = reader.GetString(reader.GetOrdinal("Permission")),
                        };

                        return user;
                    }

                    return null;
                }
            }
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new NpgsqlCommand("SELECT \"Id\", \"Name\", \"Email\", \"Password\", \"Permission\" FROM public.\"Users\" WHERE \"Email\" = @Email", connection);
                command.Parameters.AddWithValue("@Email", email);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var user = new User
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Password = reader.GetString(reader.GetOrdinal("Password")),
                            Permission = reader.GetString(reader.GetOrdinal("Permission")),
                        };

                        return user;
                    }

                    return null;
                }
            }
        }

        public async Task UpdateAsync(User user)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new NpgsqlCommand(
                    "UPDATE public.\"Users\" SET \"Name\" = @Name, \"Email\" = @Email, \"Password\" = @Password, \"Permission\" = @Permission WHERE \"Id\" = @Id",
                    connection
                );

                command.Parameters.AddWithValue("@Id", user.Id);
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@Permission", user.Permission);                
                
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteAsync(int userId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new NpgsqlCommand(
                    "DELETE FROM public.\"Users\" WHERE \"Id\" = @Id",
                    connection
                );

                command.Parameters.AddWithValue("@Id", userId);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<string?> GetPasswordAsync(string email)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new NpgsqlCommand("SELECT \"Password\" FROM public.\"Users\" WHERE \"Email\" = @Email", connection);
                command.Parameters.AddWithValue("@Email", email);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var password = reader.GetString(reader.GetOrdinal("Password"));

                        return password;
                    }

                    return null;
                }
            }
        }

        public async Task<bool> AdminExistsAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new NpgsqlCommand("SELECT 1 FROM public.\"Users\" WHERE \"Permission\" = @Permission LIMIT 1", connection);
                command.Parameters.AddWithValue("@Permission", "admin");

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return true;
                    }

                    return false;
                }
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = new List<User>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("SELECT \"Id\", \"Name\", \"Email\", \"Permission\" FROM public.\"Users\"", connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        users.Add(new User
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Password = "",
                            Permission = reader.GetString(reader.GetOrdinal("Permission"))
                        });
                    }
                }
            }

            return users;
        }
    }
}