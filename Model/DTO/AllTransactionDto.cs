namespace PaymentGatewayAPI.Model.DTO
{
    public class AllTransactionDto
    {
        public required string CardHolderName { get; set; }
        public required string CardNo { get; set; }
        public required string ExpiryDate { get; set; }
        public required string CVV { get; set; }
        public required decimal Amount { get; set; }

        public required string Currency { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        public TransactionStatus Status { get; set; } = TransactionStatus.Processing;

        public string Message { get; set; } = String.Empty;
    }
}
