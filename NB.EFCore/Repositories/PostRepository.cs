using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using NB.EFCore.Entities;
using NyhedsBlog_Backend.Core.Models;
using NyhedsBlog_Backend.Core.Models.Post;
using NyhedsBlog_Backend.Core.Models.User;
using NyhedsBlog_Backend.Domain.IRepositories;

namespace NB.EFCore.Repositories
{
    public class PostRepository : ICreateReadRepository<Post>
    {
        
        private readonly NbContext _ctx;

        public PostRepository(NbContext ctx)
        {
            _ctx = ctx;
        }

        public Post Create(Post obj)
        {
            var newEntity = _ctx.Posts.Add(new PostEntity
            {
                Title = obj.Title,
                UserId = obj.Author.Id,
                CategoryId = obj.Category.Id,
                Content = obj.Content,
                Date = obj.Date
            }).Entity;
            _ctx.SaveChanges();
            
            return GetById(newEntity.Id);
        }

        public Post Update(Post obj)
        {
            var newEntity = new PostEntity
            {
                Id = obj.Id,
                Title = obj.Title,
                UserId = obj.Author.Id,
                CategoryId = obj.Category.Id,
                Content = obj.Content,
                Date = obj.Date
            };
            _ctx.ChangeTracker.Clear();
            _ctx.Posts.Update(newEntity);
            _ctx.SaveChanges();

            return GetById(obj.Id);
        }

        public Post Delete(Post obj)
        {
            var entity = GetById(obj.Id);
            _ctx.Posts.Remove(new PostEntity {Id = obj.Id});
            _ctx.SaveChanges();

            return entity;
        }

        public Post GetById(int id)
        {
            return Conversion().FirstOrDefault(post => post.Id == id) ??
                   throw new FileNotFoundException(RepositoryStrings.IdNotFound);
        }

        public IEnumerable<Post> GetAll()
        {
            return Conversion().ToList();
        }

        public IEnumerable<Post> Search(string term)
        {
            return Conversion().Where(post => post.Title == term).ToList();
        }
        
        private IQueryable<Post> Conversion()
        {
            
            return _ctx.Posts
                .Select(post => new Post()
                {
                    Id = post.Id,
                    Title = post.Title,
                    Author = new User
                    {
                        Id = post.Author.Id,
                        Email = post.Author.Email,
                        Firstname = post.Author.Firstname,
                        Lastname = post.Author.Lastname,
                        PhoneNumber = post.Author.PhoneNumber,
                        Username = post.Author.Username,
                        Password = post.Author.Password
                    },
                    Category = new Category
                    {
                        Id = post.Category.Id,
                        Title = post.Category.Title,
                        Description = post.Category.Description,
                    },
                    Content = post.Content,
                    Date = post.Date
                });
        }
        
    }
}