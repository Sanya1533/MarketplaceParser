using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WildberriesParser
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new Form1());
            }
            catch(Exception ex)
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments, Environment.SpecialFolderOption.Create);
                if (!Directory.Exists(Path.Combine(path, "Marketplace Parser")))
                    Directory.CreateDirectory(Path.Combine(path, "Marketplace Parser"));
                path = Path.Combine(path, "Marketplace Parser", "log.txt");
                try
                {
                    using (var file = File.CreateText(path))
                    {
                        file.WriteLine(ex);
                    }
                }
                catch { }
                try
                {
                    try
                    {
                        Process.Start(path);
                    }
                    catch { }
                    MessageBox.Show("Подробности в файле\n"+ path, "Непредвиденная ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch { }
            }
        }
    }
}
