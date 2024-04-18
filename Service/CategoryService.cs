
using ApiCatalogo.DTOs;
using ApiCatalogo.DTOs.Mappings;


namespace ApiCatalogo.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _uof;

        public CategoryService(IUnitOfWork uof)
        {
            _uof = uof;

        }

        public IEnumerable<CategoryResponse> GetCategories()
        {
            var categories = _uof.CategoryRepository.GetAll();

            if (categories is null)
            {
                return null;
            }

            var categoriesList = categories.ToCategoryDTOList();

            return categoriesList;
        }
        public CategoryResponse GetCategory(int id)
        {
            var category = _uof.CategoryRepository.Get(c => c.CategoryId == id);
            _ = category ?? null;

            var categoryResponse = category.ToCategoryDTO();

            return categoryResponse;
        }
        public CategoryResponse Create(CategoryRequest categoryDto)
        {
            _ = categoryDto ?? throw new ArgumentNullException(nameof(categoryDto));

            var category = categoryDto.ToCategory();

            var categoryEntity = _uof.CategoryRepository.Create(category);
            _uof.Commit();

            var categoryResponse = categoryEntity.ToCategoryDTO();

            return categoryResponse;
        }
        public CategoryResponse Update(CategoryRequest categoryDto)
        {
            _ = categoryDto ?? throw new ArgumentNullException(nameof(categoryDto));

            var category = categoryDto.ToCategory();

            var categoryEntity = _uof.CategoryRepository.Update(category);
            _uof.Commit();

            var categoryResponse = categoryEntity.ToCategoryDTO();

            return categoryResponse;
        }
        public CategoryResponse Delete(int id)
        {
            var category = _uof.CategoryRepository.Get(c => c.CategoryId == id);
            _ = category ?? null;

            var categoryEntity = _uof.CategoryRepository.Delete(category!);
            _uof.Commit();

            var categoryResponse = categoryEntity.ToCategoryDTO();

            return categoryResponse;
        }
        public IEnumerable<CategoryResponse> GetCategoriesAndProducts()
        {
            var categories = _uof.CategoryRepository.GetCategoriesAndProducts();

            var categoriesList = categories.ToCategoryDTOList();

            return categoriesList;
        }
    }
}