using apicatalogo.Models;
using ApiCatalogo.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Repositories
{
    public class ProductRepository : IProductRepository
    {

        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public Product Create(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }

        public Product Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();
            return product;
        }

        public Product GetProductById(int id)
        {
            var product = _context.Products.FirstOrDefault(product => product.ProductId == id);
            return product;
        }

        public Product Delete(int id)
        {
            Product product = _context.Products.Find(id);

            _ = product ?? throw new ArgumentNullException(nameof(product));

            _context.Products.Remove(product);
            _context.SaveChanges();

            return product;
        }

        public IQueryable<Product> GetProducts()
        {
            return _context.Products;
        }
    }
}