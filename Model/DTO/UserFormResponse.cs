namespace NurseRecordingSystem.Model.DTO
{
    public class UserFormResponse
    {
        public bool IsSuccess { get; set; } 
        public int UserFormId { get; set; }
        public string Message { get; set; } = null!;

    }
}
