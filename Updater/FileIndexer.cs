using System;
using System.IO;

namespace WindowsFileApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            //creating a DirectoryInfo object
            DirectoryInfo mydir = new DirectoryInfo(@".");

            // getting the files in the directory, their names and size
            FileInfo[] f = mydir.GetFiles();
            foreach (FileInfo file in f)
            {
                Console.WriteLine("File Name: {0} Size: {1}", file.Name, file.Length);
            }
            Console.ReadKey();
            Console.ReadLine();
        }
    }
}