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
        public int Status { get; set; }
    }
}