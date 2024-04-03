using apicatalogo.Models;

namespace ApiCatalogo.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetCategories();
        Category GetCategory(int id);
        Category Create(Category category);
        Category Update(Category category);
        Category Delete(int id);
        IEnumerable<Category> GetCategoriesAndProducts();

    }
}