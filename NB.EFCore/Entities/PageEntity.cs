using System;

namespace NB.EFCore.Entities
{
    public class PageEntity
    {
        public int Id { get; set; }
        public string PrettyDescriptor { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public UserEntity Author { get; set; }
        public int AuthorId { get; set; }
        public DateTime Date { get; set; }
    }
}