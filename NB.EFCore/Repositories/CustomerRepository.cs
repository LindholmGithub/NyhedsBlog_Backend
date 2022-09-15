﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NB.EFCore.Entities;
using NyhedsBlog_Backend.Core.Models.Customer;
using NyhedsBlog_Backend.Core.Models.Post;
using NyhedsBlog_Backend.Core.Models.User;
using NyhedsBlog_Backend.Domain.IRepositories;

namespace NB.EFCore.Repositories
{
    public class CustomerRepository : ICustomerRepository
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
                Payments = new List<PaymentEntity>()
            }).Entity;
            _ctx.SaveChanges();

            return GetById(newEntity.Id);
        }

        public Customer Update(Customer obj)
        {
            var oldObject = GetById(obj.Id);

            obj.Payments ??= oldObject.Payments;
            obj.Password ??= oldObject.Password;

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
                Payments = obj.Payments.Select(ConvertPaymentToEntity).ToList()
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

        public Customer AddPayment(Customer c, Payment p)
        {
            var currentCustomer = GetById(c.Id);
            currentCustomer.Payments.Add(p);

            return Update(currentCustomer);
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
                    Payments = customer.Payments.Select(p => ConvertPaymentEntityToModel(p)).ToList()
                });
        }

        private static PaymentEntity ConvertPaymentToEntity(Payment p)
        {
            return new PaymentEntity
            {
                Id = p.Id,
                Timestamp = p.Timestamp,
                Amount = p.Amount,
                PostId = p.Post.Id,
                Status = (int) p.Status,
            };
        }

        private static Payment ConvertPaymentEntityToModel(PaymentEntity p)
        {
            return new Payment
            {
                Id = p.Id,
                Timestamp = p.Timestamp,
                Amount = p.Amount,
                Status = (PaymentStatus) p.Status,
                Post = new Post
                {
                    Id = p.PostId
                }
            };
        }
    }
}