using System.ComponentModel.DataAnnotations;

namespace ApiCatalogo.DTOs
{
    public class CategoryResponse
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }

        public static CategoryResponse Create(int categoryId, string name, string imageUrl)
        {
            return new CategoryResponse
            {
                CategoryId = categoryId,
                Name = name,
                ImageUrl = imageUrl
            };
        }

    }
}