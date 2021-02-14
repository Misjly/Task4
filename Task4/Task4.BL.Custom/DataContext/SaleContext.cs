using Task4.BL.Custom.DataContext.Configurations;
using Task4.DAL.Models;
using System.Data.Common;
using System.Data.Entity;
using System.Configuration;

namespace Task4.BL.Custom.DataContext
{
    public class SaleContext : DbContext
    {
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Sale> Sales { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new SaleConfiguration());
            modelBuilder.Configurations.Add(new ManagerConfiguration());
        }

        protected void Initialize()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SaleContext, Migrations.Configuration>());
        }

        public SaleContext() : base("Task4Database")
        {
            Initialize();
        }

        public SaleContext(DbConnection connection, bool contextOwnsConnection) : base(connection, contextOwnsConnection)
        {
            Initialize();
        }
    }
}