using System;
using System.IO;
using Task4.Domain.Absractions;
using Task4.Domain.FileProviders;

namespace Task4.BL.Custom.DataSourceProviders
{
    public class WatcherFileProvider : BaseFileProvider<CsvDTO>, IDataSourceProvider<CsvDTO>, IProcessHandler, IDisposable
    {
        protected FileSystemWatcher Watcher { get; set; }
        public WatcherFileProvider()
            : base()
        {
            Watcher = new FileSystemWatcher();
            Watcher.Path = SourceFolder;
            Watcher.Filter = this.SearchPattern;
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

        ~WatcherFileProvider()
        {
            Dispose();
        }

        protected void OnFileSystemEvent(object sender, FileSystemEventArgs e)
        {
            OnNew(this, new CsvDTOParser(e.FullPath, this.DestFolder));
        }

        public void Cancel()
        {
            Stop();
        }
    }
}