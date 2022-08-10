using System.Collections.Generic;
using NyhedsBlog_Backend.Core.Models;

namespace NyhedsBlog_Backend.Core.IServices
{
    public interface IPostService
    {
        public Post GetOneById(int id);
        public List<Post> GetAll();
        public Post CreatePost(Post p);
        public Post DeletePost(Post p);
        public Post UpdatePost(Post p);
    }
}