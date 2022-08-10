using System;

namespace NB.WebAPI.DTO.PostDTO
{
    public class Post_DTO_In
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime Date { get; set; }
    }
}