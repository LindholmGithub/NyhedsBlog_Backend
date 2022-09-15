using System;

namespace NyhedsBlog_Backend.Core.Models.Customer
{
    public class Payment
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public double Amount { get; set; }
        public Post.Post Post { get; set; }
        
        public PaymentStatus Status { get; set; }
    }
}