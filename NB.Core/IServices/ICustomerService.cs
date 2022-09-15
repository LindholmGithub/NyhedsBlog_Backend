using System.Collections.Generic;
using NyhedsBlog_Backend.Core.Models.Customer;

namespace NyhedsBlog_Backend.Core.IServices
{
    public interface ICustomerService
    {
        public Customer GetOneById(int id);
        public List<Customer> GetAll();
        public Customer CreateCustomer(Customer c);
        public Customer DeleteCustomer(Customer c);
        public Customer UpdateCustomer(Customer c);
        public Customer AddPayment(Customer c, Payment p);
        public Customer Validate(string username, string password);
    }
}