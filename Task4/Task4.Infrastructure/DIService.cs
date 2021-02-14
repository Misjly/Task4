using System;
using System.Collections.Generic;

namespace Task4.Infrastructure
{
    public class DIService
    {
        public static DIService Instance { get; private set; }
        private IDictionary<Type, Func<object>> container = new Dictionary<Type, Func<object>>();
        public T Resolve<T>()
        {
            return (T)container[typeof(T)].Invoke();
        }

        static DIService()
        {
            Instance = new DIService();
        }

        public void Register(Type type, Func<object> creator)
        {
            container.Add(type, creator);
        }

    }
}