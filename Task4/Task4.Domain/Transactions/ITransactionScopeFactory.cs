using System.Transactions;

namespace Task4.Domain.Transactions
{
    public interface ITransactionScopeFactory
    {
        TransactionScope Create(TransactionScopeOption? scopeOption = null, IsolationLevel? isolation = null);
    }
}