using System.Collections.Generic;

namespace NyhedsBlog_Backend.Core.Models.Post
{
    public class Category
    {
        public int Id { get; set; }
        public string PrettyDescriptor { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PrettyDescriptor { get; set; }
        public List<Post> Posts { get; set; }
    }
}