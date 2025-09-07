using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Contracts.ServiceContracts.User;
using NurseRecordingSystem.Model.DTO;

namespace NurseRecordingSystem.Class.Services.UserServices
{
    public class CreateUsersServices : ICreateUsersService
    {
        private readonly string? _connectionString;

        public CreateUsersServices(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        //Create Auth for User Function (role = user)
        public async Task<int> CreateAuthenticationAsync(CreateAuthenticationRequest authRequest)
        {
            if (authRequest == null)
            {
                throw new ArgumentNullException(nameof(authRequest), "Authentication cannot be null");
            }
            //Integer(1) = User Access
            var role = 1;
            await using (var connection = new SqlConnection(_connectionString))
            await using(var cmdUserAuth =
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
                    await connection.OpenAsync();
                    var result = await cmdUserAuth.ExecuteScalarAsync();
                    if (result == null || result == DBNull.Value)
                    {
                        throw new Exception("Failed to retrieve the new authentication ID.");
                    }

                    return (int)result;
                }
                catch (SqlException ex)
                {
                    throw new ArgumentException("Database ERROR occured during creating AUTHENTICATION", ex);
                }
            }
        }

        //Create User Function 
        public async Task CreateUser(CreateUserRequest user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }
            await using(var connection = new SqlConnection(_connectionString))
            await using(var cmdCreateUser =
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
                    await connection.OpenAsync();
                    await cmdCreateUser.ExecuteNonQueryAsync();
                }
                catch (SqlException ex)
                {
                    throw new Exception("Database ERROR occured during creating during USER", ex);
                }
            }
        }
    }
}
