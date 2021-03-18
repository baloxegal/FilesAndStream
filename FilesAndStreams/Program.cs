using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace FilesAndStreams
{
    class Program
    {
        static void Main(string[] args)
        {
            string dir_1Name = @"D:\Amdaris\first\";
            string dir_2Name = @"D:\Amdaris\second\";

            Directory.CreateDirectory(dir_1Name);
            Dictionary<string, DateTimeOffset> dict_files_1 = new Dictionary<string, DateTimeOffset>();

            while (true)
            {
                string[] array_files_1 = Directory.GetFiles(dir_1Name);

                if (array_files_1.Length == 0)
                    continue;

                foreach (var p in array_files_1)
                {
                    FileInfo f_info = new FileInfo(p);
                    string[] array = p.Split(@"\");
                    var filename = array[array.Length - 1];
                    dict_files_1.Add(filename, f_info.LastWriteTime);
                }

                if (!Directory.Exists(dir_2Name))
                {
                    Directory.CreateDirectory(dir_2Name);
                }

                var array_files_2 = Directory.GetFiles(dir_2Name);

                for (int i = 0; i < array_files_2.Length - 1; i++)
                {
                    string[] array = array_files_2[i].Split(@"\");
                     var filename =  array[array.Length - 1];
                    array_files_2[i] = filename;
                }

                if (dict_files_1.Keys.Except(array_files_2).Count() == 0)
                    continue;               

                if (array_files_2.Length == 0)
                {
                    foreach (var p in array_files_1)
                    {
                        string[] array = p.Split(@"\");
                        var filename = array[array.Length - 1];
                        File.Copy(p, $"{dir_2Name}{filename}");
                    }
                }
                else
                {
                    foreach (var p in dict_files_1.Keys)
                    {
                        FileInfo f_info = new FileInfo(p);
                        if (!array_files_2.Contains(p))
                        {
                            f_info.CopyTo($"{dir_2Name}{p}");
                        }
                    }
                    foreach(var p in array_files_2)
                    {
                        FileInfo f_info = new FileInfo(p);
                        if (!dict_files_1.ContainsKey(p))
                        {
                            File.Delete($"{dir_2Name}{p}");                                                           
                        }
                    }
                }
                Thread.Sleep(3000);
            }           
        }
    }
}
