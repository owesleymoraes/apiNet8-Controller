
using apicatalogo.Models;

namespace ApiCatalogo.Repositories
{
    public interface IProductRepository
    {
        IQueryable<Product> GetProducts();
        Product GetProductById(int id);
        Product Create(Product product);
        Product Update(Product product);
        Product Delete(int id);

    }
}