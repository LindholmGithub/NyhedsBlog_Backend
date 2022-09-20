namespace NB.WebAPI.DTO.CategoryDTO
{
    public class Category_DTO_In
    {
        public string Title { get; set; }
        
        public bool Featured { get; set; }
        public string Description { get; set; }
        public string PrettyDescriptor { get; set; }
    }
}