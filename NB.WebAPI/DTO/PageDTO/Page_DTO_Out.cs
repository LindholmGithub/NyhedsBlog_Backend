using System;
using NB.WebAPI.DTO.UserDTO;
using NyhedsBlog_Backend.Core.Models.User;

namespace NB.WebAPI.DTO.PageDTO
{
    public class Page_DTO_Out
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PrettyDescriptor { get; set; }
        public string Content { get; set; }
        public User_DTO_Out Author { get; set; }
        public DateTime Date { get; set; }
    }
}