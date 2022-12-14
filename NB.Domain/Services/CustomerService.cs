using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using NyhedsBlog_Backend.Core.IServices;
using NyhedsBlog_Backend.Core.Models.Customer;
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
        
        private readonly ICustomerRepository _repo;

        public CustomerService(ICustomerRepository repo)
        {
            _repo = repo;
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
            return ValidateUpdate(c) ? _repo.Update(c) : null;
        }

        public Payment AddPayment(Payment p)
        {
            return _repo.AddPayment(p);
        }

        public Payment UpdatePayment(Payment p)
        {
            return _repo.UpdatePayment(p);
        }

        public Payment GetPayment(int id)
        {
            return _repo.GetPayment(id);
        }

        public Customer Validate(string username, string password)
        {
            List<Customer> allUsers = GetAll();

            var check = allUsers.Where(u => u.Username == username && u.Password == password).ToList();

            if (check.Any())
                return check[0];

            throw new InvalidDataException(InvalidLogin);
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
        public bool ValidateUpdate(Customer obj)
        {
            if (obj.Username.Length < UsernameMinimumLength)
                throw new InvalidDataException(InvalidUsername);
            
            if (obj.Password is {Length: < PasswordMinimumLength})
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