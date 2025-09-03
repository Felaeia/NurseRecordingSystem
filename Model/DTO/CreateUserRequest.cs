namespace NurseRecordingSystem.Model.DTO
{
    public class CreateUserRequest
    {
        public string FirstName { get; set; } 
        public string? MiddleName { get; set; }
        public string? Address { get; set; } 
        public string? LastName { get; set; } 
        public string? ContactNumber { get; set; } 
        public int AuthId { get; set; }
    }
}
