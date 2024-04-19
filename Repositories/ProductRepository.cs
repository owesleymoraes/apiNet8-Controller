using apicatalogo.Models;
using ApiCatalogo.Context;

namespace ApiCatalogo.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {

        public ProductRepository(AppDbContext context) : base(context) { }

        public IEnumerable<Product> GetProductsByCategory(int id)
        {
            return GetAll().Where(c => c.CategoryId == id);

        }
    }
}