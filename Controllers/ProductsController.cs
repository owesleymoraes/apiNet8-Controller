using apicatalogo.Models;
using ApiCatalogo.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            try
            {
                var products = await _context.Products.AsNoTracking().ToListAsync();

                if (products is null)
                {
                    return NotFound("Produtos não encontrados");
                }
                return products;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema na solicitação");
            }
        }

        [HttpGet("{id:min(1)}", Name = "GetById")]
        public ActionResult<Product> Get(int id)
        {
            var product = _context.Products.FirstOrDefault(products => products.ProductId == id);

            if (product is null)
            {
                return NotFound("Produto não encontrado");
            }
            return product;
        }

        [HttpPost]
        public ActionResult Post(Product product)
        {
            if (product is null)
            {
                return BadRequest();
            }
            _context.Products.Add(product);
            _context.SaveChanges();

            return new CreatedAtRouteResult("GetById", new { id = product.ProductId }, product);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }
            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(product => product.ProductId == id);
            if (product is null)
            {
                return NotFound("Produto não localizado");
            }
            _context.Products.Remove(product);
            _context.SaveChanges();

            return Ok(product);
        }
    }


}