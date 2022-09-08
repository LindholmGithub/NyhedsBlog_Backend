using System.Collections.Generic;
using System.IO;
using NyhedsBlog_Backend.Core.IServices;
using NyhedsBlog_Backend.Core.Models;
using NyhedsBlog_Backend.Domain.IRepositories;

namespace NyhedsBlog_Backend.Domain.Services
{
    public class PageService : IPageService
    {
        //Custom Length Constraints
        private const int
            TitleMinimumLength = 6,
            TitleMaximumLength = 60;

        //Custom Length Errors
        private readonly string
            InvalidTitle = DomainStrings.InvalidData + " Overskrifts længde skal være over " + TitleMinimumLength +
                           " tegn, og under " + TitleMaximumLength + " tegn.";


        private readonly IPageRepository _repo;

        public PageService(IPageRepository repo)
        {
            _repo = repo;
        }

        public Page GetOneById(int id)
        {
            if (id > 0)
            {
                return _repo.GetById(id);
            }
            throw new InvalidDataException(DomainStrings.IdMustBeOverZero);

        }

        public Page GetOneBySlug(string slug)
        {
            return _repo.GetOneBySlug(slug);
        }

        public List<Page> GetAll()
        {
            return _repo.GetAll() as List<Page>;
        }

        public Page CreatePage(Page p)
        {
            return Validate(p) ? _repo.Create(p) : null;
        }

        public Page DeletePage(Page p)
        {
            if (p.Id > 0)
            {
                return _repo.Delete(p);
            }
            throw new InvalidDataException(DomainStrings.IdMustBeOverZero);
        }

        public Page UpdatePage(Page p)
        {
            return Validate(p) ? _repo.Update(p) : null;
        }
        
        public bool Validate(Page obj)
        {
            if (obj.Title.Length < TitleMinimumLength)
                throw new InvalidDataException(InvalidTitle);
            
            if (obj.Title.Length > TitleMaximumLength)
                throw new InvalidDataException(InvalidTitle);

            return true;
        }
    }
}