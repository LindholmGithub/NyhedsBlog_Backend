using NyhedsBlog_Backend.Core.Models.User;

namespace NB.WebAPI.DTO.UserDTO
{
    public class User_DTO_Out
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
    }
}