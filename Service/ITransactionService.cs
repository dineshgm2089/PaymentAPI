using Microsoft.AspNetCore.Mvc;
using PaymentGatewayAPI.Model;

namespace PaymentGatewayAPI.Service
{
    public interface ITransactionService
    {
        Task<ServiceResponse<List<AllTransactionDto>>> GetAllTransaction();

        Task<ServiceResponse<TransactionDto>> Pay(PayTransactionDto request);
       
    }
}
