using System;
using System.Windows.Forms;

namespace ProjectOrganizationApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Views.MainForm());
        }
    }
}
