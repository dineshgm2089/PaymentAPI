namespace PaymentGatewayAPI.Service
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(Merchant Merchant,string Password);

        Task<ServiceResponse<string>> Login(string MerchantName, string Password);

        

    }
}
