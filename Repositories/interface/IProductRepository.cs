
using apicatalogo.Models;

namespace ApiCatalogo.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetProducts(ProductParameters productParameters);
        IEnumerable<Product> GetProductsByCategory(int id);

    }
}