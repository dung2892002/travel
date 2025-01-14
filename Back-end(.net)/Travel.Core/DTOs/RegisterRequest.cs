﻿namespace Travel.Core.DTOs
{
    public class RegisterRequest
    {
        public string Fullname { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RePassword { get; set; } = string.Empty;
    }
}
