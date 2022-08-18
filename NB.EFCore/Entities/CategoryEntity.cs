using System.Collections.Generic;

namespace NB.EFCore.Entities
{
    public class CategoryEntity
    {
        public int Id { get; set; }
        public string PrettyDescriptor { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        
        public List<PostEntity> Posts { get; set; }
    }
}