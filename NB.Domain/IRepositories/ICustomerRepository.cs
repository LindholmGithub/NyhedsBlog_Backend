using NyhedsBlog_Backend.Core.Models.Customer;

namespace NyhedsBlog_Backend.Domain.IRepositories
{
    public interface ICustomerRepository : ICreateReadRepository<Customer>
    {
        public Payment AddPayment(Payment p);

        public Payment UpdatePayment(Payment p);
        public Payment GetPayment(int id);
    }
}