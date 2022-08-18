using System.Collections.Generic;
using System.IO;
using System.Linq;
using NB.EFCore.Entities;
using NyhedsBlog_Backend.Core.Models;
using NyhedsBlog_Backend.Core.Models.Post;
using NyhedsBlog_Backend.Core.Models.Subscription;
using NyhedsBlog_Backend.Core.Models.User;
using NyhedsBlog_Backend.Domain.IRepositories;

namespace NB.EFCore.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly NbContext _ctx;
        
        public CategoryRepository(NbContext ctx)
        {
            _ctx = ctx;
        }
        
        public Category Create(Category obj)
        {
            var newEntity = _ctx.Categories.Add(new CategoryEntity
            {
                Title = obj.Title,
                Description = obj.Description,
                PrettyDescriptor = obj.PrettyDescriptor,
            }).Entity;
            _ctx.SaveChanges();
            return GetById(newEntity.Id);
        }

        public Category Update(Category obj)
        {
            var old = GetById(obj.Id);
            obj.Posts = old.Posts;
            var newEntity = new CategoryEntity
            {
                Id = obj.Id,
                Description = obj.Description,
                Title = obj.Title,
                PrettyDescriptor = obj.PrettyDescriptor,
                Posts = obj.Posts.Select(o => new PostEntity
                {
                    Id = o.Id
                }).ToList()
            };
            _ctx.ChangeTracker.Clear();
            _ctx.Categories.Update(newEntity);
            _ctx.SaveChanges();

            return GetById(obj.Id);
        }

        public Category Delete(Category obj)
        {
            var entity = GetById(obj.Id);
            _ctx.Categories.Remove(new CategoryEntity {Id = obj.Id});
            _ctx.SaveChanges();

            return entity;
        }

        public Category GetById(int id)
        {
            return Conversion().FirstOrDefault(cat => cat.Id == id) ??
                   throw new FileNotFoundException(RepositoryStrings.IdNotFound);
        }

        public IEnumerable<Category> GetAll()
        {
            return Conversion().ToList();
        }

        public IEnumerable<Category> Search(string term)
        {
            return Conversion().Where(cat => cat.Title == term).ToList();
        }

        public Category GetOneBySlug(string slug)
        {
            return Conversion().FirstOrDefault(cat => cat.PrettyDescriptor == slug) ??
                   throw new FileNotFoundException(RepositoryStrings.IdNotFound);
        }

        private IQueryable<Category> Conversion()
        {
            return _ctx.Categories
                .Select(cat => new Category
                {
                    Id = cat.Id,
                    Description = cat.Description,
                    Title = cat.Title,
                    PrettyDescriptor = cat.PrettyDescriptor,
                    Posts = cat.Posts.Select(p => new Post
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Content = p.Content,
                        FeaturedImageUrl = p.FeaturedImageUrl,
                        Date = p.Date,
                        RequiredSubscription = (SubscriptionType) p.RequiredSubscription,
                        Category = new Category
                        {
                            Id = p.Category.Id,
                            Description = p.Category.Description,
                            PrettyDescriptor = p.Category.PrettyDescriptor,
                            Title = p.Category.Title
                        },
                        Author = new User
                        {
                            Id = p.Author.Id,
                            Firstname = p.Author.Firstname,
                            Lastname = p.Author.Lastname,
                            Email = p.Author.Email,
                            Username = p.Author.Username,
                            Password = p.Author.Password,
                            PhoneNumber = p.Author.PhoneNumber,
                            Role = (UserRole) p.Author.Role
                        }
                    }).ToList()
                });
        }
    }
}