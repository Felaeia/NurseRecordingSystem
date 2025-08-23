namespace NurseRecordingSystem.Model.DTO
{
    public class CreateUserRequest
    {
        public int FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string Address { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;
    }
}
