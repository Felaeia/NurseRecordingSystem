namespace NurseRecordingSystem.Model.DTO
{
    public class LoginResponse
    {
        public int AuthId { get; set; }
        public string? UserName { get; set; } 
        public string? Email { get; set; } 
        public int Role { get; set; }

        //public int Token { get; set; } //JWT Token or Session Token

    }
}
