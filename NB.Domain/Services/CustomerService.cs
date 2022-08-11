using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using NyhedsBlog_Backend.Core.IServices;
using NyhedsBlog_Backend.Core.Models;
using NyhedsBlog_Backend.Core.Models.Subscription;
using NyhedsBlog_Backend.Domain.IRepositories;

namespace NyhedsBlog_Backend.Domain.Services
{
    public class CustomerService : ICustomerService
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
        
        private readonly ICreateReadRepository<Customer> _repo;
        private readonly ICreateReadRepository<Subscription> _subRepo;

        public CustomerService(ICreateReadRepository<Customer> repo, ICreateReadRepository<Subscription> subRepo)
        {
            _repo = repo;
            _subRepo = subRepo;
        }
        
        public Customer GetOneById(int id)
        {
            if (id > 0)
            {
                return _repo.GetById(id);
            }
            throw new InvalidDataException(DomainStrings.IdMustBeOverZero);
        }

        public List<Customer> GetAll()
        {
            return _repo.GetAll() as List<Customer>;
        }

        public Customer CreateCustomer(Customer c)
        {
            if (Validate(c))
            {
                Subscription sub = _subRepo.Create(new Subscription
                {
                    DateFrom = DateTime.Now,
                    DateTo = DateTime.MaxValue,
                    Type = SubscriptionType.None
                });

                c.Subscription = sub;

                return _repo.Create(c);
            }

            return null;
        }

        public Customer DeleteCustomer(Customer c)
        {
            if (c.Id > 0)
            {
                return _repo.Delete(c);
            }

            throw new InvalidDataException(DomainStrings.IdMustBeOverZero);
        }

        public Customer UpdateCustomer(Customer c)
        {
            return Validate(c) ? _repo.Update(c) : null;
        }
        
        public bool Validate(Customer obj)
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