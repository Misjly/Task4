using Task4.Domain.LogicTaskContexts.Factories;
using System;
using Task4.DAL.Csv;

namespace Task4.BL.Custom.Factories
{
    public class CustomLogicTaskContextFactory : ILogicTaskContextFactory<CustomLogicTaskContext, CSVDTO>
    {
        public CustomLogicTaskContext CreateInstance()
        {
            return new CustomLogicTaskContext()
            {
                Session = Guid.NewGuid()
            };
        }
    }
}