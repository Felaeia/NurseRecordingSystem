namespace NurseRecordingSystem.Model.DTO
{
    public class UserAuth
    {
        public int AuthId { get; init; }
        public string UserName { get; init; } = null!;
        public byte[] PasswordHash { get; init; } = null!;
        public byte[] PasswordSalt { get; init; } = null!;
    }
}
