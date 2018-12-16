using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Updater
{
    public class FileIndexer : 
    {
        public FileIndexer()
        {
                       //creating a DirectoryInfo object
            DirectoryInfo mydir = new DirectoryInfo(@"../../../Nethereum.Worbooks.Tests/Nethereum.Worbooks.Tests");

            //var matches = Regex.Matches(text, @"test", RegexOptions.Multiline);
            // getting the files in the directory, their names and size
            FileInfo[] f = mydir.GetFiles();
            foreach (FileInfo file in f)
            {
                if (file.Name.Contains("UnitTest"))
                {
                Console.WriteLine("File Name: {0} Size: {1}", file.Name, file.Length);
                
                }
                
            }
            Console.ReadKey();
            Console.ReadLine();
        }
    }
}