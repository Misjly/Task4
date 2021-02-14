using Task4.Domain.CSVParsing;
using System;
using System.Configuration;

namespace Task4.Domain.FileProviders
{
    public abstract class BaseFileProvider<TDataItem>
    {
        public string SourceFolder { get; private set; }
        public string DestFolder { get; private set; }
        public string SearchPattern { get; private set; }

        public BaseFileProvider(
            string sourceFolder = null,
            string destFolder = null,
            string searchPattern = null)
            : this()
        {
            if (sourceFolder != null)
            {
                SourceFolder = sourceFolder;
            }
            if (destFolder != null)
            {
                DestFolder = sourceFolder;
            }
            if (searchPattern != null)
            {
                SearchPattern = searchPattern;
            }
        }

        public BaseFileProvider()
        {
            SourceFolder = ConfigurationManager.AppSettings["sourceFolder"];
            DestFolder = ConfigurationManager.AppSettings["destFolder"];
            SearchPattern = ConfigurationManager.AppSettings["searchPattern"];
        }

        public event EventHandler<IDataSource<TDataItem>> New;
        protected virtual void OnNew(object sender, IDataSource<TDataItem> dataSource)
        {
            New?.Invoke(sender, dataSource);
        }
    }
}