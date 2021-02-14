namespace Task4.Domain.Absractions
{
    public interface IUnitOfWork
    {
        void Commit();
        void Rollback();
        void Execute();
    }
}