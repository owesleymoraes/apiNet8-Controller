using apicatalogo.Models;
using ApiCatalogo.Context;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public ActionResult<IEnumerable<Product>> Get()
        {
            var products = _context.Products.ToList();

            if (products is null)
            {
                return NotFound("Produtos não encontrados");
            }
            return products;
        }

        [HttpGet("{id}", Name = "GetById")]
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