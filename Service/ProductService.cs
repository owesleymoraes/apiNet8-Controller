using apicatalogo.Models;
using ApiCatalogo.Repositories;

namespace ApiCatalogo.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public Product Create(Product product)
        {
            _ = product ?? throw new ArgumentNullException(nameof(product));

            var productResponse = _repository.Create(product);

            return productResponse;
        }

        public Product GetProductById(int id)
        {
            var product = _repository.GetProductById(id);

            _ = product ?? throw new InvalidOperationException("Produto é null");

            return product;
        }

        public Product? Update(Product product)
        {
            _ = product ?? throw new ArgumentNullException(nameof(product));

            var hasProduct = _repository.GetProductById(product.ProductId);

            if (hasProduct is not null)
            {
                var productModel = _repository.Update(product);
                return productModel;
            }

            return null;

        }
        public Product Delete(int id)
        {
            var hasProduct = _repository.GetProductById(id);

            if (hasProduct is not null)
            {
                Product product = _repository.Delete(id);
                return product;
            }
            return null;
        }
        public IEnumerable<Product> GetProducts()
        {
            return _repository.GetProducts();
        }
    }
}