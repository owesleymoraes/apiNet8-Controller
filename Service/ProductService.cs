using apicatalogo.Models;
using ApiCatalogo.Repositories;

namespace ApiCatalogo.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _uof;

        public ProductService(IUnitOfWork uof)
        {
            _uof = uof;

        }

        public Product Create(Product product)
        {
            _ = product ?? throw new ArgumentNullException(nameof(product));

            var productResponse = _uof.ProductRepository.Create(product);
            _uof.Commit();

            return productResponse;
        }
        public Product GetProductById(int id)
        {
            var product = _uof.ProductRepository.Get(c => c.ProductId == id);

            _ = product ?? throw new InvalidOperationException("Produto é null");

            return product;
        }
        public Product? Update(Product product)
        {
            _ = product ?? throw new ArgumentNullException(nameof(product));

            var hasProduct = _uof.ProductRepository.Get(c => c.ProductId == product.ProductId);

            if (hasProduct is not null)
            {
                var productModel = _uof.ProductRepository.Update(product);
                _uof.Commit();

                return productModel;
            }

            return null;

        }
        public Product Delete(int id)
        {
            var product = _uof.ProductRepository.Get(c => c.ProductId == id);
            _ = product ?? null;

            return _uof.ProductRepository.Delete(product!);
        }
        public IEnumerable<Product> GetProducts()
        {
            return _uof.ProductRepository.GetAll();
        }
        public IEnumerable<Product> GetProductByCategory(int id)
        {
            var products = _uof.ProductRepository.GetProductsByCategory(id);
            _ = products ?? null;
            return products!;
        }


    }
}