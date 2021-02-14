using Task4.Domain.Transactions;
using System.Transactions;

namespace Task4.BL.Custom.Factories
{
    public class TransactionScopeFactory : ITransactionScopeFactory
    {
        public TransactionScope Create(TransactionScopeOption? scopeOption = null, IsolationLevel? isolation = null)
        {
                var transactionScope = new TransactionScope(
                   scopeOption ?? TransactionScopeOption.Required,
                   new TransactionOptions { IsolationLevel = isolation ?? IsolationLevel.ReadCommitted },
                   TransactionScopeAsyncFlowOption.Enabled);
            return transactionScope;
        }
    }
}