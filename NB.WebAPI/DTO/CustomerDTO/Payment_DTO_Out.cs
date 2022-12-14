using System;
using NyhedsBlog_Backend.Core.Models.Customer;

namespace NB.WebAPI.DTO.CustomerDTO
{
    public class Payment_DTO_Out
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public double Amount { get; set; }
        public int PostId { get; set; }
        
        public int Status { get; set; }
        
        public string PaymentLink { get; set; }
    }
}