
using ApiCatalogo.Repositories;

namespace ApiCatalogo.Service
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }

        void Commit();

    }
}