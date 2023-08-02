using AutoMapper;

namespace PaymentGatewayAPI
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Transaction, TransactionDto>().ReverseMap();
            CreateMap<Transaction, AllTransactionDto>().ReverseMap();
            CreateMap<Transaction, PayTransactionDto>().ReverseMap();
        }
    }
}
