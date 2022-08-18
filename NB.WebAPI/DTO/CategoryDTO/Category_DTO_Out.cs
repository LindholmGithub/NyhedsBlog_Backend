using System.Collections.Generic;
using NB.WebAPI.DTO.PostDTO;

namespace NB.WebAPI.DTO.CategoryDTO
{
    public class Category_DTO_Out
    {
        public int Id { get; set; }
        
        public string PrettyDescriptor { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public List<Post_DTO_Out> Posts { get; set; }
    }
}