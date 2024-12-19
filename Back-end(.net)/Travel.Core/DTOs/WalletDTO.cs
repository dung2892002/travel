namespace Travel.Core.DTOs
{
    public class WalletDTO
    {
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
        public string BankNumber { get; set; } = null!;
        public string BankName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Username { get; set; } = null!;
    }
}
