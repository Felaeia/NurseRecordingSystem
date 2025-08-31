using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NurseRecordingSystem.Model.DatabaseModels;
using NurseRecordingSystem.Model.DTO;

namespace NurseRecordingSystem.Class.Authentication
{
    public class UserAuthenticationService
    {
        private readonly string? _connectionString;

        public UserAuthenticationService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        // Replace the return statement inside the Login method to return a string (e.g., a JWT token).
        // If you want to return the LoginResponse object, change the method signature to return LoginResponse instead of string.
        // Here is the fix assuming you want to return LoginResponse:

        public LoginResponse Login(LoginRequest request)
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
                    connection.Open();
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



        //TODO: Forgot Password Function
        //DO THIS SHIT AFTER YOU MAKE EMAIL WORK

        //TODO: Veryfy Password from Hash
        //public bool 

        // TODO: Implement JWT generation
        //private string GenerateJwtToken(string username, int role)
        
        //    return "fake-jwt-token";
        //}
    }
}
