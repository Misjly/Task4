using Task4.BL.Custom;
using Task4.BL.Custom.Builders;
using System;

namespace Task4.ConsoleClient
{
    public class ConloseMessageBuilder : BackupFeatureBuilder
    {
        protected override void Building()
        {
            base.Building();
            BildConsoleMessaging();
        }

        protected virtual void BildConsoleMessaging()
        {
            TaskStrategyFactory.ActionContainer.OnCompleted += new EventHandler<CustomLogicTaskContext>(
            (sender, context) => { Console.WriteLine("completed"); });
            TaskStrategyFactory.ActionContainer.OnFaulted += new EventHandler<CustomLogicTaskContext>(
            (sender, context) => { Console.WriteLine($"faulted on while processing {context.DataItem}"); });
            TaskStrategyFactory.ActionContainer.OnCancelled += new EventHandler<CustomLogicTaskContext>(
            (sender, context) => { Console.WriteLine("cancelled"); });
        }

    }
}