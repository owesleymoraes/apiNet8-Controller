
using apicatalogo.Models;
using ApiCatalogo.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategory(int id)
        {

            return _context.Categories.FirstOrDefault(category => category.CategoryId == id);

        }

        public Category Create(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return category;
        }

        public Category Update(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
            _context.SaveChanges();
            return category;
        }

        public Category Delete(int id)
        {

            Category category = _context.Categories.Find(id);

            _ = category ?? throw new ArgumentNullException(nameof(category));

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return category;

        }

        public IEnumerable<Category> GetCategoriesAndProducts()
        {
            return _context.Categories.Include(product => product.Products).ToList();
        }
    }
}