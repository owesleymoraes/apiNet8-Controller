using apicatalogo.Models;

namespace ApiCatalogo.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        IEnumerable<Category> GetCategoriesAndProducts();
    }
}