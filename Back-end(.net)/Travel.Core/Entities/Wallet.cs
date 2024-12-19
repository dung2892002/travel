using System.Text.Json.Serialization;

namespace Travel.Core.Entities
{
    public class Wallet
    {
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
        public string BankNumber { get; set; } = null!;
        public string BankName { get; set; } = null!;


        [JsonIgnore]
        public virtual User? User { get; set; }
    }
}
