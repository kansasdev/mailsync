using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AcceptLicCA
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FormLicCA f = new FormLicCA();
            Application.Run(f);

            if(!f.CertInstalled)
            {
                return -1;
            }
            else
            {
                return 0;
            }

        }
    }
}
