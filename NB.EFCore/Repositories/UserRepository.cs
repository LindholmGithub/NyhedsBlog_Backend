using System.Collections.Generic;
using System.IO;
using System.Linq;
using NB.EFCore.Entities;
using NyhedsBlog_Backend.Core.Models.User;
using NyhedsBlog_Backend.Domain.IRepositories;

namespace NB.EFCore.Repositories
{
    public class UserRepository : ICreateReadRepository<User>
    {
        private readonly NbContext _ctx;

        public UserRepository(NbContext ctx)
        {
            _ctx = ctx;
        }

        public User Create(User obj)
        {
            var newEntity = _ctx.Users.Add(new UserEntity
            {
                Firstname = obj.Firstname,
                Lastname = obj.Lastname,
                Email = obj.Email,
                PhoneNumber = obj.PhoneNumber,
                Username = obj.Username,
                Password = obj.Password,
                Role = (int) obj.Role
            }).Entity;
            _ctx.SaveChanges();

            return GetById(newEntity.Id);
        }

        public User Update(User obj)
        {
            var newEntity = new UserEntity
            {
                Id = obj.Id,
                Firstname = obj.Firstname,
                Lastname = obj.Lastname,
                Email = obj.Email,
                PhoneNumber = obj.PhoneNumber,
                Username = obj.Username,
                Password = obj.Password,
                Role = (int) obj.Role
            };
            _ctx.ChangeTracker.Clear();
            _ctx.Users.Update(newEntity);
            _ctx.SaveChanges();

            return GetById(obj.Id);
        }

        public User Delete(User obj)
        {
            var entity = GetById(obj.Id);
            _ctx.ChangeTracker.Clear();
            _ctx.Users.Remove(new UserEntity {Id = obj.Id});
            _ctx.SaveChanges();

            return entity;
        }

        public User GetById(int id)
        {
            return Conversion().FirstOrDefault(user => user.Id == id) ??
                   throw new FileNotFoundException(RepositoryStrings.IdNotFound);
        }

        public IEnumerable<User> GetAll()
        {
            return Conversion().ToList();
        }

        public IEnumerable<User> Search(string term)
        {
            return Conversion().Where(user => user.Username == term).ToList();
        }
        
        private IEnumerable<User> Conversion()
        {
            return _ctx.Users
                .AsEnumerable()
                .Select(user => new User
                {
                    Id = user.Id,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Username = user.Username,
                    Password = user.Password,
                    Role = (UserRole) user.Role
                });
        }
    }
}