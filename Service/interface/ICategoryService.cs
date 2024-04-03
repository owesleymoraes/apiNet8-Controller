

using apicatalogo.Models;

namespace ApiCatalogo.Service
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetCategories();
        Category GetCategory(int id);
        Category Create(Category category);
        Category Update(Category category);
        Category Delete(int id);
        IEnumerable<Category> GetCategoriesAndProducts();

    }
}