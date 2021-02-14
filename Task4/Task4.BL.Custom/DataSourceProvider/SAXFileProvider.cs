using Task4.Domain.FileProviders;
using System.IO;

namespace Task4.BL.Custom.DataSourceProviders
{
    public class SAXFileProvider : BaseFileProvider<CsvDTO>, IDataSourceProvider<CsvDTO>
    {
        bool _isCancelled = false;

        public SAXFileProvider() : base()
        {
        }
        public SAXFileProvider(string sourceFolder, string destFolder)
            : base(sourceFolder, destFolder)
        {
        }

        public void Cancel()
        {
            _isCancelled = false;
        }

        public void Dispose()
        {
            // nothing to dispose
        }

        public void Start()
        {
            foreach (var c in
            Directory.GetFiles(
                SourceFolder,
                SearchPattern,
                SearchOption.TopDirectoryOnly))
            {
                OnNew(this, new CsvParser(c, this.DestFolder));
                if (_isCancelled)
                {
                    break;
                }
            }
        }

        public void Stop()
        {
            Cancel();
        }

    }
}