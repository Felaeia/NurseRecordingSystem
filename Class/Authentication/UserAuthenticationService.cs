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

        //Create Auth Function (role = user)
        public int CreateAuthentication(CreateAuthenticationRequest authRequest)
        {
            if (authRequest == null)
            {
                throw new ArgumentNullException(nameof(authRequest), "Authentication cannot be null");
            }
            //Integer(1) = User Access
            var role = 1;
            using (var connection = new SqlConnection(_connectionString))
            using (var cmdUserAuth = 
                new SqlCommand("INSERT INTO [Auth] (userName, password, email, role) " +
                "VALUES (@userName, @password, @email, @role);" +
                "SELECT CAST(SCOPE_IDENTITY() as int);", connection))
            {
                cmdUserAuth.Parameters.AddWithValue("@userName", authRequest.UserName);
                cmdUserAuth.Parameters.AddWithValue("@password", authRequest.Password);
                cmdUserAuth.Parameters.AddWithValue("@email", authRequest.Email);
                cmdUserAuth.Parameters.AddWithValue("@role", role);
                try
                {
                    connection.Open();

                    // Fix: ExecuteScalar() returns object, which can be null. Cast after null check.
                    var result = cmdUserAuth.ExecuteScalar();
                    if (result == null || result == DBNull.Value)
                    {
                        throw new Exception("Failed to retrieve the new authentication ID.");
                    }

                    return (int)result;
                }
                catch(SqlException ex)
                {
                    throw new ArgumentException("Database ERROR occured during creating AUTHENTICATION", ex);
                }
                //Redundant Close() call removed, as 'using' statement handles it automatically.
                //connection.Close();

            }
        }

        //Create User Function 
        public void CreateUser(CreateUserRequest user)
        {
            if (user == null)
                {
                    throw new ArgumentNullException(nameof(user), "User cannot be null");
                }
            using (var connection = new SqlConnection(_connectionString))
            using (var cmdCreateUser =
                new SqlCommand("INSERT INTO [Users] (authId, firstName, middleName, lastName, contactNumber, address) " +
                "VALUES (@authId, @firstName, @middleName, @lastName, @contactNumber, @address)", connection))
            {
                cmdCreateUser.Parameters.AddWithValue("@firstName", user.FirstName);
                cmdCreateUser.Parameters.AddWithValue("@middleName", user.MiddleName);
                cmdCreateUser.Parameters.AddWithValue("@lastName", user.LastName);
                cmdCreateUser.Parameters.AddWithValue("@contactNumber", user.ContactNumber);
                cmdCreateUser.Parameters.AddWithValue("@address", user.Address);
                cmdCreateUser.Parameters.AddWithValue("@authId", user.AuthId);
                try
                {
                    connection.Open();
                    cmdCreateUser.ExecuteNonQuery();
                } catch (SqlException ex)
                {
                    throw new Exception("Database ERROR occured during creating during USER", ex);
                }
                //Redundant Close() call removed, as 'using' statement handles it automatically.
                //connection.Close();
            }

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

        //TODO: Veryfy Password from Hash
        //private bool VerifyPassword(string plainPassword, string storedHash)
        //{
        //    // Example: BCrypt verification
        //    return BCrypt.Net.BCrypt.Verify(plainPassword, storedHash);
        //}

        // TODO: Implement JWT generation
        //private string GenerateJwtToken(string username, int role)
        
        //    return "fake-jwt-token";
        //}
    }
}
