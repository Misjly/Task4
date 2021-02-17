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

        public SaleContext() : base()
        {
            //Configuration conf = ConfigurationManager.OpenExeConfiguration(@"C:\Users\nikita\source\repos\workspace\Task4\Task4\Task4.ServiceClient\bin\Debug\Task4.ServiceClient.exe");
            //string databaseName = conf.AppSettings.Settings["Task4Database"].Value;
            //new DbContext(databaseName);
            Initialize();
        }

        public SaleContext(DbConnection connection, bool contextOwnsConnection) : base(connection, contextOwnsConnection)
        {
            Initialize();
        }
    }
}