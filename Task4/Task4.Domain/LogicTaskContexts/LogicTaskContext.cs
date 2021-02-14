using System;
using System.Threading;
using System.Threading.Tasks;
using Task4.Domain.CSVParsing;

namespace Task4.Domain.LogicTaskContexts
{
    public class LogicTaskContext<TDataItem>
    {
        public CancellationToken CancellationToken { get; set; }
        public Task Current { get; set; }
        public Exception Exception { get; set; }
        public IDataSource<TDataItem> DataSource { get; set; }

        public TDataItem DataItem { get; set; }
    }
}