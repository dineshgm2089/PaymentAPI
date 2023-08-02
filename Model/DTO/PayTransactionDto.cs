using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGatewayAPI.Model.DTO
{
    public class PayTransactionDto
    {
        public required string CardHolderName { get; set; }
        public required string CardNo { get; set; }
        public required string ExpiryDate { get; set; }
        public required string CVV { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public required decimal Amount { get; set; }
        public required string Currency { get; set; }
    }
}
