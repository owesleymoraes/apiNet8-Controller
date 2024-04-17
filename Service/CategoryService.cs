
using apicatalogo.Models;
using ApiCatalogo.Repositories;

namespace ApiCatalogo.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _uof;

        public CategoryService(IUnitOfWork uof)
        {
            _uof = uof;

        }


        public IEnumerable<Category> GetCategories()
        {
            return _uof.CategoryRepository.GetAll();
        }
        public Category GetCategory(int id)
        {
            return _uof.CategoryRepository.Get(c => c.CategoryId == id);

        }
        public Category Create(Category category)
        {
            _ = category ?? throw new ArgumentNullException(nameof(category));

            var categoryResponse = _uof.CategoryRepository.Create(category);
            _uof.Commit();

            return categoryResponse;
        }
        public Category Update(Category category)
        {
            _ = category ?? throw new ArgumentNullException(nameof(category));

            var categoryResponse = _uof.CategoryRepository.Update(category);
            _uof.Commit();

            return categoryResponse;
        }
        public Category Delete(int id)
        {
            var category = _uof.CategoryRepository.Get(c => c.CategoryId == id);
            _ = category ?? null;

            var categoryResponse = _uof.CategoryRepository.Delete(category!);
            _uof.Commit();

            return categoryResponse;
        }

        public IEnumerable<Category> GetCategoriesAndProducts()
        {
            return _uof.CategoryRepository.GetCategoriesAndProducts();
        }
    }
}