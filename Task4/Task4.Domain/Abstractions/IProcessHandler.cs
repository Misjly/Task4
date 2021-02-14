namespace Task4.Domain.Absractions
{
    public interface IProcessHandler
    {
        void Start();
        void Stop();
        void Cancel();
    }
}