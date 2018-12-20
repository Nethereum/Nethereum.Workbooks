using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Updater
{
    public class FileIndexer
    {
        public FileIndexer()
        {
            DirectoryInfo mydir = new DirectoryInfo(@"../../../Nethereum.Worbooks.Tests");

            FileInfo[] f = mydir.GetFiles();
            foreach (FileInfo file in f)
            {
                if (file.Name.Contains("UnitTest"))
                {
                        Nugets.Add(new Nuget() { Path = file.DirectoryName });
                    //for (int i = 0; i < Nugets.Count; i++)
                    //{
                    //    Nugets.Add(new Nuget() { Path = f[i].DirectoryName });
                    //}
                    System.Console.WriteLine("File Name: {0} Size: {1}", file.Name, file.DirectoryName);
                }
            }
            //for (int i = 0; i < Nugets.Count; i++)
            //{
            //    if (mydir.Name.Contains("UnitTest"))
            //    {
            //        Nugets.Add(new Nuget() { Path = f[i].DirectoryName });
            //        Console.WriteLine("File Name: {0} Size: {1}", f[1].Name, f[i].DirectoryName);
            //        Console.WriteLine(Nugets);
            //    }
            //}
            System.Console.ReadLine();
        }
        List<Nuget> Nugets = new List<Nuget>();
    }
}