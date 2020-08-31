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
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //ClientLogin clientLogin = new ClientLogin();
            //clientLogin.ReceiveClassInstance(clientLogin);
            //Application.Run(clientLogin);

            ClientAplication clientAplication = new ClientAplication();
            clientAplication.ReceiveClassInstance(clientAplication);
            Application.Run(clientAplication);

            //Application.Run(new Form1());
        }
    }
}
