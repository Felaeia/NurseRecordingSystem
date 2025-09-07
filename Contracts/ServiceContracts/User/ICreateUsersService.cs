using NurseRecordingSystem.Model.DTO;

namespace NurseRecordingSystem.Contracts.ServiceContracts.User
{
    public interface ICreateUsersService
    {
        Task<int> CreateUserAuthenticateAsync(CreateAuthenticationRequest authRequest, CreateUserRequest user);
        Task CreateUser(CreateUserRequest user);
    }
}
