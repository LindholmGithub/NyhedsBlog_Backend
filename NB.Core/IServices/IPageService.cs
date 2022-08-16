using System.Collections.Generic;
using NyhedsBlog_Backend.Core.Models;

namespace NyhedsBlog_Backend.Core.IServices
{
    public interface IPageService
    {
        public Page GetOneById(int id);
        public List<Page> GetAll();
        public Page CreatePage(Page p);
        public Page DeletePage(Page p);
        public Page UpdatePage(Page p);
    }
}