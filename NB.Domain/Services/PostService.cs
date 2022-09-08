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
            TitleMaximumLength = 60,
            ContentMinimumLength = 400;

        //Custom Length Errors
        private readonly string
            InvalidTitle = DomainStrings.InvalidData + " Overskrifts længde skal være over " + TitleMinimumLength +
                           " tegn, og under " + TitleMaximumLength + " tegn.";
        
        private readonly string
            InvalidContent = DomainStrings.InvalidData + " Indholdets længde skal være over " + ContentMinimumLength +
                           " tegn.";


        private readonly IPostRepository _repo;

        public PostService(IPostRepository repo)
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

        public Post GetOneBySlug(string slug)
        {
            return _repo.GetOneBySlug(slug);
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

            if (obj.Content.Length < ContentMinimumLength)
                throw new InvalidDataException(InvalidContent);
            
            return true;
        }
    }
}