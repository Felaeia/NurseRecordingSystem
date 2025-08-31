using NurseRecordingSystem.Model.DTO;

namespace NurseRecordingSystem.Contracts.ServiceContracts.Auth
{
    public interface IUserAuthServices
    {
        Task<LoginResponse> AuthenticateAsync(LoginRequest request);
        Task<LoginResponse> Login(LoginRequest request);
    }
}
