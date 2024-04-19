using apicatalogo.Models;
using ApiCatalogo.DTOs;
using AutoMapper;

namespace ApiCatalogo.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;

        }

        public ProductDTO Create(ProductDTO product)
        {
            _ = product ?? throw new ArgumentNullException(nameof(product));

            var productentity = _mapper.Map<Product>(product);

            var productResponse = _uof.ProductRepository.Create(productentity);
            _uof.Commit();

            var response = _mapper.Map<ProductDTO>(productResponse);

            return response;
        }
        public ProductDTO GetProductById(int id)
        {
            var product = _uof.ProductRepository.Get(c => c.ProductId == id);

            _ = product ?? throw new InvalidOperationException("Produto é null");

            // _mapper.Map<Destino>(Origem);
            var response = _mapper.Map<ProductDTO>(product);

            return response;
        }
        public ProductDTO? Update(ProductDTO product)
        {
            _ = product ?? throw new ArgumentNullException(nameof(product));

            var hasProduct = _uof.ProductRepository.Get(c => c.ProductId == product.ProductId);

            if (hasProduct is not null)
            {
                var productEntity = _mapper.Map<Product>(product);
                var productModel = _uof.ProductRepository.Update(productEntity);
                _uof.Commit();

                var response = _mapper.Map<ProductDTO>(productModel);

                return response;
            }

            return null;

        }
        public ProductDTO Delete(int id)
        {
            var product = _uof.ProductRepository.Get(c => c.ProductId == id);
            if (product is null) return null;

            var productEntity = _uof.ProductRepository.Delete(product!);
            var response = _mapper.Map<ProductDTO>(productEntity);

            return response;
        }
        public IEnumerable<ProductDTO> GetProducts()
        {
            IEnumerable<Product> productEntity = _uof.ProductRepository.GetAll();
            var response = _mapper.Map<IEnumerable<ProductDTO>>(productEntity);
            return response;
        }
        public IEnumerable<ProductDTO> GetProductByCategory(int id)
        {
            var products = _uof.ProductRepository.GetProductsByCategory(id);
            var response = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return response;
        }


    }
}