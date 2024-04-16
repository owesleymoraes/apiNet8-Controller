
using apicatalogo.Models;
using ApiCatalogo.Context;
using Microsoft.EntityFrameworkCore;


namespace ApiCatalogo.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context) { }
        public IEnumerable<Category> GetCategoriesAndProducts()
        {
            return _context.Categories.Include(product => product.Products).ToList();
        }
    }
}