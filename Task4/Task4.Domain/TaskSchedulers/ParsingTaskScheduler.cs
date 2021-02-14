using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task4.Domain.TaskSchedulers
{
    public class ParsingTaskScheduler : TaskScheduler
    {
        private IProducerConsumerCollection<Task> Tasks { get; set; }

        public ParsingTaskScheduler() : this(new ConcurrentQueue<Task>())
        {

        }

        public ParsingTaskScheduler(IProducerConsumerCollection<Task> source)
        {
            Tasks = source;
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return Tasks.ToList();
        }
        
        protected override void QueueTask(Task task)
        {
            if (!Tasks.TryAdd(task))
            {
                throw new InvalidOperationException("Task cannot be added");
            }
            if (!base.TryExecuteTask(task))
            {
                TryExecuteTaskInline(task, true);
            }
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            Task temp = task;
            if (taskWasPreviouslyQueued)
            {
                if (Tasks.TryTake(out temp))
                {
                    temp.RunSynchronously();
                    return true;
                }
                return false;
            }
            else
            {
                task.RunSynchronously();
                return true;
            }
        }

        public override int MaximumConcurrencyLevel => 3;
    }
}