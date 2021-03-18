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
            string dir_watcher_name = @"D:\Amdaris\WatcherFolder\";

            //This is My FileChecker

            //string dir_1_name = @"D:\Amdaris\first\";
            //string dir_2_name = @"D:\Amdaris\second\";

            //Directory.CreateDirectory(dir_1_name);
            //List<string> last_list_files_1 = Directory.GetFiles(dir_1_name).ToList();
            //List<string> new_list_files_1 = new List<string>();

            //while (true)
            //{
            //    if (!Directory.Exists(dir_1_name))
            //        Directory.CreateDirectory(dir_1_name);

            //    new_list_files_1 = Directory.GetFiles(dir_1_name).ToList();

            //    if (last_list_files_1.OrderBy(s => s).SequenceEqual(new_list_files_1.OrderBy(s => s)))
            //        continue;

            //    var except_new = new_list_files_1.Except(last_list_files_1);
            //    var except_last = last_list_files_1.Except(new_list_files_1);

            //    last_list_files_1 = new_list_files_1;

            //    if (!Directory.Exists(dir_2_name))
            //        Directory.CreateDirectory(dir_2_name);

            //    foreach (var p in except_new)
            //        File.Copy(p, p.Replace("first", "second"));

            //    foreach (var p in except_last)
            //        File.Delete(p.Replace("second", "first"));

            //    Thread.Sleep(5000);
            //}

            //This is Microsoft FileChecker

            /*using*/
            var watcher = new FileSystemWatcher(dir_watcher_name);

            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;


            //new FileSystemEventHandler(OnChanged)
            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            
            watcher.Filter = "*.txt";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

            //This is Driver Info

            DriveInfo[] myDrives = DriveInfo.GetDrives();
             
            foreach (DriveInfo d in myDrives)
            {
                Console.WriteLine("Name: {0}", d.Name);
                Console.WriteLine("Type: {0}", d.DriveType);
                if (d.IsReady)
                {
                    Console.WriteLine("Free space: {0}", d.TotalFreeSpace);
                    Console.WriteLine("Format: {0}", d.DriveFormat);
                    Console.WriteLine("Label: {0}\n", d.VolumeLabel);
                }
            }

            //StreamReader 

            string fn = @"D:\Amdaris\WatcherFolder\NewTextDocument.txt";
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(fn);
                for (string line = reader.ReadLine(); line != null;
                          line = reader.ReadLine())
                    Console.WriteLine(line);
            }
            catch (IOException e)
            { Console.WriteLine(e.Message); }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            //Writer

            string[] myTasks = {"Repair water tap",
                    "Buy bread and milk",
                    "Study last C# features",
                    "Complete lecture materials",
                    "Phone to mum" };
            File.WriteAllLines(@"D:\Amdaris\WatcherFolder\NewTextDocument.txt", myTasks);
            foreach (string task in
               File.ReadAllLines(@"D:\Amdaris\WatcherFolder\NewTextDocument.txt"))
            {
                Console.WriteLine("To do: {0}", task);
            }


            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }
        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            Console.WriteLine($"Changed: {e.FullPath}");
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            Console.WriteLine(value);
        }

        private static void OnDeleted(object sender, FileSystemEventArgs e) =>
            Console.WriteLine($"Deleted: {e.FullPath}");

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"Renamed:");
            Console.WriteLine($"    Old: {e.OldFullPath}");
            Console.WriteLine($"    New: {e.FullPath}");
        }

        private static void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private static void PrintException(Exception ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }
    }
}

