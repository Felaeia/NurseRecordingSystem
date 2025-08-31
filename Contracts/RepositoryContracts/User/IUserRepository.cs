using NurseRecordingSystem.Model.DTO;

namespace NurseRecordingSystem.Contracts.RepositoryContracts.User
{
    public interface IUserRepository
    {
        //This method finds a user by their username
        //Returns user object if found, otherwise null
        Task<UserAuth> GetUserByUsernameAsync(string username);
    }
}
