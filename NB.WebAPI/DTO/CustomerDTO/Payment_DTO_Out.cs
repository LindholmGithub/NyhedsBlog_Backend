using System;

namespace NB.WebAPI.DTO.CustomerDTO
{
    public class Payment_DTO_Out
    {
        public DateTime Timestamp { get; set; }
        public double Amount { get; set; }
        public int PostId { get; set; }
    }
}