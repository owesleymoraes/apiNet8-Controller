using apicatalogo.Models;
using ApiCatalogo.Repositories;

namespace ApiCatalogo.Service
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _repositoryGeneric;
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository, IRepository<Product> repositoryGeneric)
        {
            _repository = repository;
            _repositoryGeneric = repositoryGeneric;
        }

        public Product Create(Product product)
        {
            _ = product ?? throw new ArgumentNullException(nameof(product));

            var productResponse = _repository.Create(product);

            return productResponse;
        }

        public Product GetProductById(int id)
        {
            var product = _repositoryGeneric.Get(c => c.ProductId == id);

            _ = product ?? throw new InvalidOperationException("Produto é null");

            return product;
        }

        public Product? Update(Product product)
        {
            _ = product ?? throw new ArgumentNullException(nameof(product));

            var hasProduct = _repositoryGeneric.Get(c => c.ProductId == product.ProductId);

            if (hasProduct is not null)
            {
                var productModel = _repositoryGeneric.Update(product);
                return productModel;
            }

            return null;

        }
        public Product Delete(int id)
        {
            var product = _repositoryGeneric.Get(c => c.ProductId == id);
            _ = product ?? null;

            return _repositoryGeneric.Delete(product!);
        }
        public IEnumerable<Product> GetProducts()
        {
            return _repository.GetAll();
        }

        public IEnumerable<Product> GetProductByCategory(int id)
        {
            var products = _repository.GetProductsByCategory(id);
            _ = products ?? null;
            return products!;
        }


    }
}