using Task4.Domain.Transactions;
using System.Transactions;
using System;

namespace Task4.BL.Custom.Factories
{
    public class TransactionScopeFactory : ITransactionScopeFactory
    {
        public TransactionScope Create(TransactionScopeOption? scopeOption = null, IsolationLevel? isolation = null)
        {
            TransactionScope transactionScope = new TransactionScope();
                transactionScope = new TransactionScope(
                   scopeOption ?? TransactionScopeOption.RequiresNew,
                   new TransactionOptions { IsolationLevel = isolation ?? IsolationLevel.ReadCommitted },
                   TransactionScopeAsyncFlowOption.Enabled);
            return transactionScope;
        }
    }
}