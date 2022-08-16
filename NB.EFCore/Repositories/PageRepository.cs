using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using NB.EFCore.Entities;
using NyhedsBlog_Backend.Core.Models;
using NyhedsBlog_Backend.Core.Models.User;
using NyhedsBlog_Backend.Domain.IRepositories;

namespace NB.EFCore.Repositories
{
    public class PageRepository : ICreateReadRepository<Page>
    {
        private readonly NbContext _ctx;

        public PageRepository(NbContext ctx)
        {
            _ctx = ctx;
        }


        public Page Create(Page obj)
        {
            var newEntity = _ctx.Pages.Add(new PageEntity
            {
                Title = obj.Title,
                PrettyDescriptor = obj.PrettyDescriptor,
                AuthorId = obj.Author.Id,
                Content = obj.Content,
                Date = obj.Date
            }).Entity;
            _ctx.SaveChanges();
            
            return GetById(newEntity.Id);
        }

        public Page Update(Page obj)
        {
            var newEntity = new PageEntity
            {
                Id = obj.Id,
                Title = obj.Title,
                PrettyDescriptor = obj.PrettyDescriptor,
                AuthorId = obj.Author.Id,
                Content = obj.Content,
                Date = obj.Date
            };
            _ctx.ChangeTracker.Clear();
            _ctx.Pages.Update(newEntity);
            _ctx.SaveChanges();

            return GetById(obj.Id);
        }

        public Page Delete(Page obj)
        {
            var entity = GetById(obj.Id);
            _ctx.Pages.Remove(new PageEntity {Id = obj.Id});
            _ctx.SaveChanges();

            return entity;
        }

        public Page GetById(int id)
        {
            return Conversion().FirstOrDefault(post => post.Id == id) ??
                   throw new FileNotFoundException(RepositoryStrings.IdNotFound);
        }

        public IEnumerable<Page> GetAll()
        {
            return Conversion().ToList();
        }

        public IEnumerable<Page> Search(string term)
        {
            return Conversion().Where(page => page.Title == term).ToList();
        }

        private IQueryable<Page> Conversion()
        {
            return _ctx.Pages
                .Include(entity => entity.Author)
                .Select(page => new Page()
                {
                    Id = page.Id,
                    Title = page.Title,
                    PrettyDescriptor = page.PrettyDescriptor,
                    Author = new User
                    {
                        Id = page.Author.Id,
                        Email = page.Author.Email,
                        Firstname = page.Author.Firstname,
                        Lastname = page.Author.Lastname,
                        PhoneNumber = page.Author.PhoneNumber,
                        Username = page.Author.Username,
                        Password = page.Author.Password,
                        Role = (UserRole) page.Author.Role
                    },
                    Content = page.Content,
                    Date = page.Date
                });
        }
        
    }
}