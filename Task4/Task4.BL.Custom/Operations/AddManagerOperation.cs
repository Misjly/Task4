using Task4.Domain.Absractions;
using Task4.DAL.Repositories;
using Task4.DAL.Models;
using System;
using System.Transactions;

namespace Task4.BL.Custom.Operations
{
    public class AddManagerOperation : IUnitOfWork
    {
        public Manager Manager { get; set; }

        protected IGenericRepository<Manager> ManagerRepository;

        protected TransactionScope Scope { get; set; }

        public AddManagerOperation(
            IGenericRepository<Manager> userRepository,
            TransactionScope scope
            )
        {
            ManagerRepository = userRepository;
            Scope = scope;
        }

        public void Commit()
        {
            Scope.Complete();
        }

        public void Execute()
        {
            try
            {
                if (ManagerRepository.SingleOrDefault(x => x.SecondName == Manager.SecondName) == null)
                {
                    ManagerRepository.Add(Manager);
                    ManagerRepository.Save();
                }
                Scope.Complete();
            }
            catch (NullReferenceException e)
            {
                Rollback();
                throw e;
            }
            catch (TransactionException e)
            {
                Rollback();
                throw new InvalidOperationException("Adding manager failed", e);
            }
        }
        public void Rollback()
        {
            
        }
    }
}