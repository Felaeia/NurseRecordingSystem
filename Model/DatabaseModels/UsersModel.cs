namespace NurseRecordingSystem.Model.DatabaseModels
{
    public class UserModel
    {
        public int userId { get; set; }
        public int authId { get; set; }
        public string firstName { get; set; } = null!;
        public string middleName { get; set; } = null!;
        public string lastName { get; set; } = null!;
        public int contactNumber { get; set; }
        public string address { get; set; } = null!;

    }
}
