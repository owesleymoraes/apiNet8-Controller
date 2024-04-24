using ApiCatalogo.DTOs;
using apicatalogo.Models;
using ApiCatalogo.Service;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductDTO>> Get()
        {

            var products = _service.GetProducts().ToList();
            return Ok(products);

        }

        [HttpGet("{id:min(1)}", Name = "GetById")]
        public ActionResult<ProductDTO> Get(int id)
        {
            var product = _service.GetProductById(id);

            if (product is null)
            {
                return NotFound("Produto não encontrado");
            }
            return product;
        }

        [HttpPost]
        public ActionResult<ProductDTO> Post(ProductDTO productDTO)
        {
            if (productDTO is null)
            {
                return BadRequest();
            }

            ProductDTO productResponse = _service.Create(productDTO);

            return new CreatedAtRouteResult("GetById", new { id = productResponse.ProductId }, productResponse);
        }

        [HttpPut("{id}")]
        public ActionResult<ProductDTO> Put(int id, ProductDTO product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            var updatedProduct = _service.Update(product);

            if (updatedProduct == null)
            {
                return BadRequest("Id informado não corresponde a nenhum produto.");
            }


            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        public ActionResult<ProductDTO> Delete(int id)
        {
            var deleteProduct = _service.Delete(id);

            if (deleteProduct == null)
            {
                return BadRequest("Id informado não corresponde a nenhum produto.");
            }

            return Ok(deleteProduct);

        }

        [HttpGet("products/{id}")]
        public ActionResult<IEnumerable<ProductDTO>> GetProductAndCategoryById(int id)
        {
            var product = _service.GetProductByCategory(id);

            if (product == null)
            {
                return BadRequest("Id informado não corresponde a nenhum produto.");
            }

            return Ok(product);

        }
        [HttpGet("pagination")]
        public ActionResult<IEnumerable<ProductDTO>> GetProductPagination([FromQuery] ProductParameters productParameters)
        {
            var product = _service.GetProductsPagination(productParameters);

            if (product == null)
            {
                return BadRequest("Id informado não corresponde a nenhum produto.");
            }

            return Ok(product);

        }

    }

}