using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NurseRecordingSystem.Class.Services.UserServices;
using NurseRecordingSystem.Contracts.ServiceContracts.User;
using NurseRecordingSystem.Model.DTO;

namespace PresentationProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ICreateUsersService _userAuth;

        public UserController(ICreateUsersService userAuth)
        {
            _userAuth = userAuth
                ?? throw new ArgumentNullException(nameof(userAuth), "UserAuthentication cannot be null");
        }

        #region Post User Authentication
        /// <summary>
        /// Create authentication (login credentials) for a new user.
        /// </summary>
        [HttpPost("create-auth")]
        public async Task<IActionResult> CreateAuthentication([FromBody] CreateAuthenticationRequest request)
        {
            try
            {
                var authId = await _userAuth.CreateAuthenticationAsync(request);
                return Ok(new { AuthId = authId, Message = "Authentication created successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
        #endregion

        #region Post User
        /// <summary>
        /// Create user profile linked to an authentication record.
        /// </summary>
        [HttpPost("create-user")]
        public IActionResult CreateUser([FromBody] CreateUserRequest user)
        {
            try
            {
                _userAuth.CreateUser(user);
                return Ok(new { Message = "User created successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
        #endregion

    }
}
