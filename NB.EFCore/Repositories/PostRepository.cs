using System;
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
    public class PostRepository : IPostRepository
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
                AuthorId = obj.Author.Id,
                CategoryId = obj.Category.Id,
                PrettyDescriptor = obj.PrettyDescriptor,
                FeaturedImageUrl = obj.FeaturedImageUrl,
                Content = obj.Content,
                Date = DateTime.Now,
                Paid = obj.Paid,
                Price = obj.Price
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
                AuthorId = obj.Author.Id,
                CategoryId = obj.Category.Id,
                PrettyDescriptor = obj.PrettyDescriptor,
                FeaturedImageUrl = obj.FeaturedImageUrl,
                Content = obj.Content,
                Date = DateTime.Now,
                Paid = obj.Paid,
                Price = obj.Price
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
            
            var unit = Conversion().FirstOrDefault(post => post.Id == id) ??
                   throw new FileNotFoundException(RepositoryStrings.IdNotFound);

            return unit;
        }

        public IEnumerable<Post> GetAll()
        {
            return Conversion().ToList();
        }

        public IEnumerable<Post> Search(string term)
        {
            return Conversion().Where(post => post.Title == term).ToList();
        }

        public Post GetOneBySlug(string slug)
        {
            var unit = Conversion().FirstOrDefault(post => post.PrettyDescriptor == slug) ??
                   throw new FileNotFoundException(RepositoryStrings.IdNotFound);
            return unit;
        }

        private IQueryable<Post> Conversion()
        {
            
            var posts = _ctx.Posts
                .Include(entity => entity.Author)
                .Include(entity => entity.Category)
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
                        Password = post.Author.Password,
                        Role = (UserRole) post.Author.Role
                    },
                    Category = new Category
                    {
                        Id = post.Category.Id,
                        Title = post.Category.Title,
                        Description = post.Category.Description,
                    },
                    PrettyDescriptor = post.PrettyDescriptor,
                    FeaturedImageUrl = post.FeaturedImageUrl,
                    Content = post.Content,
                    Date = post.Date,
                    Paid = post.Paid,
                    Price = post.Price
                });
            return posts;
        }
        
    }
}