using System;

namespace NB.WebAPI.DTO.PageDTO
{
    public class Page_DTO_In
    {
        public string Title { get; set; }
        public string PrettyDescriptor { get; set; }
        public string Content { get; set; }
        public int AuthorId { get; set; }
        public DateTime Date { get; set; }
    }
}