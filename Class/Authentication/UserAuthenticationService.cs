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
            using (var cmdAuth = 
                new SqlCommand("INSERT INTO [Auth] (userName, password, email, role) " +
                "VALUES (@userName, @password, @email, @role);" +
                "SELECT CAST(SCOPE_IDENTITY() as int);", connection))
            {
                cmdAuth.Parameters.AddWithValue("@userName", authRequest.UserName);
                cmdAuth.Parameters.AddWithValue("@password", authRequest.Password);
                cmdAuth.Parameters.AddWithValue("@email", authRequest.Email);
                cmdAuth.Parameters.AddWithValue("@role", role);
                try
                {
                    connection.Open();

                    // Fix: ExecuteScalar() returns object, which can be null. Cast after null check.
                    var result = cmdAuth.ExecuteScalar();
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
        public void CreateUser(UserModel user)
        {
            if (user == null)
                {
                    throw new ArgumentNullException(nameof(user), "User cannot be null");
                }
            using (var connection = new SqlConnection(_connectionString))
            using (var cmdUser =
                new SqlCommand("INSERT INTO [Users] (authId, firstName, middleName, lastName, contactNumber, address) " +
                "VALUES (@authId, @firstName, @middleName, @lastName, @contactNumber, @address)", connection))
            {
                cmdUser.Parameters.AddWithValue("@firstName", user.firstName);
                cmdUser.Parameters.AddWithValue("@middleName", user.middleName);
                cmdUser.Parameters.AddWithValue("@lastName", user.lastName);
                cmdUser.Parameters.AddWithValue("@contactNumber", user.contactNumber);
                cmdUser.Parameters.AddWithValue("@address", user.address);
                cmdUser.Parameters.AddWithValue("@authId", user.authId);
                try
                {
                    connection.Open();
                    cmdUser.ExecuteNonQuery();
                } catch (SqlException ex)
                {
                    throw new Exception("Database ERROR occured during creating during USER", ex);
                }
                //Redundant Close() call removed, as 'using' statement handles it automatically.
                //connection.Close();
            }

        }

        //Login User Function


        //Forgot Password Function
    }
}
