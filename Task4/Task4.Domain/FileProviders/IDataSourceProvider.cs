using System;
using Task4.Domain.Absractions;
using Task4.Domain.CSVParsing;

namespace Task4.Domain.FileProviders
{
    public interface IDataSourceProvider<T> : IDisposable, IProcessHandler
    {
        event EventHandler<IDataSource<T>> New;
    }
}