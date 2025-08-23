using Microsoft.AspNetCore.Mvc;
using NurseRecordingSystem.Class.Authentication;
using NurseRecordingSystem.Model.DatabaseModels;
using NurseRecordingSystem.Model.DTO;

namespace NurseRecordingSystem.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserAuthenticationService _userAuth;

        public AuthController(UserAuthenticationService userAuth)
        {
            _userAuth = userAuth 
                ?? throw new ArgumentNullException(nameof(userAuth), "UserAuthentication cannot be null");
        }

        /// <summary>
        /// Create authentication (login credentials) for a new user.
        /// </summary>
        [HttpPost("create-auth")]
        public IActionResult CreateAuthentication([FromBody] CreateAuthenticationRequest request)
        {
            try
            {
                var authId = _userAuth.CreateAuthentication(request);
                return Ok(new { AuthId = authId, Message = "Authentication created successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Create user profile linked to an authentication record.
        /// </summary>
        [HttpPost("create-user")]
        public IActionResult CreateUser([FromBody] UserModel user)
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
    }
}
