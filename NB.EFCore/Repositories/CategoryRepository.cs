using System.Collections.Generic;
using System.IO;
using System.Linq;
using NB.EFCore.Entities;
using NyhedsBlog_Backend.Core.Models;
using NyhedsBlog_Backend.Core.Models.Post;
using NyhedsBlog_Backend.Domain.IRepositories;

namespace NB.EFCore.Repositories
{
    public class CategoryRepository : ICreateReadRepository<Category>
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
                Description = obj.Description
            }).Entity;
            _ctx.SaveChanges();
            return GetById(newEntity.Id);
        }

        public Category Update(Category obj)
        {
            var newEntity = new CategoryEntity
            {
                Id = obj.Id,
                Description = obj.Description,
                Title = obj.Title
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

        private IQueryable<Category> Conversion()
        {
            return _ctx.Categories
                .Select(cat => new Category
                {
                    Id = cat.Id,
                    Description = cat.Description,
                    Title = cat.Title
                });
        }
    }
}