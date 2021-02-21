using System;
using Task4.DAL.Csv;
using Task4.Domain.LogicTaskContexts;

namespace Task4.BL.Custom
{
    public class CustomLogicTaskContext : LogicTaskContext<CSVDTO>
    {
        public Guid Session { get; set; }
    }
}