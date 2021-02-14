using System.Data.Entity;

namespace Task4.DAL.Repositories.Factories
{
    public interface IRepositoryFactory
    {
        IGenericRepository<TEntity> CreateInstance<TEntity>(DbContext context = null) where TEntity : class;

        void Register<TEntity, RepositoryType>()
            where TEntity : class where RepositoryType : IGenericRepository<TEntity>;
    }
}