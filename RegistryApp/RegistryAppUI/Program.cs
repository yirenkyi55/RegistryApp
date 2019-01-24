using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegistryAppUI
{
    static class Program
    {
        /// <summary>C:\Users\FuncTionZ\source\repos\RegistryApp\RegistryAppUI\Program.cs
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmStartup());

        }
    }
}
