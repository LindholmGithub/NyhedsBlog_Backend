using System;
using NyhedsBlog_Backend.Core.Models.Post;
using NyhedsBlog_Backend.Core.Models.Subscription;
using NyhedsBlog_Backend.Core.Models.User;

namespace NB.WebAPI.DTO.PostDTO
{
    public class Post_DTO_In
    {
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string PrettyDescriptor { get; set; }
        
        public string FeaturedImageUrl { get; set; }
        public string Content { get; set; }
        public int AuthorId { get; set; }
        
        public int RequiredSubscription { get; set; }
        public DateTime Date { get; set; }
    }
}