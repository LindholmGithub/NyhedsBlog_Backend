﻿namespace NyhedsBlog_Backend.Core.Models
{
    public class Customer
    {
        
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        
        public string Address { get; set; }
        
        public int Zipcode { get; set; }
        
        public string City { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        
        public Subscription.Subscription Subscription { get; set; }
    }
}