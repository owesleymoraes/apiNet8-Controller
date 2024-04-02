using apicatalogo.Models;
using ApiCatalogo.Context;
using ApiCatalogo.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Category>> Get()
        {
            var categories = _context.Categories.AsNoTracking().ToList();

            if (categories is null)
            {
                return NotFound("Categorias não encontradas");
            }
            return categories;
        }

        [HttpGet("{id}", Name = "GetCategoryById")]
        public ActionResult<Category> Get(int id)
        {
            try
            {
                var category = _context.Categories.FirstOrDefault(category => category.CategoryId == id);

                if (category == null)
                {
                    return NotFound("Categoria não encontrado");
                }
                return Ok(category);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema na solicitação");
            }

        }

        [HttpPost]
        public ActionResult Post(Category category)
        {
            if (category is null)
            {
                return BadRequest();
            }
            _context.Categories.Add(category);
            _context.SaveChanges();

            return new CreatedAtRouteResult("GetCategoryById", new { id = category.CategoryId }, category);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }
            _context.Entry(category).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(category);
        }

        [HttpDelete("{id}")]
        public ActionResult<Category> Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(category => category.CategoryId == id);
            if (category is null)
            {
                return NotFound("Categoria não localizada");
            }
            _context.Categories?.Remove(category);
            _context.SaveChanges();

            return Ok(category);
        }

        [HttpGet("products")]
        public ActionResult<IEnumerable<Category>> GetCategoriesAndProducts()
        {
            return _context.Categories.Include(product => product.Products).ToList();
        }
    }
}