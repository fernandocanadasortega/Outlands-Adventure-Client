using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Outlands_Adventure_Launcher
{
    static class ModifyProgressBarColor
    {
        // Do not work well, Do not use it
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);
        public static void SetState(this ProgressBar pBar, int state)
        {
            SendMessage(pBar.Handle, 1040, (IntPtr)state, IntPtr.Zero);
        }
    }

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
}
