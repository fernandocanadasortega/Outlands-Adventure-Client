﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Data.Linq;
using System.Diagnostics;

namespace Outlands_Adventure_Launcher
{
    #region Password Strenght
    /// <summary>
    /// Class in charge of checking a password strength
    /// </summary>
    static class PasswordStrength
    {
        /// <summary>
        /// Values of a password
        /// </summary>
        public enum PasswordScore
        {
            Empty = 0,
            VeryWeak = 1,
            Weak = 2,
            Medium = 3,
            Strong = 4,
            VeryStrong = 5
        }

        /// <summary>
        /// Evaluate a password according to certain patterns
        /// </summary>
        /// <param name="password">String, Password to be evaluated</param>
        /// <returns>PasswordScore, Value of a password</returns>
        public static PasswordScore CheckStrength(string password)
        {
            int score = 1;
            if (password.Length < 1)
                return PasswordScore.Empty;
            if (password.Length < 4)
                return PasswordScore.VeryWeak;
            if (password.Length >= 8) score++;
            if (password.Length >= 12) score++;
            if (Regex.IsMatch(password, @"[0-9]+(\.[0-9][0-9]?)?"))   //number only //"^\d+$" if you need to match more than one digit.
                score++;
            if (Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z]).+$")) //both, lower and upper case
                score++;
            if (Regex.IsMatch(password, @"[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]")) //^[A-Z]+$
                score++;
            return (PasswordScore)score;
        }

        /// <summary>
        /// Show the value of security of a password after being evaluated
        /// </summary>
        /// <param name="NewPasswordTextbox">Textbox that contains the password</param>
        /// <param name="NewPasswordStrengthProgressBar">Progressbar that indicate the value of password strength</param>
        /// <param name="NewPasswordStrengthLabel">Label that indicate the value of password strength</param>
        public static void CheckPasswordStrength(TextBox NewPasswordTextbox, ProgressBar NewPasswordStrengthProgressBar,
            Label NewPasswordStrengthLabel)
        {
            PasswordScore passwordScore = CheckStrength(NewPasswordTextbox.Text);

            switch (passwordScore)
            {
                case PasswordScore.Empty:
                    NewPasswordStrengthProgressBar.Visible = false;
                    NewPasswordStrengthLabel.Visible = false;
                    break;
                case PasswordScore.VeryWeak:
                    NewPasswordStrengthProgressBar.Value = 20;
                    NewPasswordStrengthLabel.ForeColor = Color.Red;
                    NewPasswordStrengthLabel.Text = LanguageResx.ClientLanguage.passwordStrenght_VeryWeak;
                    break;

                case PasswordScore.Weak:
                    NewPasswordStrengthProgressBar.Value = 40;
                    NewPasswordStrengthLabel.ForeColor = Color.Red;
                    NewPasswordStrengthLabel.Text = LanguageResx.ClientLanguage.passwordStrenght_Weak;
                    break;

                case PasswordScore.Medium:
                    NewPasswordStrengthProgressBar.Value = 60;
                    NewPasswordStrengthLabel.ForeColor = Color.DarkGoldenrod;
                    NewPasswordStrengthLabel.Text = LanguageResx.ClientLanguage.passwordStrenght_Medium;
                    break;

                case PasswordScore.Strong:
                    NewPasswordStrengthProgressBar.Value = 80;
                    NewPasswordStrengthLabel.ForeColor = Color.Green;
                    NewPasswordStrengthLabel.Text = LanguageResx.ClientLanguage.passwordStrenght_Strong;
                    break;

                case PasswordScore.VeryStrong:
                    NewPasswordStrengthProgressBar.Value = 100;
                    NewPasswordStrengthLabel.ForeColor = Color.Green;
                    NewPasswordStrengthLabel.Text = LanguageResx.ClientLanguage.passwordStrenght_VeryStrong;
                    break;
            }
        }
    }
    #endregion Password Strenght

    #region Send Email
    /// <summary>
    /// Class in charge of sending emails
    /// </summary>
    static class SendEmail
    {
        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="emailDirection">String, Recipient address</param>
        /// <param name="emailSubject">String message header</param>
        /// <param name="emailBody">String, message body</param>
        /// <param name="emailBodyData">String, message body data (in this case is the security code)</param>
        /// <returns>Boolean, true if the email was sent correctly or false if not</returns>
        public static string SendNewEmail(String emailDirection, string emailSubject, string emailBody, string emailBodyData)
        {
            try
            {
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress("outlandsadventure@gmail.com", "Outlands Adventures Inc");
                mail.To.Add(emailDirection);
                mail.Subject = emailSubject;

                mail.IsBodyHtml = true;
                string htmlBody = "<p><h2>" + emailBody + "</h2></p> <br/>" +
                    "<p><h1>" + emailBodyData + "</h1></p>";
                mail.Body = htmlBody;

                smtpServer.Port = 587;
                smtpServer.UseDefaultCredentials = false;
                smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpServer.Credentials = new System.Net.NetworkCredential("outlandsadventure@gmail.com", "Outlands_Client_Password");
                smtpServer.EnableSsl = true;
                smtpServer.Send(mail);

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
    #endregion Send Email

    #region Confimation Code
    /// <summary>
    /// Class in charge of creating the security code
    /// </summary>
    static class CreateConfirmationCode
    {
        static readonly int codeLenght = 8;

        /// <summary>
        /// Obtain the char of every random number generated
        /// </summary>
        /// <returns>String, security code</returns>
        public static string CreateCode()
        {
            string confirmationCode = "";
            Random randomNumber = new System.Random();

            for (int currentCodeCharacter = 0; currentCodeCharacter < codeLenght; currentCodeCharacter++)
            {
                confirmationCode += ((char)GiveMeANumber(randomNumber)).ToString();
            }

            return confirmationCode;
        }

        /// <summary>
        /// Create a random character (number or Upper case letter), excluding number from 58 to 64 
        /// </summary>
        /// <param name="randomNumber">Random class in charge of selecting a random number</param>
        /// <returns>int, random number between 48 and 90</returns>
        private static int GiveMeANumber(Random randomNumber)
        {
            // These numbers are excluded because they are special character in the ascii table
            List<int> exclude = new List<int>() { 58, 59, 60, 61, 62, 63, 64 };
            var range = Enumerable.Range(1, 100).Where(i => !exclude.Contains(i));

            // numbers between 48 (number 0 in ascii table) and 90 (Z in ascii table)
            int index = randomNumber.Next(48, 90 - exclude.Count);
            return range.ElementAt(index);
        }
    }
    #endregion Confimation Code

    #region Resume Manager
    /// <summary>
    /// Class in charge resuming text with SHA algorithm
    /// </summary>
    static class Hash_SHA2
    {
        private static HashAlgorithm hashResume;
        private static Binary binaryOriginalHash;

        /// <summary>
        /// Initialice the SHA algorithm and resume the security code created
        /// </summary>
        /// <param name="confirmationCode">String, security code created</param>
        public static void InitialiceVariables(string confirmationCode)
        {
            hashResume = new SHA256Managed();
            byte[] originalHash = Hash_SHA2.CreateResume(confirmationCode);
            binaryOriginalHash = new Binary(originalHash);
        }

        /// <summary>
        /// Resume an string using the SHA algorithm
        /// </summary>
        /// <param name="confirmationCode">String, security code created</param>
        /// <returns>byte[], string summarized</returns>
        public static byte[] CreateResume(string confirmationCode)
        {
            return hashResume.ComputeHash(Encoding.UTF8.GetBytes(confirmationCode));
        }

        /// <summary>
        /// Verify if the security code created and the security code introduced by the user are the same
        /// </summary>
        /// <param name="confirmationCode">String, security code written by the user</param>
        /// <returns>Bool, true if both codes are the same, false if not</returns>
        public static bool VerifyResume(string confirmationCode)
        {
            byte[] confirmationHash = Hash_SHA2.CreateResume(confirmationCode);

            /* La clase binary es muy eficiente a la hora de hacer equals de dos resumenes hash para ver si son 
               iguales (https://stackoverflow.com/questions/18472867/checking-equality-for-two-byte-arrays/18472958) */
            Binary binaryConfirmationHash = new Binary(confirmationHash);

            return binaryOriginalHash.Equals(binaryConfirmationHash);
        }
    }
    #endregion Resume Manager

    #region Windows register manager
    /// <summary>
    /// Class in charge of opening, closing, writting and reding the windows register
    /// </summary>
    public class WindowsRegisterManager
    {
        private readonly string windowsRegistry = "Outlands_Adventure_Launcher";

        /// <summary>
        /// Abre la entrada del registro de windows si ya existe y si no existe la crea
        /// </summary>
        /// <param name="writeMode">bool, verdadero para escribir en la entrada del registro y false para leer solo</param>
        /// <returns>RegistryKey, entrada del registro de windows</returns>
        public Microsoft.Win32.RegistryKey OpenWindowsRegister(bool writeMode)
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(windowsRegistry, writeMode);

            if (key == null)
            {
                key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(windowsRegistry);
            }

            return key;
        }

        /// <summary>
        /// Cierra la entrada del registro de windows
        /// </summary>
        /// <param name="key">RegistryKey, entrada del registro de windows</param>
        public void CloseWindowsRegister(Microsoft.Win32.RegistryKey key)
        {
            key.Close();
        }

        /// <summary>
        /// Read from the windows register the position of the form and establish it the current form
        /// </summary>
        /// <param name="WindowApp">Form, Current form</param>
        public void LoadWindowPosition(Form WindowApp)
        {
            Microsoft.Win32.RegistryKey key = OpenWindowsRegister(false);

            if (key.GetValue("XPosition") != null && key.GetValue("YPosition") != null)
            {
                WindowApp.Location = new Point((int)key.GetValue("XPosition"), (int)key.GetValue("YPosition"));
            }

            CloseWindowsRegister(key);
        }

        /// <summary>
        /// Save in the windows register the position of the open form
        /// </summary>
        /// <param name="WindowApp">Form, Current form</param>
        public void SaveWindowPosition(Form WindowApp)
        {
            Microsoft.Win32.RegistryKey key = OpenWindowsRegister(true);

            key.SetValue("XPosition", WindowApp.Location.X);
            key.SetValue("YPosition", WindowApp.Location.Y);

            CloseWindowsRegister(key);
        }
    }
    #endregion Windows register manager

    #region Language / Resolution / Combobox Manager

    #region Combobox Manager
    /// <summary>
    /// Class in charge of change the visual appearance of comboboxs
    /// </summary>
    public class ComboboxManager
    {
        /// <summary>
        /// Allow Combo Box text to center aligned
        /// </summary>
        /// <param name="sender">Object that receive the events</param>
        /// <param name="e">Events that occur to the object</param>
        public void Combobox_DrawItem(object sender, DrawItemEventArgs e)
        {
            // By using Sender, one method could handle multiple ComboBoxes
            ComboBox cbx = sender as ComboBox;
            if (cbx != null)
            {
                // Always draw the background
                e.DrawBackground();

                // Drawing one of the items?
                if (e.Index >= 0)
                {
                    // Set the string alignment.  Choices are Center, Near and Far
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;

                    // Set the Brush to ComboBox ForeColor to maintain any ComboBox color settings
                    // Assumes Brush is solid
                    Brush brush = new SolidBrush(cbx.ForeColor);

                    // If drawing highlighted selection, change brush
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                        brush = SystemBrushes.HighlightText;

                    // Draw the string
                    e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, brush, e.Bounds, sf);
                }
            }
        }

        /// <summary>
        /// Close the combobox and lose the focus from the combobox
        /// </summary>
        /// <param name="configurationPanel">Panel, panel that will gain the focus</param>
        public void Combobox_DropDownClosed(Panel configurationPanel)
        {
            configurationPanel.Focus();
        }
    }
    #endregion Combobox Manager

    #region Language Manager

    /// <summary>
    /// Class in charge of change the aplication language
    /// </summary>
    public class LanguageManager
    {
        private ClientLoginLarge clientLoginLarge;
        private ClientLoginSmall clientLoginSmall;
        private ClientAplicationLarge clientAplicationLarge;
        private ClientAplicationSmall clientAplicationSmall;

        /// <summary>
        /// Change the language stored in the windows registry and change the language of the application
        /// Cambia el idioma del almacenado en la entrada del registro de windows y cambia el idioma de la aplicación
        /// </summary>
        /// <param name="languageCombobox">Combobox with all the languages</param>
        public void LanguageCombobox_LanguageChanged(ComboBox languageCombobox)
        {
            string[] selectedLanguageArray = languageCombobox.SelectedItem.ToString().Split('(');
            string selectedLanguage = selectedLanguageArray[1].Remove(selectedLanguageArray[1].Length - 1);

            WindowsRegisterManager windowsRegisterManager = new WindowsRegisterManager();
            Microsoft.Win32.RegistryKey key = windowsRegisterManager.OpenWindowsRegister(true);
            key.SetValue("selectedLanguage", selectedLanguage);
            windowsRegisterManager.CloseWindowsRegister(key);

            ChangeCurrentLanguage(selectedLanguage);
            ChangeAplicationLanguage();
            CheckLanguageComboboxSelection(selectedLanguage, languageCombobox);
        }

        /// <summary>
        /// Changes the compiler language to the one requested and allows access to the resources of that language
        /// Cambia el idioma del compilador por el solicitado y permite acceder a los recursos propios de ese idioma
        /// </summary>
        /// <param name="selectedLanguage">String, selected language</param>
        public void ChangeCurrentLanguage(string selectedLanguage)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo(selectedLanguage);
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo(selectedLanguage);
        }

        /// <summary>
        /// It reads the language that is being used from the windows registry, if there is none, it sets Spanish by default.
        /// If the application is opening then change the language of the application if it is not Spanish and if you enter
        /// settings then set the language that is in use in the dropdown
        /// 
        /// Lee el idioma que se está usando del registro del windows, si no hay ninguno establece el español por defecto.
        /// Si la aplicación se esta abriendo entonces cambia el idioma de la aplicación si no es español y si entras a
        /// ajustes entonces establece en el desplegable el idioma que está en uso 
        /// </summary>
        /// <param name="appLoading">boolean that indicates if the application is loading for the first time or if you 
        /// are entering the method after the entire application has loaded</param>
        /// <param name="languageCombobox">Combobox with all the languages</param>
        public void ReadSelectedLanguage(bool appLoading, ComboBox languageCombobox)
        {
            WindowsRegisterManager windowsRegisterManager = new WindowsRegisterManager();
            Microsoft.Win32.RegistryKey key = windowsRegisterManager.OpenWindowsRegister(true);

            string selectedLanguage = (string)key.GetValue("selectedLanguage");

            if (selectedLanguage == null || selectedLanguage.Equals(""))
            {
                selectedLanguage = "es-ES";
                key.SetValue("selectedLanguage", selectedLanguage);
            }

            if (languageCombobox != null && !appLoading)
            {
                if (CheckLanguageComboboxSelection(selectedLanguage, languageCombobox) != -1)
                {
                    CheckLanguageComboboxSelection(selectedLanguage, languageCombobox);
                }
                else
                {
                    languageCombobox.SelectedIndex = 0;
                }
            }
            else if (appLoading)
            {
                ChangeCurrentLanguage(selectedLanguage);

                // If the language is not spanish when the aplication starts then translate it
                if (!selectedLanguage.Equals("es-ES"))
                {
                    ChangeAplicationLanguage();
                }
            }

            windowsRegisterManager.CloseWindowsRegister(key);
        }

        /// <summary>
        /// Set the language currently being used in the language drop-down in the settings menu
        /// Establece en el desplegable de idiomas del menú de ajustes qué idioma se está usando actualmente
        /// </summary>
        /// <param name="selectedLanguage">Language currently being used</param>
        /// <param name="languageCombobox">Combobox with all the languages</param>
        /// <returns>Int, return -1 if the language was not found in the drop-down</returns>
        private int CheckLanguageComboboxSelection(string selectedLanguage, ComboBox languageCombobox)
        {
            for (int currentLanguageItem = 0; currentLanguageItem < languageCombobox.Items.Count; currentLanguageItem++)
            {
                if (languageCombobox.Items[currentLanguageItem].ToString().Contains(selectedLanguage))
                {
                    languageCombobox.SelectedIndex = currentLanguageItem;
                    return 0; // This 0 does nothing, but every route must return a number
                }
            }

            return -1;
        }

        /// <summary>
        /// It establishes in which form the language will be changed
        /// Establece en que formulario se va a cambiar el idioma
        /// </summary>
        /// <param name="clientLoginLarge">ClientLoginLarge class reference</param>
        /// <param name="clientAplicationLarge">ClientAplicationLarge class reference</param>
        /// <param name="clientLoginSmall">ClientLoginSmall class reference</param>
        /// <param name="clientAplicationSmall">ClientAplicationSmall class reference</param>
        public void SelectCurrentAplicationWindow(ClientLoginLarge clientLoginLarge, ClientAplicationLarge clientAplicationLarge,
            ClientLoginSmall clientLoginSmall, ClientAplicationSmall clientAplicationSmall)
        {
            this.clientLoginLarge = clientLoginLarge;
            this.clientLoginSmall = clientLoginSmall;
            this.clientAplicationLarge = clientAplicationLarge;
            this.clientAplicationSmall = clientAplicationSmall;
        }

        /// <summary>
        /// Calls methods that change the text of objects depending on the selected language
        /// Llama a los métodos que cambian el texto de los objetos dependiendo del idioma seleccionado
        /// </summary>
        private void ChangeAplicationLanguage()
        {
            if (clientLoginLarge != null)
            {
                clientLoginLarge.ChangeAplicationLanguage();
            }
            else if (clientLoginSmall != null)
            {
                clientLoginSmall.ChangeAplicationLanguage();
            }
            else if (clientAplicationLarge != null)
            {
                clientAplicationLarge.ChangeAplicationLanguage();
            }
            else if (clientAplicationSmall != null)
            {
                clientAplicationSmall.ChangeAplicationLanguage();
            }
        }
    }
    #endregion Language Manager

    #region Resolution Manager

    #endregion Resolution Manager
    public class ResolutionManager
    {
        /// <summary>
        /// Read from the windows register the resolution of the form and establish it the current form, also hide the form passed by param
        /// </summary>
        /// <param name="clientLogin">Boolean that indicates if the current form is the login form or the aplication form</param>
        public void LoadWindowResolution(bool clientLogin, ClientLoginLarge clientLoginLarge, ClientAplicationLarge clientAplicationLarge,
            ClientLoginSmall clientLoginSmall, ClientAplicationSmall clientAplicationSmall, string username)
        {
            WindowsRegisterManager windowsRegisterManager = new WindowsRegisterManager();
            Microsoft.Win32.RegistryKey key = windowsRegisterManager.OpenWindowsRegister(true);

            if (key.GetValue("SelectedResolution") == null || key.GetValue("SelectedResolution").Equals(""))
            {
                key.SetValue("SelectedResolution", "1280x720");
            }

            if (clientLoginSmall != null)
            {
                windowsRegisterManager.SaveWindowPosition(clientLoginSmall);
                clientLoginSmall.Hide();
            }
            else if (clientLoginLarge != null)
            {
                windowsRegisterManager.SaveWindowPosition(clientLoginLarge);
                clientLoginLarge.Hide();
            }
            else if (clientAplicationSmall != null)
            {
                windowsRegisterManager.SaveWindowPosition(clientAplicationSmall);
                clientAplicationSmall.Hide();
            }
            else if (clientAplicationLarge != null)
            {
                windowsRegisterManager.SaveWindowPosition(clientAplicationLarge);
                clientAplicationLarge.Hide();
            }

            if (key.GetValue("SelectedResolution").ToString().Contains("800x600"))
            {
                if (clientLogin)
                {
                    // Cargar ClientLoginSmall
                    ClientLoginSmall newClientLoginSmall = new ClientLoginSmall();
                    newClientLoginSmall.ReceiveClassInstance(newClientLoginSmall);
                    newClientLoginSmall.ShowDialog();
                }
                else
                {
                    // Cargar clientAplicationSmall
                    ClientAplicationSmall newclientAplicationSmall = new ClientAplicationSmall();
                    newclientAplicationSmall.ReceiveClassInstance(newclientAplicationSmall, username);
                    newclientAplicationSmall.ShowDialog();
                }
            }
            else
            {
                if (clientLogin)
                {
                    // Cargar ClientLoginLarge
                    if (clientLoginLarge != null) clientLoginLarge.Hide();
                    else if (clientLoginSmall != null) clientLoginSmall.Hide();
                    ClientLoginLarge newClientLoginLarge = new ClientLoginLarge();
                    newClientLoginLarge.ReceiveClassInstance(newClientLoginLarge);
                    newClientLoginLarge.ShowDialog();
                }
                else
                {
                    // Cargar clientAplicationLarge
                    ClientAplicationLarge newclientAplicationLarge = new ClientAplicationLarge();
                    newclientAplicationLarge.ReceiveClassInstance(newclientAplicationLarge, username);
                    newclientAplicationLarge.ShowDialog();
                }
            }

            windowsRegisterManager.CloseWindowsRegister(key);
        }

        /// <summary>
        /// Change the resolution stored in the windows registry and change the resolution of the application
        /// Cambia la resolución del almacenado en la entrada del registro de windows y cambia la resolución de la aplicación
        /// </summary>
        /// <param name="resolutionCombobox">Combobox with all the resolutions</param>
        /// <param name="clientLogin">Boolean that indicates if the current form is the login form or the aplication form</param>
        public void ResolutionCombobox_ResolutionChanged(ComboBox resolutionCombobox)
        {
            string[] selectedResolutionArray = resolutionCombobox.SelectedItem.ToString().Split(' ');

            WindowsRegisterManager windowsRegisterManager = new WindowsRegisterManager();
            Microsoft.Win32.RegistryKey key = windowsRegisterManager.OpenWindowsRegister(true);
            key.SetValue("SelectedResolution", selectedResolutionArray[0]);
            windowsRegisterManager.CloseWindowsRegister(key);
        }

        /// <summary>
        /// Set the current resolution in the windows registry the selected item in the resolution combobox, 
        /// if there is no selected resolution set the 1280 x 720 pixels resolution
        /// </summary>
        /// <param name="resolutionCombobox"></param>
        public void ReadSelectedResolution(ComboBox resolutionCombobox)
        {
            WindowsRegisterManager windowsRegisterManager = new WindowsRegisterManager();
            Microsoft.Win32.RegistryKey key = windowsRegisterManager.OpenWindowsRegister(true);

            string selectedResolution = "";
            if (key.GetValue("SelectedResolution") != null)
            {
                selectedResolution = key.GetValue("SelectedResolution").ToString();
            }

            if (selectedResolution.Equals(""))
            {
                selectedResolution = "1280x720";
                key.SetValue("SelectedResolution", selectedResolution);
            }

            if (CheckResolutionComboboxSelection(selectedResolution, resolutionCombobox) != -1)
            {
                CheckResolutionComboboxSelection(selectedResolution, resolutionCombobox);
            }
            else
            {
                resolutionCombobox.SelectedIndex = 0;
            }

            windowsRegisterManager.CloseWindowsRegister(key);
        }

        /// <summary>
        /// Set the resolution currently being used in the resolution drop-down in the settings menu
        /// Establece en el desplegable de resoluciones del menú de ajustes qué resolución se está usando actualmente
        /// </summary>
        /// <param name="selectedLanguage">Resolution currently being used</param>
        /// <param name="resolutionCombobox">Combobox with all the resolutions</param>
        /// <returns>Int, return -1 if the resolution was not found in the drop-down</returns>
        private int CheckResolutionComboboxSelection(string selectedLanguage, ComboBox resolutionCombobox)
        {
            for (int currentLanguageItem = 0; currentLanguageItem < resolutionCombobox.Items.Count; currentLanguageItem++)
            {
                if (resolutionCombobox.Items[currentLanguageItem].ToString().Contains(selectedLanguage))
                {
                    resolutionCombobox.SelectedIndex = currentLanguageItem;
                    return 0; // This 0 does nothing, but every route must return a number
                }
            }

            return -1;
        }
    }
    #endregion Language / Resolution / Combobox Manager

    #region Multiple Resources
    /// <summary>
    /// Class in charge of managing the multiple resolutions of the application
    /// </summary>
    static class MultipleResources
    {
        static ToolTip toolTip = new ToolTip();

        /// <summary>
        /// Shows a tooltip attached to a certain panel and with a custom message
        /// </summary>
        /// <param name="currentPanel">Panel to which the tooltip will be attached</param>
        /// <param name="toolTipText">String, custom text</param>
        public static void ShowToolTip(Panel currentPanel, string toolTipText)
        {
            toolTip.UseFading = true;
            toolTip.UseAnimation = true;
            toolTip.IsBalloon = true;
            toolTip.SetToolTip(currentPanel, toolTipText);
        }

        /// <summary>
        /// Hide the tooltip attached to a certain panel
        /// </summary>
        /// <param name="currentPanel">Panel to which the tooltip will be attached</param>
        public static void HideToolTip(Panel currentPanel)
        {
            toolTip.Hide(currentPanel);
        }

        /// <summary>
        /// Calculate the center of the given container
        /// </summary>
        /// <param name="container">Control, desired container</param>
        /// <param name="controlToCenter">Control to be centered</param>
        /// <param name="locationDifference">Integer, bias difference</param>
        public static void CalculateCenterLocation(Control container, Control controlToCenter, int locationDifference)
        {
            int centerYLocation = (container.Height / 2 - controlToCenter.Size.Height / 2);

            controlToCenter.Location = new Point(
                container.Width / 2 - controlToCenter.Size.Width / 2,
                centerYLocation / 2 + locationDifference);
        }

        /// <summary>
        /// Calculate the half size of the given container
        /// </summary>
        /// <param name="container">Control, desired container</param>
        /// <param name="controlToHalfSize">Control to be resized</param>
        /// <param name="widthSize">Decimal, bias difference</param>
        /// <param name="heightSize">Decimal, bias difference</param>
        public static void CalculateHalfSize(Control container, Control controlToHalfSize, decimal widthSize, decimal heightSize)
        {
            decimal width = container.Size.Width * widthSize;
            int truncatedWidth = Convert.ToInt32(Math.Truncate(width));

            decimal height = container.Size.Height * heightSize;
            int truncatedHeight = Convert.ToInt32(Math.Truncate(height));

            controlToHalfSize.Size = new Size(
                truncatedWidth,
                truncatedHeight);
        }

        /// <summary>
        /// Create a custom label with the given attributes
        /// </summary>
        /// <param name="controlName">String, set the control name</param>
        /// <param name="autosize">Boolean, set if the label can autosize</param>
        /// <param name="width">Integer, label width</param>
        /// <param name="height">Integer, label height</param>
        /// <param name="xPosition">Integer, label x location</param>
        /// <param name="yPosition">Integer, label y location</param>
        /// <param name="textSize">Integer, label text size</param>
        /// <param name="textAlignment">ContentAlignment, label text alignment</param>
        /// <returns></returns>
        public static Label CreateGenericLabel(string controlName, bool autosize, int width, int height, int xPosition,
            int yPosition, int textSize, ContentAlignment textAlignment)
        {
            Label genericLabel = new Label();
            genericLabel.Name = controlName;
            genericLabel.AutoSize = autosize;
            genericLabel.Size = new Size(width, height);
            genericLabel.Location = new Point(xPosition, yPosition);
            genericLabel.TextAlign = textAlignment;
            genericLabel.Font = new Font("Oxygen", textSize, FontStyle.Regular);

            return genericLabel;
        }

        /// <summary>
        /// Restart the application and go back to logging menu. Get the process id, start the same application from the start and kill the old process
        /// </summary>
        /// <param name="pid">Integer, process id to be killed</param>
        /// <param name="applicationName">String, application name to be started</param>
        public static void RestartApp(int pid, string applicationName)
        {
            // Wait for the process to terminate
            Process process = null;
            try
            {
                process = Process.GetProcessById(pid);
                process.WaitForExit(1000);
            }
            catch (ArgumentException ex)
            {
                // ArgumentException to indicate that the 
                // process doesn't exist?   LAME!!
            }

            Process.Start(applicationName, "");
            process.Kill();
        }
    }
    #endregion Multiple Resources
}