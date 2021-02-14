using Task4.Domain.CSVParsing;
using System;

namespace Task4.BL.Custom.Builders
{
    public class BackupFeatureBuilder : CheckDataIntegrityBuilder
    {
        protected override void Building()
        {
            base.Building();
            BuildBackupFeature();
        }

        protected virtual void BuildBackupFeature()
        {
            TaskStrategyFactory.ActionContainer.OnCompleted += new EventHandler<CustomLogicTaskContext>(
            (sender, context) => { (context.DataSource as IBackupable)?.BackUp(); });
        }
    }
}