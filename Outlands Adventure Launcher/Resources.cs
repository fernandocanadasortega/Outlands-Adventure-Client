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
                //MessageBox.Show(ex.Message);
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

            // La clase binary es muy eficiente a la hora de hacer equals de dos resumenes hash para ver si son iguales (https://stackoverflow.com/questions/18472867/checking-equality-for-two-byte-arrays/18472958)
            Binary binaryConfirmationHash = new Binary(confirmationHash);

            return binaryOriginalHash.Equals(binaryConfirmationHash);
        }
    }
}
