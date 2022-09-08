using System;
using NB.WebAPI.DTO.CategoryDTO;
using NB.WebAPI.DTO.UserDTO;
using NyhedsBlog_Backend.Core.Models.Post;
using NyhedsBlog_Backend.Core.Models.Subscription;
using NyhedsBlog_Backend.Core.Models.User;

namespace NB.WebAPI.DTO.PostDTO
{
    public class Post_DTO_Out
    {
        public int Id { get; set; }
        
        public Category_DTO_Out Category { get; set; }
        public string Title { get; set; }
        public string PrettyDescriptor { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string Content { get; set; }
        public User_DTO_Out Author { get; set; }
        
        public bool Authorized { get; set; }
        public int RequiredSubscription { get; set; }
        public DateTime Date { get; set; }
    }
}