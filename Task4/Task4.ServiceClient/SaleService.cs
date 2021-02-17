using System;
using System.Data.Entity;
using System.ServiceProcess;
using Task4.Domain.Strategies;
using Task4.BL.Custom;
using Task4.BL.Custom.DataContext;
using Task4.BL.Custom.Builders;
using Task4.BL.Custom.DataSourceProviders;
using System.IO;
using System.Configuration;

namespace Task4.ServiceClient
{
    public partial class SaleService : ServiceBase
    {
        private GenericProcessStrategy<CsvDTO, CustomLogicTaskContext> _handler;

        public SaleService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                using (DbContext context = new SaleContext())
                {
                    context.Database.CreateIfNotExists();
                }
                var builder = new BackupFeatureBuilder();
                _handler = builder.Build();

                //Configuration conf = ConfigurationManager.OpenExeConfiguration(@"C:\Users\nikita\source\repos\workspace\Task4\Task4\Task4.ServiceClient\bin\Debug\Task4.ServiceClient.exe");

                _handler.DataSourceProvider = new WatcherFileProvider(
                    new FileSystemWatcher(/*conf.AppSettings.Settings["sourceFolder"].Value*/@"d:\temp\", @"*.csv"));
                /*conf.AppSettings.Settings["searchPattern"].Value*/
                _handler.Start();
            }
            catch (Exception e)
            {
                _handler.Dispose();
                throw e;
            }
        }
        

        protected override void OnStop()
        {
            _handler.Dispose();
        }
    }
}
