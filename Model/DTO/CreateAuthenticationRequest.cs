namespace NurseRecordingSystem.Model.DTO
{
    public class CreateAuthenticationRequest
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;

    }
}
