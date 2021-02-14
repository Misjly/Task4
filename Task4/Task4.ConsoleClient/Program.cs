using Task4.BL.Custom.DataContext;
using Task4.BL.Custom.DataSourceProviders;
using System.Data.Entity;
using Task4.BL.Custom;
using Task4.Domain.Strategies;
using System;

namespace Task4.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (DbContext context = new SaleContext())
            {
                context.Database.CreateIfNotExists();
            }

            var builder = new ConloseMessageBuilder();

            GenericProcessStrategy<CsvDTO, CustomLogicTaskContext> handler = builder.Build();

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