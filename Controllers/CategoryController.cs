using apicatalogo.Models;
using ApiCatalogo.Filters;
using ApiCatalogo.Service;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService service, ILogger<CategoryController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Category>> Get()
        {
            var categories = _service.GetCategories();

            return Ok(categories);
        }

        [HttpGet("{id}", Name = "GetCategoryById")]
        public ActionResult<Category> Get(int id)
        {
            var category = _service.GetCategory(id);

            if (category is null)
                return NotFound("Categoria não encontrado");

            return Ok(category);
        }

        [HttpPost]
        public ActionResult Post(Category category)
        {
            if (category is null)
                return BadRequest();

            Category categoryModel = _service.Create(category);

            return new CreatedAtRouteResult("GetCategoryById", new { id = categoryModel.CategoryId }, categoryModel);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            var updatedCategory = _service.Update(category);

            if (updatedCategory == null)
            {
                return BadRequest("Id informado não corresponde a nenhum produto.");
            }

            return Ok(updatedCategory);
        }

        [HttpDelete("{id}")]
        public ActionResult<Category> Delete(int id)
        {

            _service.Delete(id);

            return Ok();
        }

        [HttpGet("products")]
        public ActionResult<IEnumerable<Category>> GetCategoriesAndProducts()
        {
            var categoriesAndProducts = _service.GetCategoriesAndProducts();
            return Ok(categoriesAndProducts);
        }
    }
}