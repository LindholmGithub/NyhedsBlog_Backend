using NyhedsBlog_Backend.Core.Models.Customer;

namespace NyhedsBlog_Backend.Domain.IRepositories
{
    public interface ICustomerRepository : ICreateReadRepository<Customer>
    {
        public Customer AddPayment(Customer c, Payment p);
    }
}