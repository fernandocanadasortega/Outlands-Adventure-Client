using System;
using System.Collections.Generic;
using System.Drawing;
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

            Rectangle resolution = Screen.PrimaryScreen.Bounds;

            WindowsRegisterManager windowsRegisterManager = new WindowsRegisterManager();
            Microsoft.Win32.RegistryKey key = windowsRegisterManager.OpenWindowsRegister(true);
            bool keepSessionOpen = Convert.ToBoolean(key.GetValue("KeepSessionOpen"));

            string selectedResolution = "";
            if (key.GetValue("SelectedResolution") != null)
            {
                selectedResolution = key.GetValue("SelectedResolution").ToString();
            }

            if (selectedResolution.Equals(""))
            {
                ClientLoginLarge clientLoginLarge = new ClientLoginLarge();
                if (resolution.Width > clientLoginLarge.Size.Width && resolution.Height > clientLoginLarge.Size.Height)
                {
                    selectedResolution = "1280x720";
                }
                else
                {
                    selectedResolution = "800x600";
                }

                key.SetValue("SelectedResolution", selectedResolution);
            }

            if (keepSessionOpen)
            {
                string username = key.GetValue("Username").ToString();

                ClientAplication clientAplication = new ClientAplication();
                clientAplication.ReceiveClassInstance(clientAplication, username);
                Application.Run(clientAplication);
            }
            else
            {
                if (selectedResolution.Equals("1280x720"))
                {
                    ClientLoginLarge clientLoginLarge = new ClientLoginLarge();
                    clientLoginLarge.ReceiveClassInstance(clientLoginLarge);
                    Application.Run(clientLoginLarge);
                }
                else
                {
                    ClientLoginSmall clientLoginSmall = new ClientLoginSmall();
                    clientLoginSmall.ReceiveClassInstance(clientLoginSmall);
                    Application.Run(clientLoginSmall);
                }
            }
        }
    }
}
