
using apicatalogo.Models;
using ApiCatalogo.Repositories;

namespace ApiCatalogo.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Category> GetCategories()
        {
            return _repository.GetCategories();
        }
        public Category GetCategory(int id)
        {
            return _repository.GetCategory(id);

        }
        public Category Create(Category category)
        {
            _ = category ?? throw new ArgumentNullException(nameof(category));

            return _repository.Create(category);
        }
        public Category Update(Category category)
        {
            _ = category ?? throw new ArgumentNullException(nameof(category));

            return _repository.Update(category);
        }
        public Category Delete(int id)
        {
            Category category = _repository.Delete(id);
            return category;
        }

        public IEnumerable<Category> GetCategoriesAndProducts()
        {
            return _repository.GetCategoriesAndProducts();
        }
    }
}