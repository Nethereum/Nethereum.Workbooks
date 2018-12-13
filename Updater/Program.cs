using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            //creating a DirectoryInfo object
            DirectoryInfo mydir = new DirectoryInfo(@"..\..\..\Nethereum.Worbooks.Tests\Nethereum.Worbooks.Tests");

            // getting the files in the directory, their names and size
            FileInfo[] f = mydir.GetFiles();
            foreach (FileInfo file in f)
            {
                Console.WriteLine("File Name: {0} Size: {1}", file.Name, file.Length);
            }
            Console.ReadKey();
            //Console.ReadLine();
        }
    }
}
