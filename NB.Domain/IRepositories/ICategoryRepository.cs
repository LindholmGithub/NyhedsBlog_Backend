using NyhedsBlog_Backend.Core.Models.Post;

namespace NyhedsBlog_Backend.Domain.IRepositories
{
    public interface ICategoryRepository : ICreateReadRepository<Category>
    {
        public Category GetOneBySlug(string slug);
    }
}