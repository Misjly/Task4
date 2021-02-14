using System;
using Task4.Domain.LogicTaskContexts;

namespace Task4.BL.Custom
{
    public class CustomLogicTaskContext : LogicTaskContext<CsvDTO>
    {
        public Guid Session { get; set; }
    }
}