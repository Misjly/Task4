using Task4.DAL.Contexts;
using Task4.DAL.Repositories;
using Task4.DAL.Repositories.Factories;
using Task4.DAL.Models;
using System;
using System.Collections.Generic;

namespace Task4.BL.Custom
{
    public class ProcessStartDataIntegrityHandler
    {
        readonly IDbContextFactory ContextFactory;
        readonly IRepositoryFactory RepositoryFactory;

        public ProcessStartDataIntegrityHandler(
            IDbContextFactory contextFactory,
            IRepositoryFactory repositoryFactory)
        {
            ContextFactory = contextFactory;
            RepositoryFactory = repositoryFactory;
        }
        public virtual void OnProcessStartRecoveryHandler(object sender, EventArgs args)
        {
            using (var context = ContextFactory.CreateInstance())
            {
                IGenericRepository<Sale> saleRepository = null;
                IEnumerable<Sale> items = null;
                try
                {
                    saleRepository = RepositoryFactory.CreateInstance<Sale>(context);
                    items = saleRepository.Get(x => x.Session != null);
                    foreach (var item in items)
                    {
                        saleRepository.Remove(item);
                    }
                    saleRepository.Save();
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException("Recovery failed", e);
                }
            }
        }

    }
}