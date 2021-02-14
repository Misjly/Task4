using System.Data.SqlClient;

namespace Task4.DAL.CommandBuilders
{
    public interface IGenericSqlCommandBuilder<TEntity> where TEntity : class
    {
        string CommandText { get; }
        SqlParameter[] GetParameters(TEntity entity);
    }
}