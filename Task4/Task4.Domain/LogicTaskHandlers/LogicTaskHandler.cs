using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Task4.Domain.LogicTaskHandlers
{
    public class LogicTaskHandler : ILogicTaskHandler
    {
        public TaskFactory TaskFactory { get; private set; }

        private IProducerConsumerCollection<Task> _items { get; set; }

        public int Count => _items.Count;

        public object SyncRoot => _items;

        public bool IsSynchronized => true;

        public LogicTaskHandler(TaskFactory factory, IProducerConsumerCollection<Task> sourceContainer)
        {
            TaskFactory = factory;
            _items = sourceContainer;
        }

        public bool TryAdd(Task task)
        {
            return _items.TryAdd(task);
        }
        public bool TryTake(out Task task)
        {
            return _items.TryTake(out task);
        }
        public bool WaitAll(int timeOut = 0)
        {
            if (timeOut == 0)
            {
                lock (this)
                {
                    Task.WaitAll(ToArray());
                    return true;
                }
            }
            else
            {
                lock (this)
                {
                    return Task.WaitAll(ToArray(), timeOut);
                }
            }
        }
        public void CopyTo(Task[] array, int index)
        {
            _items.CopyTo(array, index);
        }

        public Task[] ToArray()
        {
            return _items.ToArray();
        }
        public IEnumerator<Task> GetEnumerator()
        {
            return _items.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        public void CopyTo(Array array, int index)
        {
            _items.CopyTo(array, index);
        }
    }
}