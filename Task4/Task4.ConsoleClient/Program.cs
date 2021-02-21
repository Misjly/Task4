using Task4.BL.Custom.DataContext;
using System.Data.Entity;
using Task4.BL.Custom;
using Task4.Domain.Strategies;
using System;
using System.Diagnostics;
using Task4.DAL.Csv;
using Task4.DAL.DataSourceProviders;

namespace Task4.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Debugger.Launch();
            using (DbContext context = new SaleContext())
            {
                context.Database.CreateIfNotExists();
            }

            var builder = new ConloseMessageBuilder();

            GenericProcessStrategy<CSVDTO, CustomLogicTaskContext> handler = builder.Build();

            handler.DataSourceProvider = new WatcherFileProvider();

            try
            {
                handler.Start();
                Console.WriteLine("Add files");
                Console.ReadKey();
                handler.Stop();
            }
            finally
            {
                handler.Dispose();
            }
        }
    }
}