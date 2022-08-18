using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NB.EFCore.Entities;

namespace NB.EFCore
{
    public class NbContext : DbContext
    {
        public NbContext(DbContextOptions<NbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CategoryEntity>()
                .HasMany<PostEntity>(c => c.Posts)
                .WithOne(p => p.Category);

            modelBuilder.Entity<UserEntity>()
                .HasData(new UserEntity
                {
                    Id = 1,
                    Firstname = "Bobby",
                    Lastname = "Olsen",
                    Email = "bobby@olsen.as",
                    Username = "test@test.com",
                    Password = "test1234",
                    PhoneNumber = 42424242,
                    Role = 3
                });
            
            modelBuilder.Entity<PageEntity>()
                .HasData(new PageEntity
                {
                    Id = 1,
                    AuthorId = 1,
                    Title = "Forside",
                    Content = "Velkommen til forsiden, din ballademager :-)",
                    Date = DateTime.Now,
                    PrettyDescriptor = "frontpage"
                });

            modelBuilder.Entity<SubscriptionEntity>()
                .HasData(new SubscriptionEntity
                {
                    Id = 1,
                    Type = 0,
                    DateFrom = DateTime.MinValue,
                    DateTo = DateTime.MaxValue
                });

            modelBuilder.Entity<CustomerEntity>()
                .HasData(new CustomerEntity
                {
                    Id = 1,
                    Firstname = "Ole",
                    Lastname = "Bobbysen",
                    Email = "test@test.com",
                    Username = "test@test.com",
                    Password = "test1234",
                    PhoneNumber = 42042069,
                    SubscriptionId = 1
                });

            modelBuilder.Entity<CategoryEntity>()
                .HasData(new CategoryEntity {Id = 1, Title = "Kategori 1", Description = "Test"},
                    new CategoryEntity { Id = 2, Title = "Kategori 2", Description = "Test" },
                    new CategoryEntity { Id = 3, Title = "Kategori 3", Description = "Test" },
                    new CategoryEntity { Id = 4, Title = "Kategori 4", Description = "Test" },
                    new CategoryEntity { Id = 5, Title = "Kategori 5", Description = "Test" });

            List<PostEntity> generatedPosts = new List<PostEntity>();
            var nextId = 1;
            for (int i = 1; i <= 5; i++)
            {
                for (int x = 1; x <= 12; x++)
                {
                    generatedPosts.Add(new PostEntity
                    {
                        Id = nextId,
                        AuthorId = 1,
                        CategoryId = i,
                        Title = "Post " + x + " in Categori " + i,
                        Content = "Lorem ipsum dolor sit amet",
                        RequiredSubscription = 0,
                        Date = DateTime.Now,
                        FeaturedImageUrl = "https://variety.com/wp-content/uploads/2021/07/Rick-Astley-Never-Gonna-Give-You-Up.png"
                    });
                    nextId++;
                }
            }

            modelBuilder.Entity<PostEntity>()
                .HasData(generatedPosts);
        }

        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<SubscriptionEntity> Subscriptions { get; set; }
        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<PageEntity> Pages { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        
        public DbSet<UserEntity> Users { get; set; }
        
        
    }
}