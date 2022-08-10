using System;

namespace NB.EFCore.Entities
{
    public class SubscriptionEntity
    {
        public int Id { get; set; }
        //public Enum Type { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}