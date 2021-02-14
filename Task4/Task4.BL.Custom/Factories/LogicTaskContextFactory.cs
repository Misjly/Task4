using Task4.Domain.LogicTaskContexts.Factories;
using System;

namespace Task4.BL.Custom.Factories
{
    public class CustomLogicTaskContextFactory : ILogicTaskContextFactory<CustomLogicTaskContext, CsvDTO>
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