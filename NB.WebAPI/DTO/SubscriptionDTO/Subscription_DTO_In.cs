using System;
using NyhedsBlog_Backend.Core.Models.Subscription;

namespace NB.WebAPI.DTO.SubscriptionDTO
{
    public class Subscription_DTO_In
    {
        public SubscriptionType Type { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}