﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Task4.Model.Models;
using Task4.Domain.CSVParsing;

namespace Task4.BL.Custom
{
    public class CsvDTOParser : IDataSource<CsvDTO>, IBackupable
    {
        StreamReader Reader { get; set; }
        string _fileName;
        string _backupFolder;
        private Manager _manager { get; set; }
        public CsvDTOParser(string fileName, string backupFolder)
        {
            var items = fileName.Split(new char[] { '_', '\\' });
            _manager = new Manager();
            _manager.SecondName = items[items.Length - 2];
            _fileName = fileName;
            _backupFolder = backupFolder;
            Reader = new StreamReader(fileName);
        }

        public IEnumerator<CsvDTO> GetEnumerator()
        {
            while (Reader != null && !Reader.EndOfStream)
            {
                var items = Reader
                    .ReadLine()
                    .Split(';')
                    .Select(x => x.Trim())
                    .ToArray();

                yield return new CsvDTO()
                {
                    Date = DateTime.ParseExact(items[0], "ddMMyyyy", null),
                    Client = items[1],
                    Product = items[2],
                    Cost = int.Parse(items[3]),
                    SecondName = _manager.SecondName
                };
            }
            Close();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            if (Reader != null)
            {
                Reader.Dispose();
                GC.SuppressFinalize(this);
                Reader = null;
            }
        }

        public void BackUp()
        {
            try
            {
                Reader?.Dispose();
                FileInfo f = new FileInfo(_fileName);
                File.Move(_fileName, $"{_backupFolder}{f.Name}");
            }
            catch (IOException e)
            {
                throw new InvalidOperationException("cannot backup file", e);
            }
        }

        public void Close()
        {
            Reader?.Close();
        }

        ~CsvDTOParser()
        {
            Dispose();
        }
    }
}