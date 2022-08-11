using System;
using NyhedsBlog_Backend.Core.Models.Post;
using NyhedsBlog_Backend.Core.Models.Subscription;
using NyhedsBlog_Backend.Core.Models.User;

namespace NB.EFCore.Entities
{
    public class PostEntity
    {
        public int Id { get; set; }
        
        public CategoryEntity Category { get; set; }
        
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public UserEntity Author { get; set; }
        
        public int UserId { get; set; }
        
        public SubscriptionType RequiredSubscription { get; set; }
        public DateTime Date { get; set; }
    }
}