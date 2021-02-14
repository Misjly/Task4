using Task4.Domain.LogicTaskContexts;

namespace Task4.Domain.Strategies.Factories
{
    public interface ILogicTaskStrategyFactory<TDataItem, TLogicTaskContext>
        where TLogicTaskContext : LogicTaskContext<TDataItem>
    {
        LogicTaskStrategyEventHandlerContainer<TLogicTaskContext, TDataItem> ActionContainer { get; }
        IGenericLogicTaskStrategy<TDataItem, TLogicTaskContext> CreateInstance(TLogicTaskContext taskContext);
    }
}