using System;

namespace NyhedsBlog_Backend.Core.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        //public Enum Type { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}