using NurseRecordingSystem.Model.DTO;

namespace NurseRecordingSystem.Contracts.ServiceContracts.User
{
    public interface IUserFormService
    {
        Task<UserFormResponse> CreateUserForm(UserFormRequest userForm, string userId, string creator);
    }
}
