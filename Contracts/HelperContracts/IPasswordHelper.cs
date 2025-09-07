namespace NurseRecordingSystem.Contracts.HelperContracts
{
    public interface IPasswordHelper
    {
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] storedPasswordHash, byte[] storedPasswordSalt);
    }
}
