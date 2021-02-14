namespace Task4.Domain.LogicTaskContexts.Factories
{
    public interface ILogicTaskContextFactory<TContext, TDataItem> where TContext : LogicTaskContext<TDataItem>
    {
        TContext CreateInstance();
    }
}