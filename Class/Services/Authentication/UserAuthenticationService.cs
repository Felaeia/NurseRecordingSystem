using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NurseRecordingSystem.Class.Services.HelperServices;
using NurseRecordingSystem.Contracts.RepositoryContracts.User;
using NurseRecordingSystem.Contracts.ServiceContracts.Auth;
using NurseRecordingSystem.Model.DatabaseModels;
using NurseRecordingSystem.Model.DTO.HelperDTOs;
using System;

namespace NurseRecordingSystem.Class.Services.Authentication
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly string? _connectionString;
        private readonly IUserRepository _userRepository;
        private readonly PasswordHelper _passwordHelper;


        //Dependency Injection of IConfiguration and IUserRepository
        public UserAuthenticationService(IConfiguration configuration, IUserRepository userRepository)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            _userRepository = userRepository
                ?? throw new ArgumentNullException(nameof(userRepository),"UserAuth Service cannot be null");
            //Chore: Change this into a Interface for better testing and flexibility
            _passwordHelper = new PasswordHelper();
        }

        //User Method: Login
        #region Login
        public async Task<LoginResponseDTO> AuthenticateAsync(LoginRequestDTO request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "LoginRequest cannot be Null");
            }

            using (var connection = new SqlConnection(_connectionString))
            using (var cmdLoginUser = new SqlCommand("SELECT authId, userName, passwordHash,passwordSalt, email, role FROM Auth WHERE email = @email", connection))
            {
                cmdLoginUser.Parameters.AddWithValue("@email", request.Email);
                try
                {

                    await connection.OpenAsync();
                    using (var reader = cmdLoginUser.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            if (_passwordHelper.VerifyPasswordHash(request.Password, (byte[])reader["passwordHash"], (byte[])reader["passwordSalt"]) == true && request.Email == (reader["email"].ToString())) // TODO: use hashing here
                            {
                                return new LoginResponseDTO
                                {
                                    AuthId = int.Parse(reader["authId"].ToString()!),
                                    UserName = reader["userName"].ToString()!,
                                    Email = reader["email"].ToString()!,
                                    Role = int.Parse(reader["role"].ToString()!)
                                };
                            }
                        }
                    }

                }
                catch (SqlException ex)
                {
                    throw new Exception("Database ERROR occured during login", ex);
                }

                throw new UnauthorizedAccessException("Invalid Email or Password.");
            }
        }
        #endregion


        //User Function: To Determine the role of the user
        public async Task<int> DetermineRoleAync(LoginResponseDTO response)
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
