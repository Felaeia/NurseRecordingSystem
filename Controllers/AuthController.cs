﻿using Microsoft.AspNetCore.Mvc;
using NurseRecordingSystem.Contracts.ControllerContracts;
using NurseRecordingSystem.Contracts.ServiceContracts.Auth;
using NurseRecordingSystem.Model.DTO.HelperDTOs;

namespace NurseRecordingSystem.Controllers
{
    public class AuthController : Controller , IAuthController
    {
        private readonly IUserAuthenticationService _userAuthenticationService;

        public AuthController(IUserAuthenticationService userAuthenticationService)
        {
            _userAuthenticationService = userAuthenticationService
                ?? throw new ArgumentNullException(nameof(userAuthenticationService), "UserAuthentication cannot be null");
        }

        #region User Login (My eyes are huritng TT)
        /// <summary>
        /// Create user profile linked to an authentication record.
        /// </summary>
        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequestDTO loginUser)
        {
            try
            {
                var response = await _userAuthenticationService.AuthenticateAsync(new LoginRequestDTO
                {
                    Email = loginUser.Email,
                    Password = loginUser.Password
                });

                if (response == null)
                {
                    return Unauthorized("Invalid credentials.");
                }

                return Ok(new { Response = response ,Message = "Login Succesful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error in Login: {ex.Message}");
            }
        }
        #endregion
    }
}
