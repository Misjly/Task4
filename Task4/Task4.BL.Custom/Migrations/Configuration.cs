using System.Data.Entity.Migrations;
using Task4.BL.Custom.DataContext;

namespace Task4.BL.Custom.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<SaleContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "SaleContext";
        }

        protected override void Seed(Task4.BL.Custom.DataContext.SaleContext context)
        {
            base.Seed(context);
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
