using System;
using Task4.Domain.LogicTaskContexts;

namespace Task4.Domain.Strategies.Factories
{
    public class LogicTaskStrategyEventHandlerContainer<TLogicTaskContext, TDataItem> where TLogicTaskContext : LogicTaskContext<TDataItem>
    {
        public EventHandler<TLogicTaskContext> OnCompleted { get; set; }
        public EventHandler<TLogicTaskContext> OnFaulted { get; set; }
        public EventHandler<TLogicTaskContext> OnCancelled { get; set; }
        public EventHandler<TLogicTaskContext> OnStarting { get; set; }
        public EventHandler<TLogicTaskContext> OnDataItemHandler { get; set; }
    }
}