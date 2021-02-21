using Task4.Domain.Absractions;
using Task4.BL.Custom.Operations;
using Task4.Domain.Strategies;
using Task4.Domain.Transactions;
using Task4.DAL.Contexts;
using Task4.DAL.Repositories.Factories;
using Task4.DAL.Models;
using System;
using System.Data.Entity;
using Task4.DAL.Csv;

namespace Task4.BL.Custom
{
    public class TransactDataTaskStrategy : GenericLogicTaskStrategy<CSVDTO, CustomLogicTaskContext>
    {
        protected IDbContextFactory ContextFactory { get; set; }
        protected IRepositoryFactory RepositoryFactory { get; set; }
        protected ITransactionScopeFactory TransactionScopeFactory { get; set; }

        EventHandler<CustomLogicTaskContext> _dataItemHandler;
        public override EventHandler<CustomLogicTaskContext> DataItemHandler
        {
            get => _dataItemHandler;
            set => _dataItemHandler = value;
        }

        public TransactDataTaskStrategy(
            IDbContextFactory contextFactory,
            IRepositoryFactory repositoryFactory,
            ITransactionScopeFactory transactionScopeFactory
            ) : base()
        {
            ContextFactory = contextFactory;
            RepositoryFactory = repositoryFactory;
            TransactionScopeFactory = transactionScopeFactory;
            DataItemHandler += (sender, taskScope) => { OnDataItemHandler(taskScope); };
        }

        protected override void Disposing()
        {
            ContextFactory = null;
            RepositoryFactory = null;
            TransactionScopeFactory = null;
            base.Disposing();
        }

        public void OnDataItemHandler(CustomLogicTaskContext taskContext)
        {
            DbContext context = ContextFactory.CreateInstance();
            try
            {
                context.Database.Connection.Open();
                var managerRepository = RepositoryFactory.CreateInstance<Manager>(context);

                Manager manager = null;
                using (var scope = TransactionScopeFactory.Create())
                {
                    IUnitOfWork unitOfWork =
                        new AddManagerOperation(managerRepository, scope)
                        {
                            Manager = new Manager() { SecondName = taskContext.DataItem.SecondName }
                        };
                    unitOfWork.Execute();
                }
                manager = managerRepository.SingleOrDefault(x => x.SecondName == taskContext.DataItem.SecondName);

                var saleRepository = RepositoryFactory.CreateInstance<Sale>(context);
                var sale = new Sale()
                {
                    Id = manager.Id,
                    Date = taskContext.DataItem.Date,
                    Client = taskContext.DataItem.Client,
                    Product = taskContext.DataItem.Product,
                    Cost = taskContext.DataItem.Cost,
                    Manager = manager,
                    Session = taskContext.Session
                };
                saleRepository.Add(sale);
                saleRepository.Save();

                context.Database.Connection.Close();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Task failed", e);
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}