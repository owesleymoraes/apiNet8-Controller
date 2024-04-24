using apicatalogo.Models;
using ApiCatalogo.Context;

namespace ApiCatalogo.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {

        public ProductRepository(AppDbContext context) : base(context) { }

        public IEnumerable<Product> GetProducts(ProductParameters productParameters)
        {
            return GetAll()
            .OrderBy(p => p.Name)
            .Skip((productParameters.PageNumber - 1) * productParameters.PageSize)
            .Take(productParameters.PageSize).ToList();

        }
        public IEnumerable<Product> GetProductsByCategory(int id)
        {
            return GetAll().Where(c => c.CategoryId == id);

        }
    }
}