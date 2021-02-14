using Task4.DAL.Contexts;
using Task4.DAL.Repositories;
using Task4.DAL.Repositories.Factories;
using Task4.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Task4.BL.Custom.Factories
{
    public class TransactionalRepositotyFactory : IRepositoryFactory
    {
        IDbContextFactory _factory;
        IDictionary<Type, Type> _container;

        public TransactionalRepositotyFactory()
        {
            _container = new Dictionary<Type, Type>();
            Register<Manager, ConcurentAddGenericRepositoty<Manager>>();
            Register<Sale, GenericRepository<Sale>>();
        }

        public TransactionalRepositotyFactory(IDbContextFactory factory) : this()
        {
            _factory = factory;
        }

        public IGenericRepository<TEntity> CreateInstance<TEntity>(DbContext context) where TEntity : class
        {
            var instance = Activator.CreateInstance(
                _container[typeof(TEntity)],
                context ?? _factory.CreateInstance());
            return instance as IGenericRepository<TEntity>;
        }
        public void Register<TEntity, RepositoryType>()
            where TEntity : class where RepositoryType : IGenericRepository<TEntity>
        {
            _container.Add(typeof(TEntity), typeof(RepositoryType));
        }
    }
}