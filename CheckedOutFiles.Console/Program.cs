using CsvHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Assemblies;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace CheckedOutFiles.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Program prog = new Program();
            prog.GetPathData();
        }

        private void GetPathData()
        {
            string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string assemblyDirectory = System.IO.Path.GetDirectoryName(assemblyPath);

            string dataFile = assemblyDirectory + "\\Data.csv";

            using (var reader = new StreamReader(dataFile))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<Repositories.Entities.PathEntity>();

                    foreach(var record in records)
                    {
                        this.CheckPath(record.Path, record.Recursive);
                    }
                }
            }
        }

        private void CheckPath(string currentPath, bool recursive)
        {
            if (Directory.Exists(currentPath + "\\.git"))
            {

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "git",
                        Arguments = "status --porcelain",
                        WorkingDirectory = currentPath,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                if (!String.IsNullOrEmpty(output))
                {
                    System.Console.WriteLine(currentPath);
                    System.Console.WriteLine("=================================================");
                    System.Console.WriteLine(output);
                    System.Console.WriteLine("");
                }
            }

            if (recursive)
            {
                DirectoryInfo dir = new DirectoryInfo(currentPath);

                foreach (var subDir in dir.GetDirectories())
                {
                    this.CheckPath(subDir.FullName, recursive);
                }
            }
        }
    }
}
