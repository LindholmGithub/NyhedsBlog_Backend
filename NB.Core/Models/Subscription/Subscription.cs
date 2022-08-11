using System;

namespace NyhedsBlog_Backend.Core.Models.Subscription
{
    public class Subscription
    {
        public int Id { get; set; }
        public SubscriptionType Type { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}