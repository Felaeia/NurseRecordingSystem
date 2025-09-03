namespace NurseRecordingSystem.Model.DTO
{
    public class UserAuth
    {
        public int AuthId { get; init; }
        public string? UserName { get; init; }
        public byte[]? PasswordHash { get; init; }
        public byte[]? PasswordSalt { get; init; }
        public string? Email { get; init; }
        public int Role { get; init; }
    }
}
