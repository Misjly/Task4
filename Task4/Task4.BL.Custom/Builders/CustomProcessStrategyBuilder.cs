using Task4.BL.Custom.Factories;
using Task4.Domain.LogicTaskContexts.Factories;
using Task4.Domain.LogicTaskHandlers;
using Task4.Domain.Strategies;
using Task4.Domain.Strategies.Factories;
using Task4.Domain.TaskSchedulers;
using Task4.DAL.Contexts;
using Task4.DAL.Repositories.Factories;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Task4.BL.Custom.Builders
{
    public class CustomProcessStrategyBuilder
    {
        protected ILogicTaskHandler LogicTaskHandler { get; set; }
        protected IDbContextFactory DbContextFactory { get; set; }
        protected IRepositoryFactory RepositoryFactory { get; set; }
        protected ILogicTaskContextFactory<CustomLogicTaskContext, CsvDTO> TaskContextFactory { get; set; }
        protected ILogicTaskStrategyFactory<CsvDTO, CustomLogicTaskContext> TaskStrategyFactory { get; set; }
        protected GenericProcessStrategy<CsvDTO, CustomLogicTaskContext> ProcessStrategy { get; set; }

        protected virtual void Building()
        {
            BuildDbContextFactory();
            BuildTaskHandler();
            BuildRepositoryFactory();
            BuildTaskContextFactory();
            BuildTaskStrategyFactory();
            BuildProcessStrategy();
        }

        public GenericProcessStrategy<CsvDTO, CustomLogicTaskContext> Build()
        {
            Building();
            return ProcessStrategy;
        }

        protected virtual void BuildTaskHandler()
        {
            var cancellationTokenSource = new System.Threading.CancellationTokenSource();
            var scheduler = new ParsingTaskScheduler(new ConcurrentQueue<Task>());

            var taskFactory = new TaskFactory(
                    cancellationTokenSource.Token,
                    TaskCreationOptions.None,
                    TaskContinuationOptions.HideScheduler, scheduler);

            LogicTaskHandler = new LogicTaskHandler(taskFactory, new ConcurrentQueue<Task>());
        }

        protected virtual void BuildDbContextFactory()
        {
            DbContextFactory = new SaleContextFactory();
        }

        protected virtual void BuildRepositoryFactory()
        {
            RepositoryFactory = new TransactionalRepositotyFactory(DbContextFactory);
        }

        protected virtual void BuildTaskContextFactory()
        {
            TaskContextFactory = new CustomLogicTaskContextFactory();
        }

        protected virtual void BuildTaskStrategyFactory()
        {
            TaskStrategyFactory = new CustomTaskStrategyFactory
                (DbContextFactory,
                RepositoryFactory,
                new TransactionScopeFactory()
                );
            TaskStrategyFactory.ActionContainer.OnDataItemHandler +=
                (sender, taskContext) => { taskContext.CancellationToken.ThrowIfCancellationRequested(); };
        }

        protected virtual void BuildProcessStrategy()
        {
            ProcessStrategy = new GenericProcessStrategy<CsvDTO, CustomLogicTaskContext>
                (LogicTaskHandler,
                TaskStrategyFactory,
                TaskContextFactory);
        }

    }
}