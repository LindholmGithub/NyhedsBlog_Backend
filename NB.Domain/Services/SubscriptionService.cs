using System.Collections.Generic;
using System.IO;
using NyhedsBlog_Backend.Core.IServices;
using NyhedsBlog_Backend.Core.Models.Subscription;
using NyhedsBlog_Backend.Domain.IRepositories;

namespace NyhedsBlog_Backend.Domain.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ICreateReadRepository<Subscription> _repo;

        public SubscriptionService(ICreateReadRepository<Subscription> repo)
        {
            _repo = repo;
        }
        public Subscription GetOneById(int id)
        {
            if (id > 0)
                return _repo.GetById(id);
            throw new InvalidDataException(DomainStrings.IdMustBeOverZero);
        }

        public List<Subscription> GetAll()
        {
            return _repo.GetAll() as List<Subscription>;
        }

        public Subscription CreateSubscription(Subscription s)
        {
            return _repo.Create(s);
        }

        public Subscription DeleteSubscription(Subscription s)
        {
            if (s.Id > 0)
                return _repo.Delete(s);
            throw new InvalidDataException(DomainStrings.IdMustBeOverZero);
        }

        public Subscription UpdateSubscription(Subscription s)
        {
            return _repo.Update(s);
        }
    }
}