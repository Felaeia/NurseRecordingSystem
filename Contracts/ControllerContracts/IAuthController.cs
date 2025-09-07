﻿using Microsoft.AspNetCore.Mvc;
using NurseRecordingSystem.Model.DTO.HelperDTOs;

namespace NurseRecordingSystem.Contracts.ControllerContracts
{
    public interface IAuthController
    {
        [HttpPost("Login")]
        Task<IActionResult> CreateUser([FromBody] LoginRequestDTO loginUser);
    }
}
