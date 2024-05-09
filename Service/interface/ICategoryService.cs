
using ApiCatalogo.DTOs;

namespace ApiCatalogo.Service
{
    public interface ICategoryService
    {
        IEnumerable<CategoryResponse> GetCategories();
        CategoryResponse GetCategory(int id);
        CategoryResponse Create(CategoryRequest category);
        CategoryResponse Update(CategoryRequest category);
        CategoryResponse Delete(int id);
        IEnumerable<CategoryResponse> GetCategoriesAndProducts();

    }
}