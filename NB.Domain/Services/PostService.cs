using System;
using System.Collections.Generic;
using System.IO;
using NyhedsBlog_Backend.Core.IServices;
using NyhedsBlog_Backend.Core.Models;
using NyhedsBlog_Backend.Core.Models.Post;
using NyhedsBlog_Backend.Core.Models.Subscription;
using NyhedsBlog_Backend.Domain.IRepositories;

namespace NyhedsBlog_Backend.Domain.Services
{
    public class PostService : IPostService
    {
        //Custom Length Constraints
        private const int
            TitleMinimumLength = 10,
            TitleMaximumLength = 60;

        //Custom Length Errors
        private readonly string
            InvalidTitle = DomainStrings.InvalidData + " Title length must be over " + TitleMinimumLength +
                           " characters, and under " + TitleMaximumLength + " characters.";


        private readonly ICreateReadRepository<Post> _repo;

        public PostService(ICreateReadRepository<Post> repo)
        {
            _repo = repo;
        }

        public Post GetOneById(int id)
        {
            if (id > 0)
            {
                Console.WriteLine(_repo.GetById(id).RequiredSubscription);
                return _repo.GetById(id);
            }
            throw new InvalidDataException(DomainStrings.IdMustBeOverZero);
        }

        public List<Post> GetAll()
        {
            return _repo.GetAll() as List<Post>;
        }

        public Post CreatePost(Post p)
        {
            Console.Write(p.RequiredSubscription);
            return Validate(p) ? _repo.Create(p) : null;
        }

        public Post DeletePost(Post p)
        {
            if (p.Id > 0)
            {
                return _repo.Delete(p);
            }
            throw new InvalidDataException(DomainStrings.IdMustBeOverZero);
        }

        public Post UpdatePost(Post p)
        {
            return Validate(p) ? _repo.Update(p) : null;
        }
        public bool Validate(Post obj)
        {
            if (obj.Title.Length < TitleMinimumLength)
                throw new InvalidDataException(InvalidTitle);
            
            if (obj.Title.Length > TitleMaximumLength)
                throw new InvalidDataException(InvalidTitle);

            return true;
        }
    }
}