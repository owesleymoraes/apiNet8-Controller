using ApiCatalogo.DTOs;

namespace ApiCatalogo.Service
{
    public interface IProductService
    {
        IEnumerable<ProductDTO> GetProducts();
        ProductDTO GetProductById(int id);
        ProductDTO Create(ProductDTO productDTO);
        ProductDTO Update(ProductDTO productDTO);
        ProductDTO Delete(int id);
        IEnumerable<ProductDTO> GetProductByCategory(int id);
        IEnumerable<ProductDTO> GetProductsPagination(ProductParameters productParameters);

    }
}