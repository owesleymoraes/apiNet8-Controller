
using apicatalogo.Models;

namespace ApiCatalogo.DTOs.Mappings
{
    public static class CategoryDTOMappingsExtensions
    {
        public static CategoryResponse? ToCategoryDTO(this Category category)
        {
            if (category is null) return null;

            var categoryResponse = CategoryResponse.Create(
              categoryId: category.CategoryId!,
              name: category.Name!,
              imageUrl: category.ImageUrl!
          );

            return categoryResponse;

        }
        public static Category? ToCategory(this CategoryRequest categoryRequest)
        {
            if (categoryRequest is null) return null;

            var category = new Category()
            {
                CategoryId = categoryRequest.CategoryId,
                Name = categoryRequest.Name,
                ImageUrl = categoryRequest.ImageUrl
            };

            return category;

        }
        public static IEnumerable<CategoryResponse>? ToCategoryDTOList(this IEnumerable<Category> categories)

        {
            if (categories is null || !categories.Any())
            {
                return new List<CategoryResponse>();
            }

            return categories.Select(category => new CategoryResponse
            {
                CategoryId = category.CategoryId!,
                Name = category.Name!,
                ImageUrl = category.ImageUrl!

            }).ToList();



        }

    }
}