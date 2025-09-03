namespace NurseRecordingSystem.Class.Services.Authentication
{
    public class NurseAuthenticationService
    {
        private readonly string? _connectionString;

        public NurseAuthenticationService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        //Nurse Function: Login

        //Nurse Function: Forgot password

        //Nurse Function: Reset password

        //Nurse Function: Change password

        //Nurse Function: Send code to verification email

        //Nurse Function: Vereify code from email

        //Nurse Function: 

    }
}
