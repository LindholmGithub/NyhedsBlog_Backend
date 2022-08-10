using NyhedsBlog_Backend.Core.Models;

namespace NB.WebAPI.DTO.CustomerDTO
{
    public class Customer_DTO_Out
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Subscription Subscription { get; set; }
    }
}