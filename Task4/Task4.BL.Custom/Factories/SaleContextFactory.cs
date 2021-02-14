using Task4.BL.Custom.DataContext;
using Task4.DAL.Contexts;
using System.Data.Common;
using System.Data.Entity;

namespace Task4.BL.Custom.Factories
{
    public class SaleContextFactory : IDbContextFactory
    {
        public DbContext CreateInstance()
        {
            return new SaleContext();
        }

        public DbContext CreateInstance(DbConnection connection, bool contextOwnsConnection)
        {
            return new SaleContext(connection, contextOwnsConnection);
        }
    }
}