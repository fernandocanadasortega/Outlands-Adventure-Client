﻿using Outlands_Adventure_Launcher.Properties;
using System;
using System.Drawing;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Outlands_Adventure_Launcher
{
    public partial class LauncherLogin : Form
    {
        private string targetPanel;

        private bool loginAvaible;
        private bool registerAvaible;

        private bool loginProblemsAvaible;
        private bool usernameLost;
        private bool passwordLost;

        private Panel currentShowPasswordButton;
        private TextBox currentPasswordTextbox;
        private bool passwordVisible;
        private bool confirmPasswordVisible;

        public LauncherLogin()
        {
            targetPanel = "login";

            loginAvaible = false;
            registerAvaible = false;
            loginProblemsAvaible = false;

            passwordVisible = false;
            confirmPasswordVisible = false;

            InitializeComponent();

            ImageGradient.BackColor = Color.FromArgb(190, 0, 0, 0);

            ConfigurationPanel.BackColor = Color.FromArgb(255, 0, 0, 0);
        }

        #region Login Interface
        // Manage all items in the login panel

        #region UserName Textbox Gain Focus
        // These methods make UserName Textbox gain the focus
        private void UserNamePanel_Click(object sender, EventArgs e)
        {
            // Cuando haces click en el panel contenedor
            UserNameTextbox.Focus();
        }

        private void UserNameLabel_Click(object sender, EventArgs e)
        {
            // Cuando haces click en el label
            UserNameTextbox.Focus();
        }

        // These methods manage the focus gain
        private void UserNameTextBox_Enter(object sender, EventArgs e)
        {
            // Cuando el textbox coge el foco
            UserNameGain();
        }

        private void UserNameGain()
        {
            if (UserNameTextbox.Text.Length == 0)
            {
                UserNameLabel.Font = new Font("Perpetua Titling MT", 6, FontStyle.Bold);
                UserNameLabel.Location = new Point(0, 2);
            }
        }
        #endregion

        #region UserName Textbox Lose Focus
        // These methods manage the focus lose
        private void UserNameTextBox_Leave(object sender, EventArgs e)
        {
            // Cuando el textbox pierde el foco
            UserNameFocusLost();
        }

        private void UserNameFocusLost()
        {
            if (UserNameTextbox.Text.Length == 0)
            {
                UserNameLabel.Font = new Font("Oxygen", 10);
                UserNameLabel.Location = new Point(14, 10);
            }
        }
        #endregion

        #region Password Textbox Gain Focus
        // These methods make Password Textbox gain the focus
        private void PasswordPanel_Enter(object sender, EventArgs e)
        {
            // Cuando haces click en el panel contenedor
            PasswordTextbox.Focus();
        }

        private void PasswordLabel_Click(object sender, EventArgs e)
        {
            // Cuando haces click en el label
            PasswordTextbox.Focus();
        }

        private void PasswordTextbox_Enter(object sender, EventArgs e)
        {
            // Cuando el textbox coge el foco
            PasswordGain();
        }

        private void PasswordGain()
        {
            if (PasswordTextbox.Text.Length == 0)
            {
                PasswordLabel.Font = new Font("Perpetua Titling MT", 6, FontStyle.Bold);
                PasswordLabel.Location = new Point(0, 2);
            }

            ShowLoginPassword.Visible = true;
            currentShowPasswordButton = ShowLoginPassword;
            currentPasswordTextbox = PasswordTextbox;

            if (Control.IsKeyLocked(Keys.CapsLock))
            {
                LoginMayusLock.Visible = true;
                PasswordTextbox.Size = new Size(165, 22);
            }
            else
            {
                LoginMayusLock.Visible = false;
                PasswordTextbox.Size = new Size(195, 22);
            }
        }
        #endregion

        #region Password Textbox Lose Focus
        // These methods manage the focus lose
        private void PasswordTextbox_Leave(object sender, EventArgs e)
        {
            // Cuando el textbox pierde el foco
            PasswordFocusLost();
        }

        private void PasswordFocusLost()
        {
            if (PasswordTextbox.Text.Length == 0)
            {
                PasswordLabel.Font = new Font("Oxygen", 10);
                PasswordLabel.Location = new Point(14, 10);
            }


            ShowLoginPassword.Visible = false;
            LoginMayusLock.Visible = false;
        }
        #endregion

        #region Write Login Textboxs
        // These methods check username and password, if the is more that 4 characters in each textbox then enable the login button
        private void PasswordTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.CapsLock)
            {
                if (Control.IsKeyLocked(Keys.CapsLock))
                {
                    LoginMayusLock.Visible = true;
                    PasswordTextbox.Size = new Size(165, 22);
                }
                else if (!Control.IsKeyLocked(Keys.CapsLock))
                {
                    LoginMayusLock.Visible = false;
                    PasswordTextbox.Size = new Size(195, 22);
                }
            }
        }

        private void UserNameTextbox_TextChanged(object sender, EventArgs e)
        {
            CheckLoginTextboxs();
        }

        private void PasswordTextbox_TextChanged(object sender, EventArgs e)
        {
            CheckLoginTextboxs();
        }

        private void CheckLoginTextboxs()
        {
            if (UserNameTextbox.Text.Length >= 4 && PasswordTextbox.Text.Length >= 4)
            {
                LoginButton.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.loginSuccessful;
                LoginButton.Cursor = System.Windows.Forms.Cursors.Hand;
                loginAvaible = true;
            }
            else
            {
                LoginButton.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.loginUnavaible;
                LoginButton.Cursor = System.Windows.Forms.Cursors.Default;
                loginAvaible = false;
            }
        }
        #endregion

        #region Keep My Session Open
        // This method manage rememberMe checkbox state
        private void RememberMe_CheckedChanged(object sender, EventArgs e)
        {
            if (RememberMe.Checked)
            {
                RememberMe.Font = new Font("Oxygen", 9, FontStyle.Bold);
            }
            else
            {
                RememberMe.Font = new Font("Oxygen", 9, FontStyle.Regular);
            }
        }
        #endregion

        #region Login in
        // This method manage login button when you click on it
        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (loginAvaible)
            {
                LoginButton.Focus();
                CheckLoginCredentials();

                if (loginAvaible)
                {
                    MessageBox.Show("Logeado :P");
                }
            }
            else
            {
                LoginButton.Focus();
            }
        }

        private void CheckLoginCredentials()
        {
            //Cambiar por datos sacados de firebase
            if (UserNameTextbox.Text.Equals("Napo") && PasswordTextbox.Text.Equals("Napo"))
            {
                loginAvaible = true;
                WrongCredentials.Visible = false;
            }
            else
            {
                loginAvaible = false;
                WrongCredentials.Visible = true;
            }
        }
        #endregion

        #endregion Login Interface

        #region Register Interface
        //Manage all items in the register panel

        #region New Email Textbox Gain Focus
        // These methods make New Email Textbox gain the focus
        private void NewEmailPanel_Click(object sender, EventArgs e)
        {
            // Cuando haces click en el panel contenedor
            NewEmailTextbox.Focus();
        }

        private void NewEmailLabel_Click(object sender, EventArgs e)
        {
            // Cuando haces click en el label
            NewEmailTextbox.Focus();
        }

        private void NewEmailTextbox_Enter(object sender, EventArgs e)
        {
            // Cuando el textbox coge el foco
            NewEmailGain();
        }

        private void NewEmailGain()
        {
            if (NewEmailTextbox.Text.Length == 0)
            {
                NewEmailLabel.Font = new Font("Perpetua Titling MT", 6, FontStyle.Bold);
                NewEmailLabel.Location = new Point(0, 2);
            }
        }
        #endregion

        #region New Email Textbox Lose Focus
        // These methods manage the focus lose, and check the content of the textbox
        private void NewEmailTextbox_Leave(object sender, EventArgs e)
        {
            // Cuando el textbox pierde el foco
            NewEmailFocusLost();
        }

        private void NewEmailFocusLost()
        {
            if (NewEmailTextbox.Text.Length == 0)
            {
                NewEmailLabel.Font = new Font("Oxygen", 10);
                NewEmailLabel.Location = new Point(14, 10);

                NewEmailErrorLabel.Visible = false;
            }
            else
            {
                if (NewEmailTextbox.Text.Length < 4)
                {
                    NewEmailErrorLabel.Visible = true;
                    NewEmailErrorLabel.Text = "Debe contener cuatro letras como mínimo";
                }
                else if (!IsEmailValid(NewEmailTextbox.Text))
                {
                    NewEmailErrorLabel.Visible = true;
                    NewEmailErrorLabel.Text = "Introduce una dirección de correo válida";
                }
                else
                {
                    NewEmailErrorLabel.Visible = false;
                }
            }

            CheckRegisterTextboxs();
        }
        #endregion

        #region New UserName Textbox Gain Focus
        // These methods make New UserName Textbox gain the focus
        private void NewUserNamePanel_Click(object sender, EventArgs e)
        {
            // Cuando haces click en el panel contenedor
            NewUserNameTextbox.Focus();
        }

        private void NewUserNameLabel_Click(object sender, EventArgs e)
        {
            // Cuando haces click en el label
            NewUserNameTextbox.Focus();
        }

        private void NewUserNameTextbox_Enter(object sender, EventArgs e)
        {
            // Cuando el textbox coge el foco
            NewUserNameGain();
        }

        private void NewUserNameGain()
        {
            if (NewUserNameTextbox.Text.Length == 0)
            {
                NewUserNameLabel.Font = new Font("Perpetua Titling MT", 6, FontStyle.Bold);
                NewUserNameLabel.Location = new Point(0, 2);
            }
        }
        #endregion

        #region New UserName Textbox Lose Focus
        // These methods manage the focus lose
        private void NewUserNameTextbox_Leave(object sender, EventArgs e)
        {
            // Cuando el textbox pierde el foco
            NewUserNameFocusLost();
        }

        private void NewUserNameFocusLost()
        {
            if (NewUserNameTextbox.Text.Length == 0)
            {
                NewUserNameLabel.Font = new Font("Oxygen", 10);
                NewUserNameLabel.Location = new Point(14, 10);

                NewUserNameErrorLabel.Visible = false;
            }
            else
            {
                if (NewUserNameTextbox.Text.Length < 4)
                {
                    NewUserNameErrorLabel.Visible = true;
                    NewUserNameErrorLabel.Text = "Debe contener cuatro letras como mínimo";
                }
                else
                {
                    NewUserNameErrorLabel.Visible = false;
                }
            }

            CheckRegisterTextboxs();
        }
        #endregion

        #region New Password Textbox Gain Focus
        // These methods make New Password Textbox gain the focus
        private void NewPasswordPanel_Click(object sender, EventArgs e)
        {
            // Cuando haces click en el panel contenedor
            NewPasswordTextbox.Focus();
        }

        private void NewPasswordLabel_Click(object sender, EventArgs e)
        {
            // Cuando haces click en el label
            NewPasswordTextbox.Focus();
        }

        private void NewPasswordTextbox_Enter(object sender, EventArgs e)
        {
            // Cuando el textbox coge el foco
            NewPasswordGain();
        }

        /* Pone el label que indica Constraseña arriba con letra pequeña, te enseña el ojo para mostrar la contraseña
         * y controla si las mayúsculas están activadas cuando el textbox coje el foco
        */
        private void NewPasswordGain()
        {
            if (NewPasswordTextbox.Text.Length == 0)
            {
                NewPasswordLabel.Font = new Font("Perpetua Titling MT", 6, FontStyle.Bold);
                NewPasswordLabel.Location = new Point(0, 2);
            }

            ShowNewPassword.Visible = true;
            currentShowPasswordButton = ShowNewPassword;
            currentPasswordTextbox = NewPasswordTextbox;

            if (Control.IsKeyLocked(Keys.CapsLock))
            {
                NewPasswordMayusLock.Visible = true;
                NewPasswordTextbox.Size = new Size(165, 22);
            }
            else
            {
                NewPasswordMayusLock.Visible = false;
                NewPasswordTextbox.Size = new Size(195, 22);
            }
        }
        #endregion

        #region New Password Textbox Lose Focus
        // These methods manage the focus lose
        private void NewPasswordTextbox_Leave(object sender, EventArgs e)
        {
            // Cuando el textbox pierde el foco
            NewPasswordFocusLost();
        }

        private void NewPasswordFocusLost()
        {
            // Pone el label que indica Constraseña otra vez con letra grande en el medio y comprueba errores de la caja de texto
            if (NewPasswordTextbox.Text.Length == 0)
            {
                NewPasswordLabel.Font = new Font("Oxygen", 10);
                NewPasswordLabel.Location = new Point(14, 10);

                NewPasswordStrengthProgressBar.Visible = false;
                NewPasswordStrengthLabel.Visible = false;
                NewPasswordErrorLabel.Visible = false;
            }
            else
            {
                if (NewPasswordTextbox.Text.Length < 4)
                {
                    NewPasswordStrengthProgressBar.Visible = false;
                    NewPasswordStrengthLabel.Visible = false;

                    NewPasswordErrorLabel.Visible = true;
                    NewPasswordErrorLabel.Text = "Debe contener cuatro letras como mínimo";
                }
                else
                {
                    NewPasswordErrorLabel.Visible = false;
                }
            }

            CheckRegisterTextboxs();
            ShowNewPassword.Visible = false;
            NewPasswordMayusLock.Visible = false;
        }
        #endregion

        #region Confirm New Password Textbox Gain Focus
        // These methods make Confirm New Password Textbox gain the focus
        private void ConfirmNewPasswordPanel_Click(object sender, EventArgs e)
        {
            // Cuando haces click en el panel contenedor
            ConfirmNewPasswordTextbox.Focus();
        }

        private void ConfirmNewPasswordLabel_Click(object sender, EventArgs e)
        {
            // Cuando haces click en el label
            ConfirmNewPasswordTextbox.Focus();
        }

        private void ConfirmNewPasswordTextbox_Enter(object sender, EventArgs e)
        {
            // Cuando el textbox coge el foco
            ConfirmNewPasswordGain();
        }

        private void ConfirmNewPasswordGain()
        {
            if (ConfirmNewPasswordTextbox.Text.Length == 0)
            {
                ConfirmNewPasswordLabel.Font = new Font("Perpetua Titling MT", 6, FontStyle.Bold);
                ConfirmNewPasswordLabel.Location = new Point(0, 2);
            }

            ShowConfirmNewPassword.Visible = true;
            currentShowPasswordButton = ShowConfirmNewPassword;
            currentPasswordTextbox = ConfirmNewPasswordTextbox;

            if (Control.IsKeyLocked(Keys.CapsLock))
            {
                ConfirmNewPasswordMayusLock.Visible = true;
                ConfirmNewPasswordTextbox.Size = new Size(165, 22);
            }
            else
            {
                ConfirmNewPasswordMayusLock.Visible = false;
                ConfirmNewPasswordTextbox.Size = new Size(195, 22);
            }
        }
        #endregion

        #region Confirm New Password Textbox Lose Focus
        // These methods manage the focus lose
        private void ConfirmNewPasswordTextbox_Leave(object sender, EventArgs e)
        {
            // Cuando el textbox pierde el foco
            ConfirmNewPasswordFocusLost();
        }

        private void ConfirmNewPasswordFocusLost()
        {
            if (ConfirmNewPasswordTextbox.Text.Length == 0)
            {
                ConfirmNewPasswordLabel.Font = new Font("Oxygen", 10);
                ConfirmNewPasswordLabel.Location = new Point(14, 10);

                ConfirmNewPasswordErrorLabel.Visible = false;
            }
            else
            {
                if (!NewPasswordTextbox.Text.Equals(ConfirmNewPasswordTextbox.Text))
                {
                    ConfirmNewPasswordErrorLabel.Visible = true;
                    ConfirmNewPasswordErrorLabel.Text = "Las contraseñas no coinciden";
                }
                else if (ConfirmNewPasswordTextbox.Text.Length < 4)
                {
                    ConfirmNewPasswordErrorLabel.Visible = true;
                    ConfirmNewPasswordErrorLabel.Text = "Debe contener cuatro letras como mínimo";
                }
                else
                {
                    ConfirmNewPasswordErrorLabel.Visible = false;
                }
            }

            CheckRegisterTextboxs();
            ShowConfirmNewPassword.Visible = false;
            ConfirmNewPasswordMayusLock.Visible = false;
        }
        #endregion

        #region Write Register Textboxs
        // These methods check email, username and passwords, if there are any errors in any textbox then unable the register button
        // In password textboxes check if the key pressed is Lock Capital to show a warning to the user
        private void NewEmailTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                RegisterPanel.Focus();
            }
        }

        private void NewUserNameTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                RegisterPanel.Focus();
            }
        }

        private void NewPasswordTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.CapsLock)
            {
                if (Control.IsKeyLocked(Keys.CapsLock))
                {
                    NewPasswordMayusLock.Visible = true;
                    NewPasswordTextbox.Size = new Size(165, 22);
                }
                else if (!Control.IsKeyLocked(Keys.CapsLock))
                {
                    NewPasswordMayusLock.Visible = false;
                    NewPasswordTextbox.Size = new Size(195, 22);
                }
            }

            if (e.KeyCode == Keys.Enter)
            {
                RegisterPanel.Focus();
            }
        }

        private void ConfirmNewPasswordTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.CapsLock)
            {
                if (Control.IsKeyLocked(Keys.CapsLock))
                {
                    ConfirmNewPasswordMayusLock.Visible = true;
                    ConfirmNewPasswordTextbox.Size = new Size(165, 22);
                }
                else if (!Control.IsKeyLocked(Keys.CapsLock))
                {
                    ConfirmNewPasswordMayusLock.Visible = false;
                    ConfirmNewPasswordTextbox.Size = new Size(195, 22);
                }
            }

            if (e.KeyCode == Keys.Enter)
            {
                RegisterPanel.Focus();
            }
        }

        private void NewPasswordTextbox_TextChanged(object sender, EventArgs e)
        {
            NewPasswordErrorLabel.Visible = false;
            NewPasswordStrengthProgressBar.Visible = true;
            NewPasswordStrengthLabel.Visible = true;

            PasswordStrength.CheckPasswordStrength(NewPasswordTextbox, NewPasswordStrengthProgressBar, NewPasswordStrengthLabel);
        }

        // Check the textboxes contents
        private void CheckRegisterTextboxs()
        {
            // if there is no error label showing
            if (!NewEmailErrorLabel.Visible && !NewUserNameErrorLabel.Visible && !NewPasswordErrorLabel.Visible &&
                !ConfirmNewPasswordErrorLabel.Visible)
            {
                // If all textboxes has at least 4 caracters then enable the register button
                if (NewEmailTextbox.Text.Length > 0 && NewUserNameTextbox.Text.Length > 0 && NewPasswordTextbox.Text.Length > 0 &&
                    ConfirmNewPasswordTextbox.Text.Length > 0)
                {
                    RegisterButton.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.loginSuccessful;
                    RegisterButton.Cursor = System.Windows.Forms.Cursors.Hand;
                    registerAvaible = true;
                }
                else
                {
                    RegisterButton.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.loginUnavaible;
                    RegisterButton.Cursor = System.Windows.Forms.Cursors.Default;
                    registerAvaible = false;
                }
            }
            // If there are any error label showing then disable the register button
            else
            {
                RegisterButton.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.loginUnavaible;
                RegisterButton.Cursor = System.Windows.Forms.Cursors.Default;
                registerAvaible = false;
            }
        }
        #endregion

        #region Register in
        // Check the textboxes content when you put the mouse over the button
        private void RegisterButton_MouseEnter(object sender, EventArgs e)
        {
            RegisterButton.Focus();
            CheckRegisterTextboxs();
        }

        // This method manage login button when you click on it
        private void RegisterButton_Click(object sender, EventArgs e)
        {
            if (registerAvaible)
            {
                RegisterButton.Focus();
                CheckRegisterCredentials();

                if (registerAvaible)
                {
                    EventText.Location = new Point(40, 5);
                    EventPasswordCode.Visible = true;
                    EventSendButton.Visible = true;
                    EventExitButton.Location = new Point(305, EventSendButton.Location.Y);

                    EventText.Text = "Hemos mandado un código de confirmación a tu correo electrónico, dirígete a tu correo e introduce el código para confirmar tu cuenta";

                    string confirmationCode = CreateConfirmationCode.CreateCode();
                    Hash_SHA2.InitialiceVariables(confirmationCode);
                    SendEmail.SendNewEmail(ResetCredentialsEmailText, "Creación de una nueva cuenta del cliente de" +
                           " Outlands Adventure", "Su código de confimación es", confirmationCode);

                    targetPanel = "login";

                    ShowImageGradient();
                    EventsPanel.Visible = true;
                    ResetRegisterPanelValues();
                }
            }
            else
            {
                RegisterButton.Focus();
            }
        }

        private void CheckRegisterCredentials()
        {
            //Cambiar por datos sacados de firebase
            if (NewEmailTextbox.Text.Equals("thenapo212@gmail.com"))
            {
                NewEmailErrorLabel.Text = "Dirección de correo ya registrada";
                NewEmailErrorLabel.Visible = true;
                registerAvaible = false;
                CheckRegisterTextboxs();
            }

            if (NewUserNameTextbox.Text.Equals("Napo"))
            {
                NewUserNameErrorLabel.Text = "Nombre de usuario no disponible";
                NewUserNameErrorLabel.Visible = true;
                registerAvaible = false;
                CheckRegisterTextboxs();
            }
        }
        #endregion

        #endregion Register Interface

        #region Login Problems
        // Manage all items in login problems panel

        #region Forgotten Username Button
        private void ForgottenUsernameButton_MouseEnter(object sender, EventArgs e)
        {
            ForgottenUsernameButton.BackColor = Color.FromArgb(25, 0, 203, 255);
        }

        private void ForgottenUsernameButton_MouseLeave(object sender, EventArgs e)
        {
            ForgottenUsernameButton.BackColor = Color.FromArgb(0, 0, 203, 255);
        }

        private void ForgottenUsernameHeader_Click(object sender, EventArgs e)
        {
            SetForgottenUsernameRecover();
        }

        private void ForgottenUsernameButton_Click(object sender, EventArgs e)
        {
            SetForgottenUsernameRecover();
        }

        private void SetForgottenUsernameRecover()
        {
            usernameLost = true;
            passwordLost = false;

            ResetCredentialsHeader.Text = "¿Necesitas ayuda para recordar tu nombre de usuario? \n" +
                "Puedes solicitar que te mandemos un recordatorio de tu usuario a tu correo electrónico";

            if (!ResetCredentialsHeader.Visible)
            {
                ResetCredentialsHeader.Visible = true;
                ResetCredentialsEmailPanel.Visible = true;
                ResetCredentialsButton.Visible = true;
            }

            ResetCredentialsEmailText.Text = "";
            ForgottenUsernameButton.Focus();
        }
        #endregion

        #region Forgotten Password Button
        private void ForgottenPasswordButton_MouseEnter(object sender, EventArgs e)
        {
            ForgottenPasswordButton.BackColor = Color.FromArgb(25, 0, 203, 255);
        }

        private void ForgottenPasswordButton_MouseLeave(object sender, EventArgs e)
        {
            ForgottenPasswordButton.BackColor = Color.FromArgb(0, 0, 203, 255);
        }

        private void ForgottenPasswordButton_Click(object sender, EventArgs e)
        {
            SetForgottenPasswordRecover();
        }

        private void ForgottenPasswordHeader_Click(object sender, EventArgs e)
        {
            SetForgottenPasswordRecover();
        }

        private void SetForgottenPasswordRecover()
        {
            usernameLost = false;
            passwordLost = true;

            ResetCredentialsHeader.Text = "¿Has olvidado tu contraseña? \n" +
                "Puedes solicitar cambiar tu contraseña anterior por otra nueva";

            if (!ResetCredentialsHeader.Visible)
            {
                ResetCredentialsHeader.Visible = true;
                ResetCredentialsEmailPanel.Visible = true;
                ResetCredentialsButton.Visible = true;
            }

            ResetCredentialsEmailText.Text = "";
            ForgottenPasswordHeader.Focus();
        }
        #endregion

        #region Reset Credentials Textbox Focus Gain
        private void ResetCredentialsEmailPanel_Click(object sender, EventArgs e)
        {
            // Cuando haces click en el panel contenedor
            ResetCredentialsEmailText.Focus();
        }

        private void ResetCredentialsEmailLabel_Click(object sender, EventArgs e)
        {
            // Cuando haces click en el label
            ResetCredentialsEmailText.Focus();
        }

        private void ResetCredentialsEmailText_Enter(object sender, EventArgs e)
        {
            // Cuando el textbox coge el foco
            ResetCredentialsFocusGain();
        }

        private void ResetCredentialsFocusGain()
        {
            if (ResetCredentialsEmailText.Text.Length == 0)
            {
                ResetCredentialsEmailLabel.Font = new Font("Perpetua Titling MT", 6, FontStyle.Bold);
                ResetCredentialsEmailLabel.Location = new Point(0, 2);
            }
        }
        #endregion

        #region Reset Credentials Textbox Focus Lost
        private void ResetCredentialsEmailText_Leave(object sender, EventArgs e)
        {
            // Cuando el textbox pierde el foco
            ResetCredentialsFocusLost();
        }

        private void ResetCredentialsFocusLost()
        {
            if (ResetCredentialsEmailText.Text.Length == 0)
            {
                ResetCredentialsEmailLabel.Font = new Font("Oxygen", 10);
                ResetCredentialsEmailLabel.Location = new Point(14, 10);
            }
        }
        #endregion

        #region Write Reset Credentials Textbox
        private void ResetCredentialsEmailText_TextChanged(object sender, EventArgs e)
        {
            if (ResetCredentialsEmailText.Text.Length > 4 && IsEmailValid(ResetCredentialsEmailText.Text))
            {
                ResetCredentialsButton.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.loginSuccessful;
                ResetCredentialsButton.Cursor = System.Windows.Forms.Cursors.Hand;
                loginProblemsAvaible = true;
            }
            else
            {
                ResetCredentialsButton.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.loginUnavaible;
                ResetCredentialsButton.Cursor = System.Windows.Forms.Cursors.Default;
                loginProblemsAvaible = false;
            }
        }
        #endregion

        #region Reset Credentials Button
        private void ResetCredentialsButton_Click(object sender, EventArgs e)
        {
            if (loginProblemsAvaible)
            {
                ResetCredentialsButton.Focus();

                if (usernameLost)
                {
                    EventText.Location = new Point(40, 25);
                    EventPasswordCode.Visible = false;
                    EventSendButton.Visible = false;
                    EventExitButton.Location = new Point(237, 105);

                    // Buscar en firebase para ver si existe el correo, si existe, mandar dicho correo
                    EventText.Text = "Si la dirección de correo coincide con alguna dirección de correo de " +
                            "nuestra base de datos te mandaremos un recordatorio de tu nombre de usuario";

                    SendEmail.SendNewEmail(ResetCredentialsEmailText, "Recordatorio del nombre de usuario del cliente de" +
                        " Outlands Adventure", "Su nombre de usuario es", "Napo"); // Cambiar Napo por el nombre de la bd
                }
                else if (passwordLost)
                {
                    EventText.Location = new Point(40, 9);
                    EventPasswordCode.Visible = true;
                    EventSendButton.Visible = true;
                    EventExitButton.Location = new Point(305, EventSendButton.Location.Y);

                    // Buscar en firebase para ver si existe el correo, si existe, mandar dicho correo
                    EventText.Text = "Si la dirección de correo coincide con alguna dirección de correo de " +
                            "nuestra base de datos te enviaremos un código de confirmación para que reestablezcas tu contraseña";

                    string confirmationCode = CreateConfirmationCode.CreateCode();
                    Hash_SHA2.InitialiceVariables(confirmationCode);
                    SendEmail.SendNewEmail(ResetCredentialsEmailText, "Reestablecimiento de la contraseña del cliente de" +
                           " Outlands Adventure", "Su código de confimación es", confirmationCode);
                }

                targetPanel = "login";

                ShowImageGradient();
                EventsPanel.Visible = true;
                ResetLoginProblemsPanelValues();
            }
            else
            {
                ResetCredentialsButton.Focus();
            }
        }
        #endregion

        #endregion Login Problems

        #region Game Client Configuration
        // This method manage game client configuration button when you click on it
        private void Configuration_Click(object sender, EventArgs e)
        {
            ConfigurationButton.Focus();
            ShowImageGradient();
            ConfigurationPanel.Visible = true;
        }

        #region Select Idiom Panel
        // Hide select client idiom panel when you click out of it
        private void ConfigurationPanel_Click(object sender, EventArgs e)
        {
            if (SelectClientIdiom.Visible)
            {
                HideSelectIdiomPanel();
            }
        }

        // Button to select a new client idiom
        private void SelectedClientIdiom_Click(object sender, EventArgs e)
        {
            SelectClientIdiom.Visible = true;
            ConfigurationExitButton.Visible = false;
        }

        private void HideSelectIdiomPanel()
        {
            SelectClientIdiom.Visible = false;
            ConfigurationExitButton.Visible = true;
        }
        #endregion

        #region Configuration Exit Button
        private void ExitButton_MouseEnter(object sender, EventArgs e)
        {
            ConfigurationExitButton.Font = new Font("Oxygen", 12, FontStyle.Bold);
        }

        private void ExitButton_MouseLeave(object sender, EventArgs e)
        {
            ConfigurationExitButton.Font = new Font("Oxygen", 12, FontStyle.Regular);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            CheckTargetPanel();
        }
        #endregion

        #region Select New Client Idiom
        private void SelectNewClientIdiom(object sender, EventArgs e)
        {
            string idiomSelected = ((Panel)sender).Name + "_flag";

            switch (idiomSelected)
            {
                case "spanish_flag":
                    SelectedClientIdiom.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.spanish_flag;
                    // Reiniciar el cliente y cambiar el idioma
                    break;

                case "english_flag":
                    SelectedClientIdiom.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.english_flag;
                    // Reiniciar el cliente y cambiar el idioma
                    break;
            }

            HideSelectIdiomPanel();
        }
        #endregion

        #endregion Game Client Configuration

        #region Others
        // Miscellany of methods and functions for various things
        #region Lose Focus
        // These methods lose focus to any box  -- Hace que todas las cajas de texto pierdan el foco
        private void LoginText_Click(object sender, EventArgs e)
        {
            LoginPanel.Focus();
        }

        private void WrongCredentials_Click(object sender, EventArgs e)
        {
            LoginPanel.Focus();
        }

        private void LoginPanel_Click(object sender, EventArgs e)
        {
            LoginPanel.Focus();
        }

        private void RegisterText_Click(object sender, EventArgs e)
        {
            RegisterPanel.Focus();
        }

        private void RegisterPanel_Click(object sender, EventArgs e)
        {
            RegisterPanel.Focus();
        }

        private void LoginProblemsPanel_Click(object sender, EventArgs e)
        {
            LoginProblemsPanel.Focus();
        }

        private void LoginProblemsHeader_1_Click(object sender, EventArgs e)
        {
            LoginProblemsHeader_1.Focus();
        }

        private void LoginProblemsHeader_2_Click(object sender, EventArgs e)
        {
            LoginProblemsHeader_2.Focus();
        }

        private void ResetCredentialsHeader_Click(object sender, EventArgs e)
        {
            ResetCredentialsHeader.Focus();
        }

        private void ImagePanel_Click(object sender, EventArgs e)
        {
            ImagePanel.Focus();
        }

        private void ResetPasswordEventPanel_Click(object sender, EventArgs e)
        {
            ResetPasswordEventPanel.Focus();
        }

        private void ResetPasswordEventText_Click(object sender, EventArgs e)
        {
            ResetPasswordEventPanel.Focus();
        }
        #endregion

        #region Show and Hide Image Gradient Panel
        // Methods to show Image Gradient Panel
        private void ShowImageGradient()
        {
            ConfigurationButton.Visible = false;
            BackgroundPanel.Visible = false;
            LoginPanel.Visible = false;
            RegisterPanel.Visible = false;
            LoginProblemsPanel.Visible = false;

            ImageGradient.Visible = true;
        }

        // Methods to hide Image Gradient Panel
        private void ImageGradient_Click(object sender, EventArgs e)
        {
            ImageGradient.Focus();

            if (SelectClientIdiom.Visible)
            {
                HideSelectIdiomPanel();
            }
        }

        private void CheckTargetPanel()
        {
            BackgroundPanel.Visible = true;

            if (targetPanel.Equals("login"))
            {
                LoginPanel.Visible = true;
                RegisterPanel.Visible = false;
                LoginProblemsPanel.Visible = false;
            }
            else if (targetPanel.Equals("register"))
            {
                LoginPanel.Visible = false;
                RegisterPanel.Visible = true;
                LoginProblemsPanel.Visible = false;
            }
            else if (targetPanel.Equals("loginProblems"))
            {
                LoginPanel.Visible = false;
                RegisterPanel.Visible = false;
                LoginProblemsPanel.Visible = true;
            }

            BackgroundPanel.Focus();
            ImageGradient.Visible = false;
            ConfigurationPanel.Visible = false;
            EventsPanel.Visible = false;
            ConfigurationButton.Visible = true;
        }
        #endregion

        #region Show Hide Password
        // These methods are used to show and hide the password in password textboxes
        private void ShowPassword_Click(object sender, EventArgs e)
        {
            this.passwordVisible = this.passwordVisible ? false : true;

            if (!passwordVisible)
            {
                currentShowPasswordButton.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.hide_password;
                currentPasswordTextbox.PasswordChar = '•';
            }
            else
            {
                currentShowPasswordButton.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.show_password;
                currentPasswordTextbox.PasswordChar = '\0';
            }
        }

        private void ShowConfirmPassword_Click(object sender, EventArgs e)
        {
            this.confirmPasswordVisible = this.confirmPasswordVisible ? false : true;

            if (!this.confirmPasswordVisible)
            {
                currentShowPasswordButton.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.hide_password;
                currentPasswordTextbox.PasswordChar = '•';
            }
            else
            {
                currentShowPasswordButton.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.show_password;
                currentPasswordTextbox.PasswordChar = '\0';
            }
        }

        private void ShowResetPassword_Click(object sender, EventArgs e)
        {
            this.confirmPasswordVisible = this.confirmPasswordVisible ? false : true;

            if (!this.confirmPasswordVisible)
            {
                currentShowPasswordButton.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.hide_password;
                currentPasswordTextbox.PasswordChar = '•';
            }
            else
            {
                currentShowPasswordButton.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.show_password;
                currentPasswordTextbox.PasswordChar = '\0';
            }
        }
        #endregion

        #region Create New Account and Login Problems (Login Panel)
        // These methods manage new account button when mouse enter, exit and click on it
        private void Register_LoginProblems_MouseEnter(object sender, EventArgs e)
        {
            ((Label)sender).Font = new Font("Oxygen", 9, FontStyle.Bold);
        }

        private void Register_LoginProblems_MouseLeave(object sender, EventArgs e)
        {
            ((Label)sender).Font = new Font("Oxygen", 9, FontStyle.Regular);
        }

        // Reset all values
        private void RegisterLabel_Click(object sender, EventArgs e)
        {
            ResetLoginPanelValues();

            LoginPanel.Visible = false;
            RegisterPanel.Visible = true;
            LoginProblemsPanel.Visible = false;

            targetPanel = "register";

            NewEmailTextbox.Focus();
        }

        private void LoginProblems_Click(object sender, EventArgs e)
        {
            ResetLoginPanelValues();

            LoginPanel.Visible = false;
            RegisterPanel.Visible = false;
            LoginProblemsPanel.Visible = true;

            targetPanel = "loginProblems";
        }
        #endregion

        #region Login Existing Account (Register Panel)
        // These methods manage new account button when mouse enter, exit and click on it
        private void LoginLabel_MouseEnter(object sender, EventArgs e)
        {
            LoginLabel.Font = new Font("Oxygen", 9, FontStyle.Bold);
        }

        private void LoginLabel_MouseLeave(object sender, EventArgs e)
        {
            LoginLabel.Font = new Font("Oxygen", 9, FontStyle.Regular);
        }

        // Reset all values
        private void LoginLabel_Click(object sender, EventArgs e)
        {
            ResetRegisterPanelValues();

            LoginPanel.Visible = true;
            RegisterPanel.Visible = false;
            LoginProblemsPanel.Visible = false;

            targetPanel = "login";
        }
        #endregion

        #region Return to Login (Login Problems Panel)
        private void ReturnToLogin_MouseEnter(object sender, EventArgs e)
        {
            ReturnToLogin.Font = new Font("Oxygen", 9, FontStyle.Bold);
        }

        private void ReturnToLogin_MouseLeave(object sender, EventArgs e)
        {
            ReturnToLogin.Font = new Font("Oxygen", 9, FontStyle.Regular);
        }

        private void ReturnToLogin_Click(object sender, EventArgs e)
        {
            ResetLoginProblemsPanelValues();

            LoginPanel.Visible = true;
            RegisterPanel.Visible = false;
            LoginProblemsPanel.Visible = false;

            targetPanel = "login";
        }
        #endregion

        #region Reset Panel Values
        private void ResetLoginPanelValues()
        {
            UserNameTextbox.Text = "";
            PasswordTextbox.Text = "";
            RememberMe.Checked = false;
            passwordVisible = false;
            ShowLoginPassword.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.hide_password;

            RegisterLabel.Focus();
        }

        private void ResetRegisterPanelValues()
        {
            NewEmailTextbox.Text = "";
            NewUserNameTextbox.Text = "";
            NewPasswordTextbox.Text = "";
            ConfirmNewPasswordTextbox.Text = "";
            NewEmailErrorLabel.Text = "";
            NewUserNameErrorLabel.Text = "";
            NewPasswordErrorLabel.Text = "";
            ConfirmNewPasswordErrorLabel.Text = "";

            ShowPassword_Click(ShowNewPassword, EventArgs.Empty);
            ShowConfirmPassword_Click(ShowConfirmNewPassword, EventArgs.Empty);

            NewEmailFocusLost();
            NewUserNameFocusLost();
            NewPasswordFocusLost();
            ConfirmNewPasswordFocusLost();

            LoginLabel.Focus();
            UserNameTextbox.Focus();
        }

        private void ResetLoginProblemsPanelValues()
        {
            ResetCredentialsEmailText.Text = "";

            ResetCredentialsHeader.Visible = false;
            ResetCredentialsEmailPanel.Visible = false;
            ResetCredentialsButton.Visible = false;

            ReturnToLogin.Focus();
            UserNameTextbox.Focus();

        }

        private void ResetEventsValue()
        {
            EventPasswordCode.Text = "";
            EventErrorText.Visible = false;
        }

        private void Reset_ResetPasswordEventValues()
        {
            ResetPasswordTextbox.Text = "";
            ShowResetPassword_Click(ShowResetPassword, EventArgs.Empty);

            ResetPasswordPanel.Focus();
            ResetPasswordEventPanel.Visible = false;
        }
        #endregion

        #region Events Panel

        #region Send and Exit Event Button
        private void EventSend_ExitButton_MouseEnter(object sender, EventArgs e)
        {
            ((Label) sender).Font = new Font("Oxygen", 12, FontStyle.Bold);
        }

        private void EventSend_ExitButton_MouseLeave(object sender, EventArgs e)
        {
            ((Label)sender).Font = new Font("Oxygen", 12, FontStyle.Regular);
        }

        private void EventSendButton_Click(object sender, EventArgs e)
        {
            CheckHashResumes();
        }

        private void EventExitButton_Click(object sender, EventArgs e)
        {
            CheckTargetPanel();
            ResetEventsValue();
        }

        private void EventPasswordCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CheckHashResumes();
            }
        }

        private void CheckHashResumes()
        {
            if (Hash_SHA2.VerifyResume(EventPasswordCode.Text))
            {
                EventErrorText.Visible = false;

                EventsPanel.Visible = false;
                ResetPasswordEventPanel.Visible = true;

                ResetEventsValue();
            }
            else
            {
                EventErrorText.Visible = true;
            }
        }
        #endregion

        #endregion Events Panel

        #region Reset Password Event
        private void ResetPasswordPanel_Click(object sender, EventArgs e)
        {
            ResetPasswordTextbox.Focus();
        }

        private void ResetPasswordLabel_Click(object sender, EventArgs e)
        {
            ResetPasswordTextbox.Focus();
        }

        private void ResetPasswordTextbox_Enter(object sender, EventArgs e)
        {
            ResetPasswordFocusGain();
        }

        private void ResetPasswordTextbox_Leave(object sender, EventArgs e)
        {
            ResetPasswordFocusLost();
        }

        private void ResetPasswordFocusGain()
        {
            if (ResetPasswordTextbox.Text.Length == 0)
            {
                ResetPasswordLabel.Font = new Font("Perpetua Titling MT", 6, FontStyle.Bold);
                ResetPasswordLabel.Location = new Point(0, 2);
            }

            ShowResetPassword.Visible = true;
            currentShowPasswordButton = ShowResetPassword;
            currentPasswordTextbox = ResetPasswordTextbox;

            if (Control.IsKeyLocked(Keys.CapsLock))
            {
                ResetPasswordMayusLock.Visible = true;
                ResetPasswordTextbox.Size = new Size(165, 22);
            }
            else
            {
                ResetPasswordMayusLock.Visible = false;
                ResetPasswordTextbox.Size = new Size(195, 22);
            }
        }

        private void ResetPasswordFocusLost()
        {
            // Pone el label que indica Constraseña otra vez con letra grande en el medio y comprueba errores de la caja de texto
            if (ResetPasswordTextbox.Text.Length == 0)
            {
                ResetPasswordLabel.Font = new Font("Oxygen", 10);
                ResetPasswordLabel.Location = new Point(14, 10);

                ResetPasswordStrengthProgressBar.Visible = false;
                ResetPasswordStrengthLabel.Visible = false;
                ResetPasswordEventErrorText.Visible = false;
            }
            else
            {
                if (ResetPasswordTextbox.Text.Length < 4)
                {
                    ResetPasswordStrengthProgressBar.Visible = false;
                    ResetPasswordStrengthLabel.Visible = false;

                    ResetPasswordEventErrorText.Visible = true;
                    ResetPasswordEventErrorText.Text = "Debe contener cuatro letras como mínimo";
                }
                else
                {
                    ResetPasswordEventErrorText.Visible = false;
                }
            }

            ShowResetPassword.Visible = false;
            ResetPasswordMayusLock.Visible = false;
        }

        private void ResetPasswordTextbox_TextChanged(object sender, EventArgs e)
        {
            ResetPasswordEventErrorText.Visible = false;
            ResetPasswordStrengthProgressBar.Visible = true;
            ResetPasswordStrengthLabel.Visible = true;

            PasswordStrength.CheckPasswordStrength(ResetPasswordTextbox, ResetPasswordStrengthProgressBar, ResetPasswordStrengthLabel);
        }

        private void ResetPasswordTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.CapsLock)
            {
                if (Control.IsKeyLocked(Keys.CapsLock))
                {
                    ResetPasswordMayusLock.Visible = true;
                    ResetPasswordTextbox.Size = new Size(165, 22);
                }
                else if (!Control.IsKeyLocked(Keys.CapsLock))
                {
                    ResetPasswordMayusLock.Visible = false;
                    ResetPasswordTextbox.Size = new Size(195, 22);
                }
            }

            if (e.KeyCode == Keys.Enter)
            {
                ResetPasswordEventPanel.Focus();
            }
        }

        private void ResetPasswordSendButton_Click(object sender, EventArgs e)
        {
            ResetPasswordFocusLost();

            if (!ResetPasswordEventErrorText.Visible)
            {
                Reset_ResetPasswordEventValues();
                CheckTargetPanel();
            }
        }

        private void ResetPasswordExitButton_Click(object sender, EventArgs e)
        {
            Reset_ResetPasswordEventValues();
            CheckTargetPanel();
        }
        #endregion

        #region CheckEmailAdress
        public bool IsEmailValid(string emailAddress)
        {
            try
            {
                MailAddress checkMailAdress = new MailAddress(emailAddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        #endregion

        #endregion Others
    }
}