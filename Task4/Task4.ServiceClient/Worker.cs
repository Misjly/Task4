using System;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Task4.BL.Custom;
using Task4.BL.Custom.Builders;
using Task4.BL.Custom.DataContext;
using Task4.BL.Custom.DataSourceProviders;
using Task4.Domain.Strategies;

namespace Task4.ServiceClient
{
    public class Worker : BackgroundService
    {
        private GenericProcessStrategy<CsvDTO, CustomLogicTaskContext> _handler;
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() => Execute(stoppingToken));
        }

        protected async Task Execute(CancellationToken stoppingToken)
        {
            try
            {
                using (DbContext context = new SaleContext())
                {
                    context.Database.CreateIfNotExists();
                }
                var builder = new BackupFeatureBuilder();
                _handler = builder.Build();

                _handler.DataSourceProvider = new WatcherFileProvider(
                new FileSystemWatcher(ConfigurationManager.AppSettings["sourceFolder"],
                ConfigurationManager.AppSettings["searchPattern"]));


                _handler.Start();
                Log.Information("Handler started");

                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(1000);
                }

                _handler.Stop();
                Log.Information("Handler stoped");
            }

            catch (OperationCanceledException)
            {

            }
            catch (Exception e)
            {
                _handler.Dispose();
                throw e;
            }

            finally
            {
                _handler.Dispose();
            }
        }
    }
}
