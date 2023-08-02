using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace PaymentGatewayAPI.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) :base(options)
        {
                
        }
        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Merchant> Merchants { get; set; }


    }
}
