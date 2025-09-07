using NurseRecordingSystem.Model.DTO;

namespace NurseRecordingSystem.Contracts.ServiceContracts.User
{
    public interface ICreateUsersService
    {
        Task<int> CreateAuthenticationAsync(CreateAuthenticationRequest authRequest);
        Task CreateUser(CreateUserRequest user);
    }
}
