using System.Collections.Generic;
using NyhedsBlog_Backend.Core.Models.User;

namespace NyhedsBlog_Backend.Core.IServices
{
    public interface IUserService
    {
        public User GetOneById(int id); 
        public List<User> GetAll();
        public User CreateUser(User u);
        public User DeleteUser(User u);
        public User UpdateUser(User u);

        public User Validate(string username, string password);
    }
}