using System.Collections.Generic;
using System.IO;
using NyhedsBlog_Backend.Core.IServices;
using NyhedsBlog_Backend.Core.Models.Post;
using NyhedsBlog_Backend.Domain.IRepositories;

namespace NyhedsBlog_Backend.Domain.Services
{
    public class CategoryService : ICategoryService
    {
        //Custom Length Constraints
        private const int
            TitleMinimumLength = 4,
            TitleMaximumLength = 60,
            UrlMinimumLength = 4,
            UrlMaximumLength = 20;

        //Custom Length Errors
        private readonly string
            InvalidTitle = DomainStrings.InvalidData + " Overskrifts længde skal være over " + TitleMinimumLength +
                           " bogstaver, og under " + TitleMaximumLength + " bogstaver.";
        
        private readonly string
            InvalidUrl = DomainStrings.InvalidData + " URL længden skal være over " + UrlMinimumLength +
                           " bogstaver, og under " + UrlMaximumLength + " bogstaver.";

        
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public Category GetOneById(int id)
        {
            if (id > 0)
            {
                return _repo.GetById(id);
            }
            throw new InvalidDataException(DomainStrings.IdMustBeOverZero);

        }

        public Category GetOneBySlug(string slug)
        {
            return _repo.GetOneBySlug(slug);
        }

        public List<Category> GetAll()
        {
            return _repo.GetAll() as List<Category>;
        }

        public Category CreateCategory(Category c)
        {
            return Validate(c) ? _repo.Create(c) : null;
        }

        public Category DeleteCategory(Category c)
        {
            if (c.Id > 0)
                return _repo.Delete(c);
            
            throw new InvalidDataException(DomainStrings.IdMustBeOverZero);

        }

        public Category UpdateCategory(Category c)
        {
            return Validate(c) ? _repo.Update(c) : null;
        }
        public bool Validate(Category obj)
        {
            if (obj.Title.Length < TitleMinimumLength)
                throw new InvalidDataException(InvalidTitle);
            
            if (obj.Title.Length > TitleMaximumLength)
                throw new InvalidDataException(InvalidTitle);

            if (obj.PrettyDescriptor.Length < UrlMinimumLength)
                throw new InvalidDataException(InvalidUrl);
            
            if (obj.PrettyDescriptor.Length > UrlMaximumLength)
                throw new InvalidDataException(InvalidUrl);

            return true;
        }
    }
}