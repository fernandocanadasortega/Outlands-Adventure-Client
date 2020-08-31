using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Net.Mail;
using System.Threading;
using System.Security.Cryptography;
using System.Data.Linq;

namespace Outlands_Adventure_Launcher
{
    #region Password Strenght
    static class PasswordStrength
    {
        public enum PasswordScore
        {
            Empty = 0,
            VeryWeak = 1,
            Weak = 2,
            Medium = 3,
            Strong = 4,
            VeryStrong = 5
        }

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
                    NewPasswordStrengthLabel.Text = "Muy débil";
                    break;

                case PasswordScore.Weak:
                    NewPasswordStrengthProgressBar.Value = 40;
                    NewPasswordStrengthLabel.ForeColor = Color.Red;
                    NewPasswordStrengthLabel.Text = "Débil";
                    break;

                case PasswordScore.Medium:
                    NewPasswordStrengthProgressBar.Value = 60;
                    NewPasswordStrengthLabel.ForeColor = Color.DarkGoldenrod;
                    NewPasswordStrengthLabel.Text = "Mediana";
                    break;

                case PasswordScore.Strong:
                    NewPasswordStrengthProgressBar.Value = 80;
                    NewPasswordStrengthLabel.ForeColor = Color.Green;
                    NewPasswordStrengthLabel.Text = "Fuerte";
                    break;

                case PasswordScore.VeryStrong:
                    NewPasswordStrengthProgressBar.Value = 100;
                    NewPasswordStrengthLabel.ForeColor = Color.Green;
                    NewPasswordStrengthLabel.Text = "Muy fuerte";
                    break;
            }
        }
    }
    #endregion Password Strenght

    #region Send Email
    static class SendEmail
    {
        public static bool SendNewEmail(TextBox emailTextBox, string emailSubject, string emailBody, string emailBodyData)
        {
            try
            {
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress("outlandsadventure@gmail.com");
                mail.To.Add(emailTextBox.Text);
                mail.Subject = emailSubject;

                mail.IsBodyHtml = true;
                string htmlBody = "<p><h2>" + emailBody + "</h2></p> <br/>" +
                    "<p><h1>" + emailBodyData + "</h1></p>";
                mail.Body = htmlBody;

                smtpServer.Port = 587;
                smtpServer.UseDefaultCredentials = false;
                smtpServer.Credentials = new System.Net.NetworkCredential("outlandsadventure@gmail.com", "Outlands_Client_Password");
                smtpServer.EnableSsl = true;
                smtpServer.Send(mail);

                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
    #endregion Send Email

    #region Confimation Code
    static class CreateConfirmationCode
    {
        static readonly int codeLenght = 8;

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

        private static int GiveMeANumber(Random randomNumber)
        {
            List<int> exclude = new List<int>() { 58, 59, 60, 61, 62, 63, 64 };
            var range = Enumerable.Range(1, 100).Where(i => !exclude.Contains(i));

            int index = randomNumber.Next(48, 90 - exclude.Count);
            return range.ElementAt(index);
        }
    }
    #endregion Confimation Code

    #region Resume Manager
    static class Hash_SHA2
    {
        private static HashAlgorithm hashResume;
        private static Binary binaryOriginalHash;

        public static void InitialiceVariables(string confirmationCode)
        {
            hashResume = new SHA256Managed();
            byte[] originalHash = Hash_SHA2.CreateResume(confirmationCode);
            binaryOriginalHash = new Binary(originalHash);
        }

        public static byte[] CreateResume(string confirmationCode)
        {
             return hashResume.ComputeHash(Encoding.UTF8.GetBytes(confirmationCode));
        }

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

        public void LoadWindowPosition(Form WindowApp)
        {
            Microsoft.Win32.RegistryKey key = OpenWindowsRegister(false);

            if (key.GetValue("XPosition") != null && key.GetValue("YPosition") != null)
            {
                WindowApp.Location = new Point((int)key.GetValue("XPosition"), (int)key.GetValue("YPosition"));
            }

            CloseWindowsRegister(key);
        }

        public void SaveWindowPosition(Form WindowApp)
        {
            Microsoft.Win32.RegistryKey key = OpenWindowsRegister(true);

            key.SetValue("XPosition", WindowApp.Location.X);
            key.SetValue("YPosition", WindowApp.Location.Y);

            CloseWindowsRegister(key);
        }
    }
    #endregion Windows register manager

    #region Language / LanguageCombobox Manager
    public class ComboboxManager
    {
        // Allow Combo Box to center aligned
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

        public void Combobox_DropDownClosed(Panel configurationPanel)
        {
            configurationPanel.Focus();
        }
    }

    public class LanguageManager
    {
        private ClientLogin clientLogin;
        private ClientAplication clientAplication;

        /// <summary>
        /// Cambia el idioma del almacenado en la entrada del registro de windows y cambia el idioma de la aplicación
        /// </summary>
        /// <param name="sender">Objeto que recibe los eventos</param>
        /// <param name="e">Eventos que le ocurren al objeto</param>
        public void LanguageCombobox_LanguageChanged(ComboBox languageCombobox)
        {
            string[] selectedLanguageArray = languageCombobox.SelectedItem.ToString().Split('(');
            string selectedLanguage = selectedLanguageArray[1].Remove(selectedLanguageArray[1].Length - 1);

            ChangeCurrentLanguage(selectedLanguage);
            ChangeAplicationLanguage();
            CheckLanguageComboboxSelection(selectedLanguage, languageCombobox);

            WindowsRegisterManager windowsRegisterManager = new WindowsRegisterManager();
            Microsoft.Win32.RegistryKey key = windowsRegisterManager.OpenWindowsRegister(true);
            key.SetValue("selectedLanguage", selectedLanguage);
            windowsRegisterManager.CloseWindowsRegister(key);
        }

        /// <summary>
        /// Cambia el idioma del complilador por el solicitado y permite acceder a los recursos propios de ese idioma
        /// </summary>
        /// <param name="selectedLanguage"></param>
        private void ChangeCurrentLanguage(string selectedLanguage)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo(selectedLanguage);
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo(selectedLanguage);
        }

        /// <summary>
        /// Lee el idioma que se está usando del registro del windows, si no hay ninguno establece el español por defecto.
        /// Si la aplicación se esta abriendo entonces cambia el idioma de la aplicación si no es español y si entras a
        /// ajustes entonces establece en el desplegable el idioma que está en uso 
        /// </summary>
        /// <param name="appLoading">booleano que indica si la aplicación está cargando por primera vez o si estás entrando
        /// al método una vez que toda la aplicación ya ha cargado</param>
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
        /// Establece en el desplegable del idiomas en el menú de ajustes qué idioma se está usando actualmente
        /// </summary>
        /// <param name="selectedLanguage">Idioma que se está usando actualmente</param>
        /// <returns>Int, devuelve -1 si no se ha encontrado el idioma en el desplegable</returns>
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
        /// Establece en que formulario se va a cambiar el idioma
        /// </summary>
        /// <param name="clientLogin">Referencia de la clase ClientLogin</param>
        /// <param name="clientAplication">Referencia de la clase ClientAplication</param>
        public void SelectCurrentAplicationWindow(ClientLogin clientLogin, ClientAplication clientAplication)
        {
            this.clientLogin = clientLogin;
            this.clientAplication = clientAplication;
        }

        /// <summary>
        /// Llama a los métodos que cambian el texto de los objetos dependiendo del idioma seleccionado
        /// </summary>
        private void ChangeAplicationLanguage()
        {
            if (clientLogin != null)
            {
                clientLogin.ChangeAplicationLanguage();
            }
            else if (clientAplication != null)
            {
                clientAplication.ChangeAplicationLanguage();
            }
        }
    }
    #endregion Language Manager
}
