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
        private readonly ICreateUsersService _createUsersService;

        public UserController(ICreateUsersService createUsersService)
        {
            _createUsersService = createUsersService
                ?? throw new ArgumentNullException(nameof(createUsersService), "UserAuthentication cannot be null");
        }

        #region Post User
        /// <summary>
        /// Create authentication (login credentials) for a new user.
        /// </summary>
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateAuthentication([FromBody] CreateUserWithAuthenticationDTO request)
        {
            try
            {
                var authRequest = new CreateAuthenticationRequest
                {
                    UserName = request.UserName,
                    Password = request.Password,
                    Email = request.Email
                };

                var userRequest = new CreateUserRequest
                {
                    FirstName = request.FirstName,
                    MiddleName = request.MiddleName,
                    LastName = request.LastName,
                    Address = request.Address,
                    ContactNumber = request.ContactNumber
                };
                var authId = await _createUsersService.CreateUserAuthenticateAsync(authRequest, userRequest);
                //await _createUsersService.CreateUser(userRequest);
                return Ok(new { AuthId = authId, Message = "Authentication created successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
        #endregion

        #region Post ??? In Development LoL
        /// <summary>
        /// Create user profile linked to an authentication record.
        /// </summary>
        [HttpPost("create")]
        public IActionResult CreateUser([FromBody] CreateUserRequest user)
        {
            try
            {
                //_userAuth.CreateUser(user);
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
