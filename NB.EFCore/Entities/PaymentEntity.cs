using System;

namespace NB.EFCore.Entities
{
    public class PaymentEntity
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public double Amount { get; set; }
        public PostEntity Post { get; set; }
        public int PostId { get; set; }
        public CustomerEntity Customer { get; set; }
        public int CustomerId { get; set; }
        public int Status { get; set; }
        
        public string PaymentLink { get; set; }
    }
}