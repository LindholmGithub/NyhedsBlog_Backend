using System;
using NyhedsBlog_Backend.Core.Models.Post;
using NyhedsBlog_Backend.Core.Models.Subscription;
using NyhedsBlog_Backend.Core.Models.User;

namespace NB.WebAPI.DTO.PostDTO
{
    public class Post_DTO_Out
    {
        public int Id { get; set; }
        
        public Category Category { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public User Author { get; set; }
        
        public SubscriptionType RequiredSubscription { get; set; }
        public DateTime Date { get; set; }
    }
}