using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Contracts.RepositoryContracts.User;
using NurseRecordingSystem.Model.DTO;

namespace NurseRecordingSystem.Class.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        //This method finds a user by their username
        public async Task<UserAuth> GetUserByUsernameAsync(string username)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT authId, userName, passwordHash, passwordSalt FROM [Auth] WHERE username = @username", connection))
            {
                cmd.Parameters.AddWithValue("@username", username);
                try
                {
                    await connection.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new UserAuth
                            {
                                AuthId = (int)reader["authId"],
                                UserName = reader["userName"].ToString()!,
                                PasswordHash = (byte[])reader["passwordHash"],
                                PasswordSalt = (byte[])reader["passwordSalt"]
                            };
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Database ERROR occured during fetching user by username", ex);

                }
            }
            return null!;
        }
    }
}
