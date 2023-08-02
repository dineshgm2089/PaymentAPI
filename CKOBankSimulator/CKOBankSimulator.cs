namespace PaymentGatewayAPI.CKOBankSimulator
{
    public class CKOBankSimulator
    {
        private static List<Customer> CustomersList = new List<Customer>
        {
            new Customer
            {
                CustomerID = 1, CardHolderName = "Peter Parker",CardNo = "4123456789123456",AvailableBalance = 100.00m, CVV = "123",ExpiryDate = "0225"
            },
             new Customer
            {
                CustomerID = 2, CardHolderName = "Tony Stark",CardNo = "4123456789123457",AvailableBalance = 150.20m, CVV = "234",ExpiryDate = "0824"
            },
              new Customer
            {
                CustomerID = 3, CardHolderName = "Bruce Wayne",CardNo = "4123456789123458",AvailableBalance = 115.32m, CVV = "345",ExpiryDate = "0823"
            },
        };

        public static BankResponse ProcessPayment(Transaction Transaction)
        {
            BankResponse bankResponse = new BankResponse();
            if (!VerifyCard(Transaction))
            {
                bankResponse.IsSuccess = false;
                bankResponse.Message = "Invalid Card Details";
                return bankResponse;
            }
            else if(!CheckBalanceAndDeduct(Transaction))
            {
                bankResponse.IsSuccess = false;
                bankResponse.Message = "Insufficient Funds";
                return bankResponse;
            }


            bankResponse.IsSuccess = true;
            bankResponse.Message = "Transaction Successful";
            return bankResponse;          

        }

        private static bool VerifyCard(Transaction Transaction)
        {
            var CustomerDetails = CustomersList.FirstOrDefault(c => c.CardNo == Transaction.CardNo);
            if (CustomerDetails == null)
            {
                return false;
            }
            else if(Transaction.CardHolderName.ToLower().Trim() != CustomerDetails.CardHolderName.ToLower().Trim()
                 || Transaction.CardNo != CustomerDetails.CardNo                
                 || Transaction.CVV != CustomerDetails.CVV
                 || Transaction.ExpiryDate != CustomerDetails.ExpiryDate)
            {
                return false;
            }
            return true;
        }

        private static bool CheckBalanceAndDeduct(Transaction Transaction)
        {
            var CustomerDetails = CustomersList.FirstOrDefault(c => c.CardNo == Transaction.CardNo)!;
            if (CustomerDetails.AvailableBalance < Transaction.Amount)
            {
                return false;
            }
            CustomerDetails.AvailableBalance = (CustomerDetails.AvailableBalance - Transaction.Amount);
            return true;
        }
    }
}
