using System.Collections.Generic;

namespace NyhedsBlog_Backend.Domain.IRepositories
{
    public interface IReadRepository<T>
    {
        /// <summary>
        /// Returns one object of specified type from the specified id
        /// </summary>
        /// <param name="id">ID of object to return</param>
        /// <returns>Returns one object of specified type from the specified id</returns>
        public T GetById(int id);

        /// <summary>
        /// Returns list of all objects of specified type
        /// </summary>
        /// <returns>List of objects</returns>
        public IEnumerable<T> GetAll();

        /// <summary>
        /// Returns list of all objects of specified type which meet the specified term
        /// </summary>
        /// <param name="term">Term to search for</param>
        /// <returns>List of objects</returns>
        public IEnumerable<T> Search(string term);
    }
}