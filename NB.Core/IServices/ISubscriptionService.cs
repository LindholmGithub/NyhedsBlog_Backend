using System.Collections.Generic;
using NyhedsBlog_Backend.Core.Models.Subscription;

namespace NyhedsBlog_Backend.Core.IServices
{
    public interface ISubscriptionService
    {
        public Subscription GetOneById(int id);
        public List<Subscription> GetAll();
        public Subscription CreateSubscription(Subscription s);
        public Subscription DeleteSubscription(Subscription s);
        public Subscription UpdateSubscription(Subscription s);
    }
}