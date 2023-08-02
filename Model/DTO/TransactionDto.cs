namespace PaymentGatewayAPI.Model.DTO
{
    public class TransactionDto
    {
        public int ID { get; set; }
        public TransactionStatus Status { get; set; } = TransactionStatus.Processing;
        public string Message { get; set; } = String.Empty;
    }
}
