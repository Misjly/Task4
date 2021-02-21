using System;
using System.IO;
using Task4.DAL.Csv;
using Task4.Domain.Absractions;
using Task4.Domain.FileProviders;

namespace Task4.DAL.DataSourceProviders
{
    public class WatcherFileProvider : BaseFileProvider<CSVDTO>, IDataSourceProvider<CSVDTO>, IProcessHandler, IDisposable
    {
        protected FileSystemWatcher Watcher { get; set; }
        public WatcherFileProvider()
            : base()
        {
            Watcher = new FileSystemWatcher();
            Watcher.Path = SourceFolder;
            Watcher.Filter = SearchPattern;
            Watcher.Created += OnFileSystemEvent;
        }

        public WatcherFileProvider(FileSystemWatcher fileSystemWatcher)
            : base()
        {
            Watcher = fileSystemWatcher;
            Watcher.Path = SourceFolder;
            Watcher.Filter = SearchPattern;
            Watcher.Created += OnFileSystemEvent;
        }


        public void Start()
        {
            if (Watcher != null)
            {
                Watcher.EnableRaisingEvents = true;
            }
        }

        public void Stop()
        {
            if (Watcher != null)
            {
                Watcher.EnableRaisingEvents = false;
            }
        }

        public void Dispose()
        {
            if (Watcher != null)
            {
                Watcher.Dispose();
                GC.SuppressFinalize(this);
                Watcher = null;
            }
        }

        protected void OnFileSystemEvent(object sender, FileSystemEventArgs e)
        {
            OnNew(this, new CSVParser(e.FullPath, this.DestFolder));
        }
    }
}