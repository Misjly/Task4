using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Task4.Domain.LogicTaskHandlers
{
    public interface ILogicTaskHandler : IProducerConsumerCollection<Task>
    {
        TaskFactory TaskFactory { get; }
        bool WaitAll(int timeOut = 0);
    }
}