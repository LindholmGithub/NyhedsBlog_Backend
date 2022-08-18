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

            var numberOfCategories = 10;
            List<CategoryEntity> generatedCategories = new List<CategoryEntity>();

            for (int i = 1; i <= numberOfCategories; i++)
            {
                generatedCategories.Add(new CategoryEntity
                {
                    Id = i,
                    Title = "Kategori " + i,
                    Description = "Tester med seedet data",
                    PrettyDescriptor = "kategori-" + i
                });
            }

            modelBuilder.Entity<CategoryEntity>()
                .HasData(generatedCategories);

            List<PostEntity> generatedPosts = new List<PostEntity>();
            var nextId = 1;
            for (int i = 1; i <= numberOfCategories; i++)
            {
                for (int x = 1; x <= 12; x++)
                {
                    generatedPosts.Add(new PostEntity
                    {
                        Id = nextId,
                        AuthorId = 1,
                        CategoryId = i,
                        Title = "Post " + x + " in Categori " + i,
                        Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras a orci sapien. Vivamus erat quam, lobortis id tempor ac, condimentum ut turpis. Nullam vitae fermentum dolor. Ut et justo quis lacus hendrerit vulputate. Nam scelerisque nibh sed nunc rutrum porta vel tincidunt ex. Vestibulum ac nibh commodo, condimentum felis id, convallis nisi. Donec id sapien a orci malesuada vulputate. Nunc eu ultrices elit. Aliquam eget ligula euismod enim volutpat aliquam eu quis quam. Cras iaculis scelerisque neque, eget interdum magna. Donec porta tincidunt massa, non sodales erat facilisis eu. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras a orci sapien. Vivamus erat quam, lobortis id tempor ac, condimentum ut turpis. Nullam vitae fermentum dolor. Ut et justo quis lacus hendrerit vulputate. Nam scelerisque nibh sed nunc rutrum porta vel tincidunt ex. Vestibulum ac nibh commodo, condimentum felis id, convallis nisi. Donec id sapien a orci malesuada vulputate. Nunc eu ultrices elit. Aliquam eget ligula euismod enim volutpat aliquam eu quis quam. Cras iaculis scelerisque neque, eget interdum magna. Donec porta tincidunt massa, non sodales erat facilisis eu. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras a orci sapien. Vivamus erat quam, lobortis id tempor ac, condimentum ut turpis. Nullam vitae fermentum dolor. Ut et justo quis lacus hendrerit vulputate. Nam scelerisque nibh sed nunc rutrum porta vel tincidunt ex. Vestibulum ac nibh commodo, condimentum felis id, convallis nisi. Donec id sapien a orci malesuada vulputate. Nunc eu ultrices elit. Aliquam eget ligula euismod enim volutpat aliquam eu quis quam. Cras iaculis scelerisque neque, eget interdum magna. Donec porta tincidunt massa, non sodales erat facilisis eu. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras a orci sapien. Vivamus erat quam, lobortis id tempor ac, condimentum ut turpis. Nullam vitae fermentum dolor. Ut et justo quis lacus hendrerit vulputate. Nam scelerisque nibh sed nunc rutrum porta vel tincidunt ex. Vestibulum ac nibh commodo, condimentum felis id, convallis nisi. Donec id sapien a orci malesuada vulputate. Nunc eu ultrices elit. Aliquam eget ligula euismod enim volutpat aliquam eu quis quam. Cras iaculis scelerisque neque, eget interdum magna. Donec porta tincidunt massa, non sodales erat facilisis eu.",
                        RequiredSubscription = 0,
                        Date = DateTime.Now,
                        FeaturedImageUrl = "https://picsum.photos/800/300"
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