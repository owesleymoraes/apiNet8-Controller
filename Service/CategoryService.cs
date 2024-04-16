
using apicatalogo.Models;
using ApiCatalogo.Repositories;

namespace ApiCatalogo.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _repositoryGeneric;
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository, IRepository<Category> repositoryGeneric)
        {
            _repositoryGeneric = repositoryGeneric;
            _repository = repository;
        }


        public IEnumerable<Category> GetCategories()
        {
            return _repositoryGeneric.GetAll();
        }
        public Category GetCategory(int id)
        {
            return _repositoryGeneric.Get(c => c.CategoryId == id);

        }
        public Category Create(Category category)
        {
            _ = category ?? throw new ArgumentNullException(nameof(category));

            return _repositoryGeneric.Create(category);
        }
        public Category Update(Category category)
        {
            _ = category ?? throw new ArgumentNullException(nameof(category));

            return _repositoryGeneric.Update(category);
        }
        public Category Delete(int id)
        {
            var category = _repositoryGeneric.Get(c => c.CategoryId == id);
            _ = category ?? null;

            return _repositoryGeneric.Delete(category!);
        }

        public IEnumerable<Category> GetCategoriesAndProducts()
        {
            return _repository.GetCategoriesAndProducts();
        }
    }
}