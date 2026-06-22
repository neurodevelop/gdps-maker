using System;
using System.Windows.Forms;

namespace GDPSMaker
{
    internal static class pr
    {
        [STAThread]
        private static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new mf());
        }
    }
}
