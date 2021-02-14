using Task4.DAL.Contexts;
using Task4.DAL.Repositories;
using Task4.DAL.Repositories.Factories;
using Task4.DAL.Models;
using System;
using System.Collections.Generic;

namespace Task4.BL.Custom
{
    public class LogicTaskDataCommiter
    {
        readonly IDbContextFactory ContextFactory;
        readonly IRepositoryFactory RepositoryFactory;
        protected Exception PostPhaseException { get; set; } = null;


        public LogicTaskDataCommiter(
            IDbContextFactory contextFactory,
            IRepositoryFactory repositoryFactory)
        {
            ContextFactory = contextFactory;
            RepositoryFactory = repositoryFactory;
        }
        public void TryPostCsvFileData(CustomLogicTaskContext taskContext)
        {
            PostPhaseException = null;
            using (var context = ContextFactory.CreateInstance())
            {
                IGenericRepository<Sale> saleRepository = null;
                IEnumerable<Sale> items = null;
                try
                {
                    saleRepository = RepositoryFactory.CreateInstance<Sale>(context);
                    items = saleRepository.Get(x => x.Session == taskContext.Session);
                    foreach (var item in items)
                    {
                        item.Session = null;
                    }
                    saleRepository.Save();
                }
                catch (Exception postException)
                {
                    PostPhaseException = postException;
                    RollBack(taskContext);
                }
            }
        }

        public void RollBack(CustomLogicTaskContext taskContext)
        {
            using (var context = ContextFactory.CreateInstance())
            {
                IGenericRepository<Sale> saleRepository = null;
                IEnumerable<Sale> items = null;
                try
                {
                    saleRepository = RepositoryFactory.CreateInstance<Sale>(context);
                    items = saleRepository.Get(x => x.Session == taskContext.Session);
                    foreach (var item in items)
                    {
                        saleRepository.Remove(item);
                    }
                    saleRepository.Save();
                    if (PostPhaseException != null)
                    {
                        throw new InvalidOperationException("Commit failed", PostPhaseException);
                    }
                }
                catch (Exception rollbackException)
                {
                    throw new InvalidOperationException("Rollback failed", rollbackException);
                }
            }
        }
    }
}