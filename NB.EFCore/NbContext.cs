using Microsoft.EntityFrameworkCore;
using NB.EFCore.Entities;

namespace NB.EFCore
{
    public class NbContext : DbContext
    {
        public NbContext(DbContextOptions<NbContext> options) : base(options)
        {
            
        }
        
        public DbSet<CustomerEntity> Customers { get; set; }
        
        public DbSet<SubscriptionEntity> Subscriptions { get; set; }
    }
}