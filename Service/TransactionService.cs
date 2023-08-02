using PaymentGatewayAPI.Model;
using PaymentGatewayAPI.CKOBankSimulator;
using AutoMapper;
using PaymentGatewayAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace PaymentGatewayAPI.Service
{
    public class TransactionService : ITransactionService
    {
        private static List<Transaction> TransactionList = new List<Transaction>();
        private readonly IMapper _mapper;

        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TransactionService(IMapper mapper,DataContext dataContext,IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ServiceResponse<List<AllTransactionDto>>> GetAllTransaction()
        {           
            var serviceResponse = new ServiceResponse<List<AllTransactionDto>>();
            var AllTransactions = await _dataContext.Transactions.Where(x => x.Merchant.Id == GetUserId()).ToListAsync();
            serviceResponse.Data =  AllTransactions.Select(x => _mapper.Map<AllTransactionDto>(x)).ToList();            
            return serviceResponse;
        }

        public async Task<ServiceResponse<TransactionDto>> Pay(PayTransactionDto PayTransaction)
        {
            var serviceResponse = new ServiceResponse<TransactionDto>();

            var Transaction = _mapper.Map<Transaction>(PayTransaction);

            if (!validateDetails(Transaction))
            {                
                Transaction.Status = TransactionStatus.InvalidInput;
                Transaction.Message = "Please check the Card Details Entered";                            
            }
            else
            {
                var response = IntiatePayment(Transaction);
                if (response == null || !response.IsSuccess)
                {
                    Transaction.Status = TransactionStatus.Failed;
                    Transaction.Message = (response==null) ? "Transaction Failed" : response.Message;
                }
                else
                {
                    Transaction.Status = TransactionStatus.Success;
                    Transaction.Message = "Transaction Successful";
                }
            }
            Transaction.CardNo = Maskify(Transaction.CardNo);
            Transaction.Merchant = await _dataContext.Merchants.FirstOrDefaultAsync(x => x.Id == GetUserId());
            _dataContext.Transactions.Add(Transaction);
            await _dataContext.SaveChangesAsync();
            serviceResponse.Data =  _mapper.Map<TransactionDto>(Transaction);            
            return serviceResponse;
        }

        private bool validateDetails(Transaction Transaction)
        {
            string Name = Transaction.CardHolderName;
            string CardNo = Transaction.CardNo;          
            string ExpiryDate = Transaction.ExpiryDate;
            string CVV = Transaction.CVV;
            decimal Amount = Transaction.Amount;

            if(string.IsNullOrEmpty(Name))
            {
                return false;
            }
            else if(string.IsNullOrEmpty(CardNo) || CardNo.Length != 16 || !CardNo.All(char.IsDigit)) {
                return false;
            }
            else if(ExpiryDate.Length!=4 || !ExpiryDate.All(char.IsDigit) || !CheckExipryDate(ExpiryDate))
            {               
                return false;
            }
            else if (CVV.Length != 3 || !CVV.All(char.IsDigit))
            {
                return false;
            }
            else if(Amount <= 0.0m)
            {
                return false;
            }
            return true;
        }

        private bool CheckExipryDate(string ExpiryDate)
        {
            int Month = int.Parse(ExpiryDate.Substring(0, 2));
            int Year = int.Parse(ExpiryDate.Substring(2));
            int CurrentMonth = DateTime.UtcNow.Month;
            int CurrentYear = int.Parse(DateTime.UtcNow.ToString("yy"));

            if (Year > CurrentYear || (Year == CurrentYear && Month >= CurrentMonth))
            {
                return true;
            }
            return false;            
        }

        private CKOBankSimulator.BankResponse IntiatePayment(Transaction Transaction)
        {
            var response = CKOBankSimulator.CKOBankSimulator.ProcessPayment(Transaction);                    
            return response;
        }

        private string Maskify(string CardNo)
        {
            var hash = Enumerable.Repeat("#", CardNo.Length - 4);
            return String.Join("", hash) + CardNo.Substring(CardNo.Length - 4);
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
