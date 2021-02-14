using System;
using System.Collections.Generic;

namespace Task4.Domain.CSVParsing
{
    public interface IDataSource<T> : IEnumerable<T>, IDisposable
    {
        void Close();
    }
}