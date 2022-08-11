using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NB.EFCore.Entities;
using NyhedsBlog_Backend.Core.Models.Subscription;
using NyhedsBlog_Backend.Domain.IRepositories;

namespace NB.EFCore.Repositories
{
    public class SubscriptionRepository : ICreateReadRepository<Subscription>
    {
        private readonly NbContext _ctx;

        public SubscriptionRepository(NbContext ctx)
        {
            _ctx = ctx;
        }

        public Subscription Create(Subscription obj)
        {
            var newEntity = _ctx.Subscriptions.Add(new SubscriptionEntity
            {
                Type = (int) obj.Type,
                DateFrom = obj.DateFrom,
                DateTo = obj.DateTo
            }).Entity;
            _ctx.SaveChanges();
            return GetById(newEntity.Id);
        }

        public Subscription Update(Subscription obj)
        {
            var newEntity = new SubscriptionEntity
            {
                Type = (int) obj.Type,
                DateFrom = obj.DateFrom,
                DateTo = obj.DateTo
            };
            _ctx.ChangeTracker.Clear();
            _ctx.Subscriptions.Update(newEntity);
            _ctx.SaveChanges();

            return GetById(obj.Id);
        }

        public Subscription Delete(Subscription obj)
        {
            var entity = GetById(obj.Id);
            _ctx.Subscriptions.Remove(new SubscriptionEntity{Id = obj.Id});
            _ctx.SaveChanges();

            return entity;
        }

        public Subscription GetById(int id)
        {
            return Conversion().FirstOrDefault(subscription => subscription.Id == id) ??
                   throw new FileNotFoundException(RepositoryStrings.IdNotFound);
        }

        public IEnumerable<Subscription> GetAll()
        {
            return Conversion().ToList();
        }

        public IEnumerable<Subscription> Search(string term)
        {
            return Conversion().ToList();
        }

        private IEnumerable<Subscription> Conversion()
        {
            return _ctx.Subscriptions
                .AsEnumerable()
                .Select(sub =>
                {
                    SubscriptionType type = (SubscriptionType) sub.Type;

                    return new Subscription
                    {
                        Id = sub.Id,
                        Type = type,
                        DateFrom = sub.DateFrom,
                        DateTo = sub.DateTo
                    };
                });
        }
        
    }
}