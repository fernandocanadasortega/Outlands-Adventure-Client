using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Outlands_Adventure_Launcher
{
    public partial class CloseClientAplicationPopUp : Form
    {
        private Form windowApp;
        private string tileSizeSelected;
        private FormClosingEventArgs closeEvent;

        public CloseClientAplicationPopUp(Form windowApp, string tileSizeSelected, FormClosingEventArgs closeEvent)
        {
            InitializeComponent();

            this.windowApp = windowApp;
            this.tileSizeSelected = tileSizeSelected;
            this.closeEvent = closeEvent;

            ExitHeader.Focus();
        }

        private void CloseClientAplicationPopUp_Load(object sender, EventArgs e)
        {
            ExitHeader.Text = ClientLanguage.exit_Header;
            ExitLabel.Text = ClientLanguage.exit_Label;
            ExitButton.Text = ClientLanguage.ExitButton;
            LogoutButton.Text = ClientLanguage.LogoutButton;
        }

        private void SaveData()
        {
            WindowsRegisterManager windowsRegisterManager = new WindowsRegisterManager();
            windowsRegisterManager.SaveWindowPosition(windowApp);

            Microsoft.Win32.RegistryKey key = windowsRegisterManager.OpenWindowsRegister(true);
            key.SetValue("selectedTileSize", tileSizeSelected);

            windowsRegisterManager.CloseWindowsRegister(key);
        }

        #region Exit Button
        private void ExitButton_MouseClick(object sender, MouseEventArgs e)
        {
            SaveData();
            ClientAplication.aplicationClosing = true;
            Application.Exit();
            this.Close();
        }

        private void ExitButton_MouseEnter(object sender, EventArgs e)
        {
            ExitButton.BackColor = Color.FromArgb(230, 230, 230);
            ExitButton.ForeColor = Color.FromArgb(35, 35, 40);
        }

        private void ExitButton_MouseLeave(object sender, EventArgs e)
        {
            ExitButton.BackColor = Color.FromArgb(35, 35, 40);
            ExitButton.ForeColor = Color.FromArgb(230, 230, 230);
        }
        #endregion Exit Button

        #region Logout Button
        private void LogoutButton_MouseClick(object sender, MouseEventArgs e)
        {
            WindowsRegisterManager windowsRegisterManager = new WindowsRegisterManager();
            Microsoft.Win32.RegistryKey key = windowsRegisterManager.OpenWindowsRegister(true);
            bool keepSessionOpen = Convert.ToBoolean(key.GetValue("KeepSessionOpen"));

            if (keepSessionOpen)
            {
                key.DeleteValue("KeepSessionOpen");
                key.DeleteValue("Username");
            }

            ClientAplication.aplicationClosing = true;
            SaveData();
            Application.Restart();
            this.Close();
        }

        private void LogoutButton_MouseEnter(object sender, EventArgs e)
        {
            LogoutButton.BackColor = Color.FromArgb(230, 230, 230);
            LogoutButton.ForeColor = Color.FromArgb(35, 35, 40);
        }

        private void LogoutButton_MouseLeave(object sender, EventArgs e)
        {
            LogoutButton.BackColor = Color.FromArgb(35, 35, 40);
            LogoutButton.ForeColor = Color.FromArgb(230, 230, 230);
        }
        #endregion Logout Button

        #region Cancel Exit
        private void CancelExit_MouseClick(object sender, MouseEventArgs e)
        {
            closeEvent.Cancel = true;
            this.Close();
        }

        private void CancelExit_MouseEnter(object sender, EventArgs e)
        {
            CancelExit.BackColor = Color.FromArgb(230, 230, 230);
            CancelExit.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.Dark_Close;
        }

        private void CancelExit_MouseLeave(object sender, EventArgs e)
        {
            CancelExit.BackColor = Color.FromArgb(35, 35, 40);
            CancelExit.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.close;
        }
        #endregion Cancel Exit
    }
}