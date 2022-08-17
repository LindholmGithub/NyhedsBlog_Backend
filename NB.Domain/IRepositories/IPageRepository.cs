using NyhedsBlog_Backend.Core.Models;

namespace NyhedsBlog_Backend.Domain.IRepositories
{
    public interface IPageRepository : ICreateReadRepository<Page>
    {
        public Page GetOneBySlug(string slug);
    }
}