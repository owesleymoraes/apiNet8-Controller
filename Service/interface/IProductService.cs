using apicatalogo.Models;

namespace ApiCatalogo.Service
{
    public interface IProductService
    {
        IEnumerable<Product> GetProducts();
        Product GetProductById(int id);
        Product Create(Product product);
        Product Update(Product product);
        Product Delete(int id);

    }
}