using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using NyhedsBlog_Backend.Core.IServices;
using NyhedsBlog_Backend.Core.Models.User;
using NyhedsBlog_Backend.Domain.IRepositories;

namespace NyhedsBlog_Backend.Domain.Services
{
    public class UserService : IUserService
    {
        private const int
            UsernameMinimumLength = 4,
            PasswordMinimumLength = 6;
        
        private readonly string
            InvalidUsername = DomainStrings.InvalidData + " Username length must be over " + UsernameMinimumLength +
                              " characters!",
            InvalidPassword = DomainStrings.InvalidData + " Password length must be over " + PasswordMinimumLength +
                              " characters!",
            InvalidEmail = DomainStrings.InvalidData + " Email must be longer than zero and be correctly formatted.",
            InvalidFirstName = DomainStrings.InvalidData + " First Name length must be over zero",
            InvalidLastName = DomainStrings.InvalidData + " Last Name length must be over zero",
            InvalidPhoneNumber = DomainStrings.InvalidData + " Phone number length must be over zero and under nine.",
            InvalidLogin = "Invalid username and/or password!";
        
        private readonly ICreateReadRepository<User> _repo;

        public UserService(ICreateReadRepository<User> repo)
        {
            _repo = repo;
        }

        public User GetOneById(int id)
        {
            if (id > 0)
            {
                return _repo.GetById(id);
            }
            throw new InvalidDataException(DomainStrings.IdMustBeOverZero);
        }

        public List<User> GetAll()
        {
            return _repo.GetAll() as List<User>;
        }

        public User CreateUser(User u)
        {
            return Validate(u) ? _repo.Create(u) : null;
        }

        public User DeleteUser(User u)
        {
            if (u.Id > 0)
            {
                return _repo.Delete(u);
            }
            throw new InvalidDataException(DomainStrings.IdMustBeOverZero);
        }

        public User UpdateUser(User u)
        {
            return Validate(u) ? _repo.Update(u) : null;
        }

        public User Validate(string username, string password)
        {
            List<User> allUsers = GetAll();

            var check = allUsers.Where(u => u.Username == username && u.Password == password).ToList();

            return check.Any() ? check[0] : null;
        }

        public bool Validate(User obj)
        {
            if (obj.Username.Length < UsernameMinimumLength)
                throw new InvalidDataException(InvalidUsername);

            if (obj.Password.Length < PasswordMinimumLength)
                throw new InvalidDataException(InvalidPassword);
            
            if (obj.Email.Length <= 0 || !new EmailAddressAttribute().IsValid(obj.Email))
                throw new InvalidDataException(InvalidEmail);
            
            if (obj.Firstname.Length <= 0)
                throw new InvalidDataException(InvalidFirstName);

            if (obj.Lastname.Length <= 0)
                throw new InvalidDataException(InvalidLastName);

            if (obj.PhoneNumber is not (> 0 and <= 999999999))
                throw new InvalidDataException(InvalidPhoneNumber);

            return true;
        }
    }
}