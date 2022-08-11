using System;

namespace NyhedsBlog_Backend.Core.Models
{
    public class Page
    {
        public int Id { get; set; }
        public string PrettyDescriptor { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public User.User Author { get; set; }
        public DateTime Date { get; set; }
    }
}