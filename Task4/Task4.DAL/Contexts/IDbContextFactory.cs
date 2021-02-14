using System.Data.Common;
using System.Data.Entity;

namespace Task4.DAL.Contexts
{
    public interface IDbContextFactory
    {
        DbContext CreateInstance();
        DbContext CreateInstance(DbConnection connection, bool contextOwnsConnection);
    }
}