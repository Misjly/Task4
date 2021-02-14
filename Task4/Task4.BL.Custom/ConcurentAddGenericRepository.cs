using Task4.DAL.CommandBuilders;
using Task4.DAL.Repositories;
using System.Data.Entity;

namespace Task4.BL.Custom
{
    public class ConcurentAddGenericRepositoty<TEntity> : GenericRepository<TEntity>, IGenericRepository<TEntity> where TEntity : class
    {
        readonly IGenericSqlCommandBuilder<TEntity> _commandBuilder = null;

        public ConcurentAddGenericRepositoty(DbContext context)
            : base(context)
        {
            _commandBuilder = new ConcurrentAddWhenNotExistCommandBuilder<TEntity>(context);
        }

        public new void Add(TEntity entity)
        {
            Context.Database.ExecuteSqlCommand(
                                            _commandBuilder.CommandText,
                                            _commandBuilder.GetParameters(entity)
                                        );
            Attach(entity);
            Context.Entry(entity).Reload();
        }
    }
}