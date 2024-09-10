// TO DO:
// * Handle icons

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Shell;

namespace jr
{
    public class App : Application
    {
        static string[] extensions = new string[] { ".exe", ".bat", ".cmd", ".lnk" };

        [STAThread]
        public static void Main() => new App().Run();

        public App()
        {
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Apps");
            if (!Directory.Exists(dir))
            {
                dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            var list = new JumpList();

            foreach (var i in GetExecutableItems(dir))
            {
                list.JumpItems.Add(i);
            }

            JumpList.SetJumpList(this, list);

            this.Shutdown();
        }

        IEnumerable<JumpTask> GetExecutableItems(string dir)
        {
            var category = new DirectoryInfo(dir).Name;
            foreach (var i in Directory.GetFiles(dir))
            {
                var item = i;
                var ext = Path.GetExtension(item).ToLowerInvariant();

                //if (ext == ".lnk")
                //{
                //    item = NativeMethods.ResolveShortcut(i);
                //    ext = Path.GetExtension(item).ToLowerInvariant();
                //}

                if (!extensions.Contains(ext))
                    continue;

                var title = Path.GetFileNameWithoutExtension(i);
                yield return CreateTask(title, item, category);
            }
            yield break;
        }

        JumpTask CreateTask(string title, string path, string category)
        {
            return new JumpTask() { Title = title, ApplicationPath = path, CustomCategory = category };
        }
    }
}