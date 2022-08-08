namespace NyhedsBlog_Backend.Domain.IRepositories
{
    public interface ICreateReadRepository<T> : ICreateRepository<T>, IReadRepository<T>
    {
        
    }
}