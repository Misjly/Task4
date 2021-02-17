using System.ServiceProcess;

namespace Task4.ServiceClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new SaleService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
