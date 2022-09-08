using NyhedsBlog_Backend.Core.Models.Post;

namespace NyhedsBlog_Backend.Domain.IRepositories
{
    public interface IPostRepository: ICreateReadRepository<Post>
    {
        public Post GetOneBySlug(string slug);
    }
}