using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using FlatFile;

namespace FlatFile
{
    // Define file record with attributes
    class FileRecord
    {
        [FlatFileAttribute(1, 5)]
        public int ID {get; set;}

        [FlatFileAttribute(6, 10)]
        public string Account { get; set; }

        [FlatFileAttribute(16, 10)]
        public double Balance { get; set; }

        [FlatFileAttribute(26, 8)]
        public string Date { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // create temporary list of objects
            List<FileRecord> records = new List<FileRecord>();            
            records.Add(new FileRecord { ID = 1, Account = "12345", Balance = 12.21, Date = "20111211" });
            records.Add(new FileRecord { ID = 2, Account = "23456", Balance = 100.00, Date = "20111211" });
            records.Add(new FileRecord { ID = 3, Account = "98765", Balance = 1000.00, Date = "20111211" });
            records.Add(new FileRecord { ID = 4, Account = "90786", Balance = 99.99, Date = "20111211" });
            records.Add(new FileRecord { ID = 5, Account = "34567", Balance = 123.45, Date = "20111211" });
            
            FlatFile<FileRecord> flatFile = new FlatFile<FileRecord>();
            flatFile.writeFile(records, "FlatFile.txt");

        }
    }
}
