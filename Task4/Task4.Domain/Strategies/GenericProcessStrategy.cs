using Task4.Domain.LogicTaskContexts;
using Task4.Domain.LogicTaskContexts.Factories;
using Task4.Domain.LogicTaskHandlers;
using Task4.Domain.Strategies.Factories;
using System;
using System.Threading.Tasks;
using Task4.Domain.Absractions;
using Task4.Domain.FileProviders;
using Task4.Domain.CSVParsing;

namespace Task4.Domain.Strategies
{
    public class GenericProcessStrategy<TDataItem, TLogicTaskContext>
        : IProcessHandler, IDisposable
        where TLogicTaskContext : LogicTaskContext<TDataItem>
    {
        private bool _isRunning = false;
        private bool _disposed = false;

        public event EventHandler PreProcessing;
        public event EventHandler PostProcessing;
        protected ILogicTaskContextFactory<TLogicTaskContext, TDataItem> LogicTaskContextFactory { get; set; }
        protected ILogicTaskStrategyFactory<TDataItem, TLogicTaskContext> TaskStrategyFactory { get; set; }

        #region DataSourceProvider
        private IDataSourceProvider<TDataItem> _dataSourceProvider;
        public IDataSourceProvider<TDataItem> DataSourceProvider
        {
            get { return _dataSourceProvider; }
            set
            {
                lock (TaskHandler)
                {
                    if (!_isRunning && !_disposed)
                    {
                        if (_dataSourceProvider != null)
                        {
                            _dataSourceProvider.New -= OnNewDataSourceHandler;
                        }
                        _dataSourceProvider = value;
                        _dataSourceProvider.New += OnNewDataSourceHandler;
                    }
                }
            }
        }
        #endregion

        protected ILogicTaskHandler TaskHandler { get; private set; }

        public GenericProcessStrategy(
            ILogicTaskHandler taskHandler,
            ILogicTaskStrategyFactory<TDataItem, TLogicTaskContext> taskStrategyFactory,
            ILogicTaskContextFactory<TLogicTaskContext, TDataItem> taskContextFactory
            )
        {
            TaskHandler = taskHandler;
            TaskStrategyFactory = taskStrategyFactory;
            LogicTaskContextFactory = taskContextFactory;
        }
        protected virtual void OnPreProcessing(object sender, EventArgs args)
        {
            PreProcessing?.Invoke(sender, args);
        }

        protected virtual void OnPostProcessing(object sender, EventArgs args)
        {
            PostProcessing?.Invoke(sender, args);
        }

        public virtual void OnNewDataSourceHandler(object sender, IDataSource<TDataItem> dataSource)
        {
            RunLogicTask(dataSource);
        }

        public Task RunLogicTask(IDataSource<TDataItem> dataSource)
        {
            lock (TaskHandler.SyncRoot)
            {
                if (this._isRunning && !_disposed)
                {
                    var taskContext = LogicTaskContextFactory.CreateInstance();
                    taskContext.DataSource = dataSource;
                    taskContext.Current = new Task(
                        () =>
                        {
                            using (var taskStrategy = TaskStrategyFactory.CreateInstance(taskContext))
                            {
                                taskStrategy.Execute(taskContext);
                                Task temp = taskContext.Current;
                                if (!TaskHandler.TryTake(out temp))
                                {
                                    throw new InvalidOperationException("TaskManager was unable to remove a task");
                                }
                            }
                        },
                        TaskHandler.TaskFactory.CancellationToken,
                        TaskHandler.TaskFactory.CreationOptions);

                    if (TaskHandler.TryAdd(taskContext.Current))
                    {
                        taskContext.Current.Start(
                            TaskHandler.TaskFactory.Scheduler);
                    }
                    else
                    {
                        throw new InvalidOperationException("Unable add to the container a started task");
                    }
                    return taskContext.Current;
                }
                return null;
            }
        }

        #region  IProcessHandler
        public void Start()
        {
            if (DataSourceProvider != null && !_isRunning)
            {
                OnPreProcessing(this, null);
                _isRunning = true;
                DataSourceProvider.Start();
            }
        }

        public void Stop()
        {
            if (!_disposed && DataSourceProvider != null && _isRunning)
            {
                DataSourceProvider.New -= OnNewDataSourceHandler;
                DataSourceProvider.Stop();
                Wait();
                OnPostProcessing(this, null);
                _isRunning = false;
            }
        }
        public void Wait()
        {
            lock (TaskHandler.SyncRoot)
            {
                TaskHandler.WaitAll();
            }
        }
        #endregion


        #region IDisposable
        public void Dispose()
        {
            if (!_disposed)
            {
                Stop();
                DataSourceProvider?.Dispose();

                PreProcessing = null;
                PostProcessing = null;
                LogicTaskContextFactory = null;
                TaskStrategyFactory = null;
                TaskHandler = null;

                GC.SuppressFinalize(this);
                _disposed = true;
            }
        }
        ~GenericProcessStrategy()
        {
            Dispose();
        }
        #endregion
    }
}