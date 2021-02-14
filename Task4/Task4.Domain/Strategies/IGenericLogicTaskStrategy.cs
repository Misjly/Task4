using System;
using Task4.Domain.LogicTaskContexts;

namespace Task4.Domain.Strategies
{
    public interface IGenericLogicTaskStrategy<TDataItem, TLogicTaskContext> : IDisposable
        where TLogicTaskContext : LogicTaskContext<TDataItem>
    {
        event EventHandler<TLogicTaskContext> TaskStarting;
        event EventHandler<TLogicTaskContext> TaskCancelled;
        event EventHandler<TLogicTaskContext> TaskCompleted;
        event EventHandler<TLogicTaskContext> TaskFaulted;

        EventHandler<TLogicTaskContext> DataItemHandler { get; }
        void Execute(TLogicTaskContext taskContext);
    }
}