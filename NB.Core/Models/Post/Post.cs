﻿using System;
using NyhedsBlog_Backend.Core.Models.Post;
using NyhedsBlog_Backend.Core.Models.Subscription;
using NyhedsBlog_Backend.Core.Models.User;

namespace NyhedsBlog_Backend.Core.Models.Post
{
    public class Post
    {
        public int Id { get; set; }
        
        public Category Category { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public User.User Author { get; set; }
        
        public SubscriptionType RequiredSubscription { get; set; }
        public DateTime Date { get; set; }
    }
}