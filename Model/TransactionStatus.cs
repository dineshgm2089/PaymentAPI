using System.Text.Json.Serialization;

namespace PaymentGatewayAPI.Model
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TransactionStatus
    {
        Processing = 1,
        Success = 2,
        Failed = 3,     
        InvalidInput = 4,
    }
}
