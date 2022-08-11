using System;
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
        //public DbSet<PostEntity> Posts { get; set; }
        
        public DbSet<UserEntity> Users { get; set; }
        
        public DbSet<CategoryEntity> Category { get; set; }
    }
}