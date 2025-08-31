using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NurseRecordingSystem.Contracts.ServiceContracts.Auth;
using NurseRecordingSystem.Model.DatabaseModels;
using NurseRecordingSystem.Model.DTO;
using System;

namespace NurseRecordingSystem.Class.Services.Authentication
{
    public class UserAuthenticationService : IUserAuthServices
    {
        private readonly string? _connectionString;

        public UserAuthenticationService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        //Login Function
        public async Task<LoginResponse> Login(LoginRequest request)
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

        public async Task<LoginResponse> AuthenticateAsync(LoginRequest request)
        {
            return await Login(request); // Temporary implementation
            //var user = await 
            //if (request == null){
            //    throw new ArgumentException(nameof(request),"LoginRequest Cannot be Null");
            //}

        }
    }
}
