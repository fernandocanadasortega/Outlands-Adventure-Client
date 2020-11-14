using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Outlands_Adventure_Launcher
{
    public partial class CloseClientAplicationPopUp : Form
    {
        private Form windowApp;
        private string tileSizeSelected;
        private FormClosingEventArgs closeEvent;
        private CancellationTokenSource downloadCancellationTokenSource;

        public CloseClientAplicationPopUp(Form windowApp, string tileSizeSelected, FormClosingEventArgs closeEvent, 
            CancellationTokenSource downloadCancellationTokenSource)
        {
            InitializeComponent();

            this.windowApp = windowApp;
            this.tileSizeSelected = tileSizeSelected;
            this.closeEvent = closeEvent;
            this.downloadCancellationTokenSource = downloadCancellationTokenSource;

            ExitHeader.Focus();
        }

        public CloseClientAplicationPopUp(Form windowApp, string tileSizeSelected)
        {
            this.windowApp = windowApp;
            this.tileSizeSelected = tileSizeSelected;
        }

        private void CloseClientAplicationPopUp_Load(object sender, EventArgs e)
        {
            ExitHeader.Text = LanguageResx.ClientLanguage.exit_Header;
            ExitLabel.Text = LanguageResx.ClientLanguage.exit_Label;
            ExitButton.Text = LanguageResx.ClientLanguage.ExitButton;
            LogoutButton.Text = LanguageResx.ClientLanguage.LogoutButton;

            MultipleResources.CalculateCenterLocation(this, ExitLabel, 30);
        }

        private string SaveData()
        {
            WindowsRegisterManager windowsRegisterManager = new WindowsRegisterManager();
            windowsRegisterManager.SaveWindowPosition(windowApp);

            Microsoft.Win32.RegistryKey key = windowsRegisterManager.OpenWindowsRegister(true);
            key.SetValue("selectedTileSize", tileSizeSelected);

            string resolution = key.GetValue("SelectedResolution").ToString();

            windowsRegisterManager.CloseWindowsRegister(key);

            return resolution;
        }

        #region Exit Button
        private void ExitButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (downloadCancellationTokenSource != null)
            {
                if (!downloadCancellationTokenSource.IsCancellationRequested)
                    downloadCancellationTokenSource.Cancel();
            }

            string resolution = SaveData();
            if (resolution.Equals("1280x720"))
            {
                ClientAplicationLarge.aplicationClosing = true;
            }
            else
            {
                ClientAplicationSmall.aplicationClosing = true;
            }

            Environment.Exit(0);
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
        public void LogoutButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (downloadCancellationTokenSource != null)
            {
                if (!downloadCancellationTokenSource.IsCancellationRequested)
                    downloadCancellationTokenSource.Cancel();
            }

            WindowsRegisterManager windowsRegisterManager = new WindowsRegisterManager();
            Microsoft.Win32.RegistryKey key = windowsRegisterManager.OpenWindowsRegister(true);
            bool keepSessionOpen = Convert.ToBoolean(key.GetValue("KeepSessionOpen"));

            if (keepSessionOpen)
            {
                key.DeleteValue("KeepSessionOpen");
                key.DeleteValue("Username");
            }

            string resolution = SaveData();
            if (resolution.Equals("1280x720"))
            {
                ClientAplicationLarge.aplicationClosing = true;
            }
            else
            {
                ClientAplicationSmall.aplicationClosing = true;
            }

            MultipleResources.RestartApp(Process.GetCurrentProcess().Id, Process.GetCurrentProcess().ProcessName);
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
            MultipleResources.ShowToolTip(CancelExit, LanguageResx.ClientLanguage.button_Close_Lowercase);
            CancelExit.BackColor = Color.FromArgb(230, 230, 230);
            CancelExit.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.Dark_Close;
        }

        private void CancelExit_MouseLeave(object sender, EventArgs e)
        {
            MultipleResources.HideToolTip(CancelExit);
            CancelExit.BackColor = Color.FromArgb(35, 35, 40);
            CancelExit.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.close;
        }
        #endregion Cancel Exit
    }
}