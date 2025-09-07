﻿namespace NurseRecordingSystem.Model.DTO.UserDTOs
{
    public class CreateUserRequestDTO
    {
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string Address { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;
        public int AuthId { get; set; }
    }
}
