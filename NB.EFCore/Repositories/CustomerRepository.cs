using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NB.EFCore.Entities;
using NyhedsBlog_Backend.Core.Models;
using NyhedsBlog_Backend.Core.Models.Subscription;
using NyhedsBlog_Backend.Domain.IRepositories;

namespace NB.EFCore.Repositories
{
    public class CustomerRepository : ICreateReadRepository<Customer>
    {
        private readonly NbContext _ctx;

        public CustomerRepository(NbContext ctx)
        {
            _ctx = ctx;
        }

        public Customer Create(Customer obj)
        {
            var newEntity = _ctx.Customers.Add(new CustomerEntity
            {
                Firstname = obj.Firstname,
                Lastname = obj.Lastname,
                Address = obj.Address,
                Zipcode = obj.Zipcode,
                City = obj.City,
                Email = obj.Email,
                PhoneNumber = obj.PhoneNumber,
                Username = obj.Username,
                Password = obj.Password,
                SubscriptionId = obj.Subscription.Id
                
            }).Entity;
            _ctx.SaveChanges();

            return GetById(newEntity.Id);
        }

        public Customer Update(Customer obj)
        {
            var newEntity = new CustomerEntity
            {
                Id = obj.Id,
                Firstname = obj.Firstname,
                Lastname = obj.Lastname,
                Address = obj.Address,
                Zipcode = obj.Zipcode,
                City = obj.City,
                Email = obj.Email,
                PhoneNumber = obj.PhoneNumber,
                Username = obj.Username,
                Password = obj.Password,
                SubscriptionId = obj.Subscription.Id
            };
            _ctx.ChangeTracker.Clear();
            _ctx.Customers.Update(newEntity);
            _ctx.SaveChanges();

            return GetById(obj.Id);
        }

        public Customer Delete(Customer obj)
        {
            var entity = GetById(obj.Id);
            _ctx.Customers.Remove(new CustomerEntity {Id = obj.Id});
            _ctx.SaveChanges();

            return entity;
        }

        public Customer GetById(int id)
        {
            return Conversion().FirstOrDefault(customer => customer.Id == id) ??
                   throw new FileNotFoundException(RepositoryStrings.IdNotFound);
        }

        public IEnumerable<Customer> GetAll()
        {
            return Conversion().ToList();
        }

        public IEnumerable<Customer> Search(string term)
        {
            return Conversion().Where(customer => customer.Username == term).ToList();
        }
        
        private IQueryable<Customer> Conversion()
        {
            return _ctx.Customers
                //.Include(customer => customer.Subscription)
                .Select(customer => new Customer
                {
                    Id = customer.Id,
                    Firstname = customer.Firstname,
                    Lastname = customer.Lastname,
                    Address = customer.Address,
                    Zipcode = customer.Zipcode,
                    City = customer.City,
                    Email = customer.Email,
                    PhoneNumber = customer.PhoneNumber,
                    Username = customer.Username,
                    Password = customer.Password,
                    Subscription = new Subscription
                    {
                        Id = customer.Subscription.Id,
                        Type = (SubscriptionType) customer.Subscription.Type,
                        DateFrom = customer.Subscription.DateFrom,
                        DateTo = customer.Subscription.DateTo
                    }
                });
        }
    }
}