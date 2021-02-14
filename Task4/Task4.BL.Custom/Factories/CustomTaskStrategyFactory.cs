using Task4.Domain.Strategies;
using Task4.Domain.Strategies.Factories;
using Task4.Domain.Transactions;
using Task4.DAL.Contexts;
using Task4.DAL.Repositories.Factories;

namespace Task4.BL.Custom.Factories
{
    public class CustomTaskStrategyFactory : ILogicTaskStrategyFactory<CsvDTO, CustomLogicTaskContext>
    {
        readonly IDbContextFactory ContextFactory;
        readonly IRepositoryFactory RepoFactory;
        readonly ITransactionScopeFactory ScopeFactory;

        public LogicTaskStrategyEventHandlerContainer<CustomLogicTaskContext, CsvDTO> ActionContainer { get; private set; }

        public CustomTaskStrategyFactory(
            IDbContextFactory contextFactory,
            IRepositoryFactory repoFactory,
            ITransactionScopeFactory scopeFactory
            )
        {
            ContextFactory = contextFactory;
            RepoFactory = repoFactory;
            ScopeFactory = scopeFactory;
            ActionContainer = new LogicTaskStrategyEventHandlerContainer<CustomLogicTaskContext, CsvDTO>();
        }


        public IGenericLogicTaskStrategy<CsvDTO, CustomLogicTaskContext> CreateInstance(CustomLogicTaskContext taskContext)
        {
            var s = new TransactDataTaskStrategy(
                ContextFactory,
                RepoFactory, ScopeFactory);
            s.TaskCompleted += ActionContainer.OnCompleted;
            s.TaskCancelled += ActionContainer.OnCancelled;
            s.TaskFaulted += ActionContainer.OnFaulted;
            s.TaskStarting += ActionContainer.OnStarting;
            s.DataItemHandler += ActionContainer.OnDataItemHandler;
            return s;
        }
    }
}