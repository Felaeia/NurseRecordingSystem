using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NurseRecordingSystem.Class.Services.HelperServices;
using NurseRecordingSystem.Contracts.RepositoryContracts.User;
using NurseRecordingSystem.Contracts.ServiceContracts.Auth;
using NurseRecordingSystem.Model.DatabaseModels;
using NurseRecordingSystem.Model.DTO;
using System;

namespace NurseRecordingSystem.Class.Services.Authentication
{
    public class UserAuthenticationService : IUserAuthServices
    {
        private readonly string? _connectionString;
        private readonly IUserRepository _userRepository;

        //Dependency Injection of IConfiguration and IUserRepository
        public UserAuthenticationService(IConfiguration configuration, IUserRepository userRepository)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            _userRepository = userRepository
                ?? throw new ArgumentNullException(nameof(userRepository),"UserAuth Service cannot be null");
        }

        //User Method: Login
        public async Task<LoginResponse> AuthenticateAsync(LoginRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "LoginRequest cannot be Null");
            }

            using (var connection = new SqlConnection(_connectionString))
            using (var cmdLoginUser = new SqlCommand("SELECT authId, userName, password, email, role FROM Auth WHERE userName = @userName", connection))
            {
                cmdLoginUser.Parameters.AddWithValue("@userName", request.UserName);

                try
                {
                    await connection.OpenAsync();
                    using (var reader = cmdLoginUser.ExecuteReader())
                    {
                        //Note: During Login Method should pass the plain text password and compare with the hashed password in 
                        if (reader.Read())
                        {
                            string? storedPassword = reader["password"].ToString();

                            if (storedPassword == request.Password) // TODO: use hashing here
                            {
                                return new LoginResponse
                                {
                                    AuthId = (int)reader["authId"],
                                    UserName = reader["userName"].ToString()!,
                                    Email = reader["email"].ToString()!,
                                    Role = (int)reader["role"]
                                    // Token = ...
                                };
                            }
                        }
                    }

                }
                catch (SqlException ex)
                {
                    throw new Exception("Database ERROR occured during login", ex);
                }

                throw new UnauthorizedAccessException("Invalid username or password.");
            }
        }

        //User Function: To Determine the role of the user
        public async Task<int> DetermineRoleAync(LoginResponse response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response), "LoginResponse cannot be Null");
            }
            var user = await _userRepository.GetUserByUsernameAsync(response.UserName);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }
            return user.Role;
        }

        //User Method: Logout (fishballs need session tokens)
        public async Task LogoutAsync()
        {
            // Implement logout logic if needed (e.g., invalidate tokens, clear session data)
            await Task.CompletedTask;
        }
    }
}
