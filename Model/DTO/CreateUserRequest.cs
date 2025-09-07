namespace NurseRecordingSystem.Model.DTO
{
    public class CreateUserRequest
    {
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string Address { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;
        public int AuthId { get; set; }
    }
}
