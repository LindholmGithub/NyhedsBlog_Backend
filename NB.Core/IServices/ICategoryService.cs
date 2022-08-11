using System.Collections.Generic;
using NyhedsBlog_Backend.Core.Models.Post;

namespace NyhedsBlog_Backend.Core.IServices
{
    public interface ICategoryService
    {
        public Category GetOneById(int id);
        public List<Category> GetAll();
        public Category CreateCategory(Category c);
        public Category DeleteCategory(Category c);
        public Category UpdateCategory(Category c);
    }
}