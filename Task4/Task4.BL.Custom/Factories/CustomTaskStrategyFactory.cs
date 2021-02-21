using Task4.Domain.Strategies;
using Task4.Domain.Strategies.Factories;
using Task4.Domain.Transactions;
using Task4.DAL.Contexts;
using Task4.DAL.Repositories.Factories;
using Task4.DAL.Csv;

namespace Task4.BL.Custom.Factories
{
    public class CustomTaskStrategyFactory : ILogicTaskStrategyFactory<CSVDTO, CustomLogicTaskContext>
    {
        readonly IDbContextFactory ContextFactory;
        readonly IRepositoryFactory RepoFactory;
        readonly ITransactionScopeFactory ScopeFactory;

        public LogicTaskStrategyEventHandlerContainer<CustomLogicTaskContext, CSVDTO> ActionContainer { get; private set; }

        public CustomTaskStrategyFactory(
            IDbContextFactory contextFactory,
            IRepositoryFactory repoFactory,
            ITransactionScopeFactory scopeFactory
            )
        {
            ContextFactory = contextFactory;
            RepoFactory = repoFactory;
            ScopeFactory = scopeFactory;
            ActionContainer = new LogicTaskStrategyEventHandlerContainer<CustomLogicTaskContext, CSVDTO>();
        }


        public IGenericLogicTaskStrategy<CSVDTO, CustomLogicTaskContext> CreateInstance(CustomLogicTaskContext taskContext)
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