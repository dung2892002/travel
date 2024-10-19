namespace Travel.Core.DTOs
{
    public class ChangePasswordRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ReNewPassword { get; set; } = string.Empty;
    }
}
