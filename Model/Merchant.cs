namespace PaymentGatewayAPI.Model
{
    public class Merchant
    {
        public int Id { get; set; }
        public required string Name  { get; set; }
        public byte[] PasswordHash { get; set; } = new byte[0];
        public byte[] PasswordSalt { get; set; } = new byte[0];
    }
}
