using apicatalogo.Models;
using ApiCatalogo.DTOs;
using ApiCatalogo.Filters;
using ApiCatalogo.Service;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<CategoryResponse>> Get()
        {
            var categories = _service.GetCategories();

            return Ok(categories);
        }

        [HttpGet("{id}", Name = "GetCategoryById")]
        public ActionResult<CategoryResponse> Get(int id)
        {
            var category = _service.GetCategory(id);

            if (category is null)
                return NotFound("Categoria não encontrado");

            return Ok(category);
        }

        [HttpPost]
        public ActionResult<CategoryResponse> Post(CategoryRequest category)
        {
            if (category is null)
                return BadRequest();

            CategoryResponse categoryModel = _service.Create(category);

            return new CreatedAtRouteResult("GetCategoryById", new { id = categoryModel.CategoryId }, categoryModel);
        }

        [HttpPut("{id}")]
        public ActionResult<CategoryResponse> Put(int id, CategoryRequest category)
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
        [Authorize(Policy = "AdminOnly")]
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