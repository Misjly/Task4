using Task4.Domain.LogicTaskContexts;
using System;
using System.Threading.Tasks;

namespace Task4.Domain.Strategies
{
    public abstract class GenericLogicTaskStrategy<TDataItem, TLogicTaskContext>
        : IGenericLogicTaskStrategy<TDataItem, TLogicTaskContext>
        where TLogicTaskContext : LogicTaskContext<TDataItem>
    {
        private bool _isDisposed = false;

        public event EventHandler<TLogicTaskContext> TaskStarting;
        public event EventHandler<TLogicTaskContext> TaskCancelled;
        public event EventHandler<TLogicTaskContext> TaskCompleted;
        public event EventHandler<TLogicTaskContext> TaskFaulted;
        public abstract EventHandler<TLogicTaskContext> DataItemHandler { get; set; }

        public GenericLogicTaskStrategy()
        {
        }

        protected virtual void OnTaskStarting(object sender, TLogicTaskContext context)
        {
            TaskStarting?.Invoke(sender, context);
        }
        protected virtual void OnTaskCancelled(object sender, TLogicTaskContext context)
        {
            TaskCancelled?.Invoke(sender, context);
        }

        protected virtual void OnTaskCompleted(object sender, TLogicTaskContext context)
        {
            TaskCompleted?.Invoke(sender, context);
        }

        protected void OnTaskFaulted(object sender, TLogicTaskContext context)
        {
            TaskFaulted?.Invoke(sender, context);
        }

        public virtual void Execute(TLogicTaskContext taskContext)
        {
            //try
            //{
                OnTaskStarting(this, taskContext);
                foreach (var dto in taskContext.DataSource)
                {
                    taskContext.DataItem = dto;
                    DataItemHandler?.Invoke(this, taskContext);
                }
                OnTaskCompleted(this, taskContext);
            //}
            //catch (TaskCanceledException)
            //{
            //    OnTaskCancelled(this, taskContext);
            //}
            //catch (Exception e)
            //{
            //    taskContext.Exception = e;
            //    OnTaskFaulted(this, taskContext);
            //}
            //finally
            //{
            //    taskContext.DataSource?.Dispose();
            //}
        }

        protected void UnbindAllEvents()
        {
            TaskCompleted = null;
            TaskCancelled = null;
            TaskFaulted = null;
            TaskStarting = null;
        }

        protected virtual void Disposing()
        {
            if (!_isDisposed)
            {
                UnbindAllEvents();
                DataItemHandler = null;
                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                Disposing();
            }
        }

    }
}