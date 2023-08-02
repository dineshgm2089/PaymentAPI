namespace PaymentGatewayAPI.CKOBankSimulator
{
    public class Customer
    {       
        public  int CustomerID { get; set; }
        public required string CardHolderName { get; set; }
        public required string CardNo { get; set; }
        public required string ExpiryDate { get; set; }
        public required string CVV { get; set; }
        public required decimal AvailableBalance { get; set; }       

    }
}
