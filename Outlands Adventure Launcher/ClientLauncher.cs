using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Outlands_Adventure_Launcher
{
    static class ClientLauncher
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// Read from Windowns Register if you requested to keep your session open, if you requested it then open
        /// the client application, if not then open the application login
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            WindowsRegisterManager windowsRegisterManager = new WindowsRegisterManager();
            Microsoft.Win32.RegistryKey key = windowsRegisterManager.OpenWindowsRegister(true);
            bool keepSessionOpen = Convert.ToBoolean(key.GetValue("KeepSessionOpen"));

            if (keepSessionOpen)
            {
                string username = key.GetValue("Username").ToString();

                ClientAplication clientAplication = new ClientAplication();
                clientAplication.ReceiveClassInstance(clientAplication, username);
                Application.Run(clientAplication);
            }
            else
            {
                ClientLogin clientLogin = new ClientLogin();
                clientLogin.ReceiveClassInstance(clientLogin);
                Application.Run(clientLogin);
            }
        }
    }
}
