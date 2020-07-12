using Outlands_Adventure_Launcher.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Outlands_Adventure_Launcher
{
    public partial class LauncherLogin : Form
    {
        private string targetPanel;

        private bool loginAvaible;
        private bool registerAvaible;
        private bool registerErrors;

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
            registerErrors = false;
            loginProblemsAvaible = false;

            passwordVisible = false;
            confirmPasswordVisible = false;

            InitializeComponent();

            ImageGradient.BackColor = Color.FromArgb(190, 0, 0, 0);

            ConfigurationPanel.BackColor = Color.FromArgb(255, 0, 0, 0);
        }

        #region Login Interface
        // Manage all items in the login panel

        #region UserName Textbox Focus
        // These methods make UserName Textbox gain the focus
        private void UserNamePanel_Label_Click(object sender, EventArgs e)
        {
            // Cuando haces click en el label o en el panel contenedor
            UserNameTextbox.Focus();
        }

        // This method manage the focus gain
        private void UserNameTextBox_Enter(object sender, EventArgs e)
        {
            // Cuando el textbox coge el foco
            TextboxGainFocusAnimation(UserNameTextbox, UserNameLabel, null, null);
        }

        // This method manage the focus lose
        private void UserNameTextBox_Leave(object sender, EventArgs e)
        {
            // Cuando el textbox pierde el foco
            TextboxLoseFocusAnimation(UserNameTextbox, UserNameLabel, null, null, null, null, null);
        }
        #endregion

        #region Password Textbox Focus
        // These methods make Password Textbox gain the focus
        private void PasswordPanel_Label_Click(object sender, EventArgs e)
        {
            // Cuando haces click en el label o en el panel contenedor
            PasswordTextbox.Focus();
        }

        private void PasswordTextbox_Enter(object sender, EventArgs e)
        {
            // Cuando el textbox coge el foco
            TextboxGainFocusAnimation(PasswordTextbox, PasswordLabel, ShowLoginPassword, LoginMayusLock);
        }

        // This method manage the focus lose
        private void PasswordTextbox_Leave(object sender, EventArgs e)
        {
            // Cuando el textbox pierde el foco
            TextboxLoseFocusAnimation(PasswordTextbox, PasswordLabel, null, ShowLoginPassword, LoginMayusLock, null, null);
        }
        #endregion

        #region Write Login Textboxs
        // These methods check username and password, if the is more that 4 characters in each textbox then enable the login button
        private void UserNameTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            TextboxKeyUp(e, LoginPanel, null, null);
        }

        private void PasswordTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            TextboxKeyUp(e, LoginPanel, PasswordTextbox, LoginMayusLock);
        }

        private void UserName_PasswordTextbox_TextChanged(object sender, EventArgs e)
        {
            CheckLoginTextboxs();
        }

        private void CheckLoginTextboxs()
        {
            if (UserNameTextbox.Text.Length >= 4 && PasswordTextbox.Text.Length >= 4)
            {
                Login_RegisterButton(true, LoginButton, ref loginAvaible);
            }
            else
            {
                Login_RegisterButton(false, LoginButton, ref loginAvaible);
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
                    // Cerrar este form y llamar a otro form cuando te logeas
                }
            }
            else
            {
                LoginButton.Focus();
            }
        }

        private void CheckLoginCredentials()
        {
            string sqlQuery = "SELECT COUNT(*) FROM user_information WHERE user_name LIKE '" + UserNameTextbox.Text + "' &&" +
                " user_password = SHA('" + PasswordTextbox.Text + "')";
            int rowsRecovered = SQLManager.CheckDuplicatedData(sqlQuery);

            if (rowsRecovered > 0)
            {
                loginAvaible = true;
                WrongCredentials.Visible = false;
            }
            else if (rowsRecovered == 0)
            {
                Login_RegisterButton(false, LoginButton, ref loginAvaible);
                WrongCredentials.Visible = true;
            }
            else
            {
                ShowImageGradient();
                EventsPanel.Visible = true;
                GenericPopUpMessage("No se puede conectar con la BD");
            }
        }
        #endregion

        #endregion Login Interface

        #region Register Interface
        //Manage all items in the register panel

        #region New Email Textbox Focus
        // These methods make New Email Textbox gain and loose the focus
        private void NewEmailPanel_Label_Click(object sender, EventArgs e)
        {
            // Cuando haces click en el label o en el panel contenedor
            NewEmailTextbox.Focus();
        }

        private void NewEmailTextbox_Enter(object sender, EventArgs e)
        {
            // Cuando el textbox coge el foco
            TextboxGainFocusAnimation(NewEmailTextbox, NewEmailLabel, null, null);
        }

        private void NewEmailTextbox_Leave(object sender, EventArgs e)
        {
            // Cuando el textbox pierde el foco
            CheckRegister_ResetTexboxErrors(NewEmailTextbox);
        }
        #endregion

        #region New UserName Textbox Focus
        // These methods make New UserName Textbox gain and loose the focus
        private void NewUserNamePanel_Label_Click(object sender, EventArgs e)
        {
            // Cuando haces click en el label o en el panel contenedor
            NewUserNameTextbox.Focus();
        }

        private void NewUserNameTextbox_Enter(object sender, EventArgs e)
        {
            // Cuando el textbox coge el foco
            TextboxGainFocusAnimation(NewUserNameTextbox, NewUserNameLabel, null, null);
        }

        private void NewUserNameTextbox_Leave(object sender, EventArgs e)
        {
            // Cuando el textbox pierde el foco
            CheckRegister_ResetTexboxErrors(NewUserNameTextbox);
        }
        #endregion

        #region New Password Textbox Focus
        // These methods make New Password Textbox gain and loose the focus
        private void NewPasswordPanel_Label_Click(object sender, EventArgs e)
        {
            // Cuando haces click en el label o en el panel contenedor
            NewPasswordTextbox.Focus();
        }

        private void NewPasswordTextbox_Enter(object sender, EventArgs e)
        {
            // Cuando el textbox coge el foco
            TextboxGainFocusAnimation(NewPasswordTextbox, NewPasswordLabel, ShowNewPassword, NewPasswordMayusLock);
        }

        private void NewPasswordTextbox_Leave(object sender, EventArgs e)
        {
            // Cuando el textbox pierde el foco
            CheckRegister_ResetTexboxErrors(NewPasswordTextbox);
        }
        #endregion

        #region Confirm New Password Textbox Focus
        // These methods make Confirm New Password Textbox gain and loose the focus
        private void ConfirmNewPasswordPanel_Label_Click(object sender, EventArgs e)
        {
            // Cuando haces click en el label o en el panel contenedor
            ConfirmNewPasswordTextbox.Focus();
        }

        private void ConfirmNewPasswordTextbox_Enter(object sender, EventArgs e)
        {
            // Cuando el textbox coge el foco
            TextboxGainFocusAnimation(ConfirmNewPasswordTextbox, ConfirmNewPasswordLabel, ShowConfirmNewPassword,
                ConfirmNewPasswordMayusLock);
        }

        private void ConfirmNewPasswordTextbox_Leave(object sender, EventArgs e)
        {
            // Cuando el textbox pierde el foco
            CheckRegister_ResetTexboxErrors(ConfirmNewPasswordTextbox);
        }
        #endregion

        #region Write Register Textboxs
        // These methods check email, username and passwords, if there are any errors in any textbox then unable the register button
        // In password textboxes check if the key pressed is Lock Capital to show a warning to the user
        private void NewEmail_UserNameTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            TextboxKeyUp(e, RegisterPanel, null, null);
        }

        private void NewPasswordTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            TextboxKeyUp(e, RegisterPanel, NewPasswordTextbox, NewPasswordMayusLock);
        }

        private void ConfirmNewPasswordTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            TextboxKeyUp(e, RegisterPanel, ConfirmNewPasswordTextbox, ConfirmNewPasswordMayusLock);
        }

        private void RegisterTextboxes_TextChanged(object sender, EventArgs e)
        {
            CheckRegisterTextboxs();
        }

        private void NewPasswordTextbox_TextChanged(object sender, EventArgs e)
        {
            CheckTextboxPasswordStrength(NewPasswordTextbox, NewPasswordErrorLabel, NewPasswordStrengthProgressBar, 
                NewPasswordStrengthLabel);
            CheckRegisterTextboxs();
        }

        // Check the textboxes contents
        private void CheckRegisterTextboxs()
        {
            // If all textboxes has at least 4 caracters then enable the register button
            if (NewEmailTextbox.Text.Length >= 4 && NewUserNameTextbox.Text.Length >= 4 && NewPasswordTextbox.Text.Length >= 4 &&
                ConfirmNewPasswordTextbox.Text.Length >= 4)
            {
                Login_RegisterButton(true, RegisterButton, ref registerAvaible);
            }
            else
            {
                Login_RegisterButton(false, RegisterButton, ref registerAvaible);
            }
        }
        #endregion

        #region Register in
        // This method manage login button when you click on it
        private void RegisterButton_Click(object sender, EventArgs e)
        {
            RegisterButton.Focus();

            if (!NewEmailErrorLabel.Visible && !NewUserNameErrorLabel.Visible && !NewPasswordErrorLabel.Visible &&
                !ConfirmNewPasswordErrorLabel.Visible)
            {
                if (registerAvaible)
                {
                    CheckRegisterCredentials();

                    if (registerAvaible && !registerErrors)
                    {
                        string confirmationCode = CreateConfirmationCode.CreateCode();
                        Hash_SHA2.InitialiceVariables(confirmationCode);
                        bool messageError = SendEmail.SendNewEmail(NewEmailTextbox, "Creación de una nueva cuenta del cliente de" +
                               " Outlands Adventure", "Su código de confimación es", confirmationCode);

                        if (!messageError)
                        {
                            EventText.Location = new Point(40, 5);
                            EventCode.Visible = true;
                            EventSendButton.Visible = true;
                            EventExitButton.Location = new Point(305, EventSendButton.Location.Y);

                            EventText.Text = "Hemos mandado un código de confirmación a tu correo electrónico, dirígete a tu correo" +
                                " e introduce el código para confirmar tu cuenta";

                            targetPanel = "login";
                            registerErrors = false;

                            ShowImageGradient();
                            EventsPanel.Visible = true;
                        }
                        else
                        {
                            // Cambio registerErrors de true a false para que despúes de mandar el mensaje de error
                            //puedas intentarlo de nuevo al instante
                            registerErrors = true;
                            ShowImageGradient();
                            EventsPanel.Visible = true;
                            GenericPopUpMessage("Problema al enviar el correo");
                        }
                    }
                }
            }
            else
            {
                Login_RegisterButton(false, RegisterButton, ref registerAvaible);
            }
        }

        private void CheckRegisterCredentials()
        {
            string sqlQuery = "SELECT COUNT(*) FROM user_information WHERE user_email LIKE '" + NewEmailTextbox.Text + "'";
            int emailRowsRecovered = SQLManager.CheckDuplicatedData(sqlQuery);

            sqlQuery = "SELECT COUNT(*) FROM user_information WHERE user_name LIKE '" + NewUserNameTextbox.Text + "'";
            int nameRowsRecovered = SQLManager.CheckDuplicatedData(sqlQuery);

            if (emailRowsRecovered > -1 && nameRowsRecovered > -1)
            {
                if (emailRowsRecovered > 0)
                {
                    GenericError(NewEmailErrorLabel, "Dirección de correo ya registrada");
                    Login_RegisterButton(false, RegisterButton, ref registerAvaible);
                }
                else
                {
                    NewEmailErrorLabel.Visible = false;
                }

                if (nameRowsRecovered > 0)
                {
                    GenericError(NewUserNameErrorLabel, "Nombre de usuario ya registrado");
                    Login_RegisterButton(false, RegisterButton, ref registerAvaible);
                }
                else
                {
                    NewUserNameErrorLabel.Visible = false;
                }
            }
            else
            {
                // Cambio registerErrors de true a false para que despúes de mandar el mensaje de error
                //puedas intentarlo de nuevo al instante
                registerErrors = true;
                ShowImageGradient();
                EventsPanel.Visible = true;
                GenericPopUpMessage("No se puede conectar con la BD");
            }
        }

        private void RegisterNewUser()
        {
            string sqlQuery = "INSERT INTO user_information VALUES ('" + NewEmailTextbox.Text + "', '"
                + NewUserNameTextbox.Text + "', SHA('" + NewPasswordTextbox.Text + "'), null)";
            string queryError = SQLManager.InsertNewUser(sqlQuery);

            if (queryError.Length > 0)
            {
                if (queryError.Contains("Unable to connect"))
                {
                    // Pop up de falta de internet - No te puedes conectar a la base de datos
                    GenericPopUpMessage("No se puede conectar con la BD");
                }

                else
                {
                    // Cualquier otro tipo de error de la base de datos que tendra que salir en el pop up
                    GenericPopUpMessage(queryError);
                    MessageBox.Show(queryError);
                }
                registerErrors = true;
            }
        }
        #endregion

        #endregion Register Interface

        #region Login Problems Interface
        // Manage all items in login problems panel

        #region Forgotten Username / Password Button
        private void ForgottenUsername_PasswordButton_MouseEnter(object sender, EventArgs e)
        {
            ForgottenUsernameButton.BackColor = Color.FromArgb(25, 0, 203, 255);
        }

        private void ForgottenUsername_PasswordButton_MouseLeave(object sender, EventArgs e)
        {
            ForgottenUsernameButton.BackColor = Color.FromArgb(0, 0, 203, 255);
        }

        private void ForgottenUsernameButton_Header_Click(object sender, EventArgs e)
        {
            SetForgottenUsername_PasswordRecover(true);
        }

        private void ForgottenPasswordButton_Header_Click(object sender, EventArgs e)
        {
            SetForgottenUsername_PasswordRecover(false);
        }

        private void SetForgottenUsername_PasswordRecover(bool forgottenUsername)
        {
            if (forgottenUsername)
            {
                usernameLost = true;
                passwordLost = false;

                ResetCredentialsHeader.Text = "¿Necesitas ayuda para recordar tu nombre de usuario? \n" +
                    "Puedes solicitar que te mandemos un recordatorio de tu usuario a tu correo electrónico";
            }
            else
            {
                usernameLost = false;
                passwordLost = true;

                ResetCredentialsHeader.Text = "¿Has olvidado tu contraseña? \n" +
                    "Puedes solicitar cambiar tu contraseña anterior por otra nueva";
            }

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

        #region Reset Credentials Textbox Focus
        private void ResetCredentialsEmailPanel_Label_Click(object sender, EventArgs e)
        {
            // Cuando haces click en el panel contenedor o en el label
            ResetCredentialsEmailText.Focus();
        }

        private void ResetCredentialsEmailText_Enter(object sender, EventArgs e)
        {
            // Cuando el textbox coge el foco
            TextboxGainFocusAnimation(ResetCredentialsEmailText, ResetCredentialsEmailLabel, null, null);
        }

        private void ResetCredentialsEmailText_Leave(object sender, EventArgs e)
        {
            // Cuando el textbox pierde el foco
            TextboxLoseFocusAnimation(ResetCredentialsEmailText, ResetCredentialsEmailLabel, null, null, null, null, null);
        }
        #endregion

        #region Write Reset Credentials Textbox
        private void ResetCredentialsEmailText_TextChanged(object sender, EventArgs e)
        {
            if (ResetCredentialsEmailText.Text.Length > 4 && IsEmailValid(ResetCredentialsEmailText.Text))
            {
                Login_RegisterButton(true, ResetCredentialsButton, ref loginProblemsAvaible);
            }
            else
            {
                Login_RegisterButton(false, ResetCredentialsButton, ref loginProblemsAvaible);
            }
        }
        #endregion

        #region Reset Credentials Button
        private void ResetCredentialsButton_Click(object sender, EventArgs e)
        {
            if (loginProblemsAvaible)
            {
                BackgroundPanel.Visible = false;
                LoginProblemsPanel.Visible = false;

                ResetCredentialsButton.Focus();
                bool messageError = false;

                if (usernameLost)
                {
                    EventText.Location = new Point(40, 25);
                    EventCode.Visible = false;
                    EventSendButton.Visible = false;
                    EventExitButton.Location = new Point(237, 105);

                    // Buscar en firebase para ver si existe el correo, si existe, mandar dicho correo
                    EventText.Text = "Si la dirección de correo coincide con alguna dirección de correo de " +
                            "nuestra base de datos te mandaremos un recordatorio de tu nombre de usuario";

                    messageError = SendEmail.SendNewEmail(ResetCredentialsEmailText, "Recordatorio del nombre de usuario del cliente de" +
                        " Outlands Adventure", "Su nombre de usuario es", "Napo"); // Cambiar Napo por el nombre de la bd
                }
                else if (passwordLost)
                {
                    EventText.Location = new Point(40, 9);
                    EventCode.Visible = true;
                    EventSendButton.Visible = true;
                    EventExitButton.Location = new Point(305, EventSendButton.Location.Y);

                    // Buscar en firebase para ver si existe el correo, si existe, mandar dicho correo
                    EventText.Text = "Si la dirección de correo coincide con alguna dirección de correo de " +
                            "nuestra base de datos te enviaremos un código de confirmación para que reestablezcas tu contraseña";

                    string confirmationCode = CreateConfirmationCode.CreateCode();
                    Hash_SHA2.InitialiceVariables(confirmationCode);
                    messageError = SendEmail.SendNewEmail(ResetCredentialsEmailText, "Reestablecimiento de la contraseña del cliente de" +
                           " Outlands Adventure", "Su código de confimación es", confirmationCode);
                }

                if (messageError)
                {
                    LoginProblemsPanel.Visible = true;
                    Login_RegisterButton(false, ResetCredentialsButton, ref loginProblemsAvaible);
                }
                else
                {
                    targetPanel = "login";

                    ShowImageGradient();
                    EventsPanel.Visible = true;
                    ResetLoginProblemsPanelValues();
                }
            }
            else
            {
                ResetCredentialsButton.Focus();
            }
        }
        #endregion

        #endregion Login Problems Interface

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

        #region Popup Events

        #region Events Panel
        private void EventSend_ExitButton_MouseEnter(object sender, EventArgs e)
        {
            ((Label)sender).Font = new Font("Oxygen", 12, FontStyle.Bold);
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
            if (registerErrors)
            {
                targetPanel = "register";
                registerErrors = false;
            }
            else
            {
                targetPanel = "login";
            }

            CheckTargetPanel();
            ResetEventsValue();
        }

        private void EventCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CheckHashResumes();
            }
        }

        private void EventCode_TextChanged(object sender, EventArgs e)
        {
            EventCode.Text = EventCode.Text.ToUpper();
            EventCode.Select(EventCode.Text.Length, 0);
        }

        private void CheckHashResumes()
        {
            if (Hash_SHA2.VerifyResume(EventCode.Text))
            {
                EventCodeError.Visible = false;

                EventsPanel.Visible = false;

                if (passwordLost) ResetPasswordEventPanel.Visible = true;

                if (registerAvaible)
                {
                    RegisterNewUser();
                    if (!registerErrors)
                    {
                        ResetRegisterPanelValues();
                        GenericPopUpMessage("Te has registrado exitosamente");
                    }
                }

                ResetEventsValue();
            }
            else
            {
                EventCodeError.Visible = true;
            }
        }
        #endregion Events Panel

        #region Reset Password Event
        private void ResetPasswordPanel_Label_Click(object sender, EventArgs e)
        {
            // Cuando haces click en el label o en el panel contenedor
            ResetPasswordTextbox.Focus();
        }

        private void ResetPasswordTextbox_Enter(object sender, EventArgs e)
        {
            TextboxGainFocusAnimation(ResetPasswordTextbox, ResetPasswordLabel, ShowResetPassword, ResetPasswordMayusLock);
        }

        private void ResetPasswordTextbox_Leave(object sender, EventArgs e)
        {
            CheckRegister_ResetTexboxErrors(ResetPasswordTextbox);
        }


        private void ResetPasswordTextbox_TextChanged(object sender, EventArgs e)
        {
            CheckTextboxPasswordStrength(ResetPasswordTextbox, ResetPasswordEventErrorText, ResetPasswordStrengthProgressBar,
                ResetPasswordStrengthLabel);
        }

        private void ResetPasswordTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            TextboxKeyUp(e, ResetPasswordPanel, ResetPasswordTextbox, ResetPasswordMayusLock);
        }

        private void ResetPasswordSendButton_Click(object sender, EventArgs e)
        {
            CheckRegister_ResetTexboxErrors(ResetPasswordTextbox);

            if (!ResetPasswordEventErrorText.Visible)
            {
                Reset_ResetPasswordEventValues();
                GenericPopUpMessage("Contraseña cambiada con éxito");
            }
        }

        private void ResetPasswordExitButton_Click(object sender, EventArgs e)
        {
            targetPanel = "login";
            CheckTargetPanel();
            Reset_ResetPasswordEventValues();
        }
        #endregion Reset Password Event

        private void GenericPopUpMessage(string eventText)
        {
            EventText.Location = new Point(40, 25);
            EventCode.Visible = false;
            EventSendButton.Visible = false;
            EventExitButton.Location = new Point(237, 105);

            EventText.Text = eventText;

            ResetPasswordEventPanel.Visible = false;
            EventsPanel.Visible = true;
        }

        #endregion Popup Events

        #region Change Panel (ex: Change from login to register)
        private void ActionLabel_MouseEnter(object sender, EventArgs e)
        {
            ((Label)sender).Font = new Font("Oxygen", 9, FontStyle.Bold);
        }

        private void ActionLabel_MouseLeave(object sender, EventArgs e)
        {
            ((Label)sender).Font = new Font("Oxygen", 9, FontStyle.Regular);
        }

        // Create New Account (Login Panel)
        // Reset all values
        private void RegisterLabel_Click(object sender, EventArgs e)
        {
            ResetLoginPanelValues();

            targetPanel = "register";
            CheckTargetPanel();
        }

        // Login Problems (Login Panel)
        // Reset all values
        private void LoginProblems_Click(object sender, EventArgs e)
        {
            ResetLoginPanelValues();

            targetPanel = "loginProblems";
            CheckTargetPanel();
        }

        // Login Existing Account (Register Panel)
        // Reset all values
        private void LoginLabel_Click(object sender, EventArgs e)
        {
            ResetRegisterPanelValues();

            targetPanel = "login";
            CheckTargetPanel();
        }

        // Return to Login (Login Problems Panel)
        // Reset all values
        private void ReturnToLogin_Click(object sender, EventArgs e)
        {
            ResetLoginProblemsPanelValues();

            targetPanel = "login";
            CheckTargetPanel();
        }
        #endregion Change Panel

        #region Set destination panel  -  Reset current panel  -  Enable Loading Panel

        #region Check Target Panel
        private void CheckTargetPanel()
        {
            if (targetPanel.Equals("login"))
            {
                LoginPanel.Visible = true;
                RegisterPanel.Visible = false;
                LoginProblemsPanel.Visible = false;
                LoginPanel.Focus();
            }
            else if (targetPanel.Equals("register"))
            {
                LoginPanel.Visible = false;
                RegisterPanel.Visible = true;
                LoginProblemsPanel.Visible = false;
                RegisterPanel.Focus();
            }
            else if (targetPanel.Equals("loginProblems"))
            {
                LoginPanel.Visible = false;
                RegisterPanel.Visible = false;
                LoginProblemsPanel.Visible = true;
                LoginProblemsPanel.Focus();
            }

            HideImageGradient();
        }
        #endregion Check Target Panel

        #region Reset Panel Values
        private void ResetLoginPanelValues()
        {
            UserNameTextbox.Text = "";
            PasswordTextbox.Text = "";
            RememberMe.Checked = false;
            WrongCredentials.Visible = false;

            // Lo pongo a true para que luego al hacer el click se ponga a falso y cambie el icono
            passwordVisible = true;
            currentPasswordTextbox = PasswordTextbox;
            currentShowPasswordButton = ShowLoginPassword;
            ShowPassword_Click(null, EventArgs.Empty);

            // Llamar al método que se ejecuta cuando una caja de texto pierde el foco
            TextboxLoseFocusAnimation(UserNameTextbox, UserNameLabel, null, null, null, null, null);
            TextboxLoseFocusAnimation(PasswordTextbox, PasswordLabel, null, ShowLoginPassword, LoginMayusLock, null, null);
        }

        private void ResetRegisterPanelValues()
        {
            NewEmailTextbox.Text = "";
            NewUserNameTextbox.Text = "";
            NewPasswordTextbox.Text = "";
            ConfirmNewPasswordTextbox.Text = "";

            // Los pongo a true para que luego al hacer el click se pongan a falso y cambien el icono
            passwordVisible = true;
            currentPasswordTextbox = NewPasswordTextbox;
            currentShowPasswordButton = ShowNewPassword;
            ShowPassword_Click(null, EventArgs.Empty);

            confirmPasswordVisible = true;
            currentPasswordTextbox = ConfirmNewPasswordTextbox;
            currentShowPasswordButton = ShowConfirmNewPassword;
            ShowConfirmPassword_Click(null, EventArgs.Empty);

            // Llamar al método que se ejecuta cuando una caja de texto pierde el foco
            TextBox[] registerTexboxes = { NewEmailTextbox, NewUserNameTextbox, NewPasswordTextbox, ConfirmNewPasswordTextbox };
            for (int currentTextbox = 0; currentTextbox < registerTexboxes.Length; currentTextbox++)
            {
                CheckRegister_ResetTexboxErrors(registerTexboxes[currentTextbox]);
            }

            registerErrors = false;
        }

        private void ResetLoginProblemsPanelValues()
        {
            ResetCredentialsEmailText.Text = "";

            ResetCredentialsHeader.Visible = false;
            ResetCredentialsEmailPanel.Visible = false;
            ResetCredentialsButton.Visible = false;

            TextboxLoseFocusAnimation(ResetCredentialsEmailText, ResetCredentialsEmailLabel, null, null, null, null, null);
        }

        private void ResetEventsValue()
        {
            EventCode.Text = "";
            EventCodeError.Visible = false;
        }

        private void Reset_ResetPasswordEventValues()
        {
            ResetPasswordTextbox.Text = "";

            // Lo pongo a true para que luego al hacer el click se ponga a falso y cambie el icono
            confirmPasswordVisible = true;
            currentPasswordTextbox = ResetPasswordTextbox;
            currentShowPasswordButton = ShowResetPassword;
            ShowResetPassword_Click(null, EventArgs.Empty);

            // Llamar al método que se ejecuta cuando una caja de texto pierde el foco
            CheckRegister_ResetTexboxErrors(ResetPasswordTextbox);
        }
        #endregion Reset Panel Values

        #endregion Set destination panel  -  Reset current panel  -  Enable Loading Panel

        #region Textboxes / Email / Password comprobations
        /// <summary>
        /// Hace más pequeño el texto que indica para que sirve la caja de texto, te enseña el ojo para mostrar la contraseña
        /// y controla si el bloqueo de mayúsculas está activado para mostrar un aviso cuando el textbox coje el foco
        /// </summary>
        /// <param name="currentTextbox">Caja de texto donde escribir lo que necesites</param>
        /// <param name="currentTextboxLabel">Label que indica para que sirve la caja de texto</param>
        private void TextboxGainFocusAnimation(TextBox currentTextbox, Label currentTextboxLabel, Panel showPassword, 
            Panel mayusLock)
        {
            // El método solo deberia tener este if
            if (currentTextbox.Text.Length == 0)
            {
                currentTextboxLabel.Font = new Font("Perpetua Titling MT", 6, FontStyle.Bold);
                currentTextboxLabel.Location = new Point(0, 2);
            }

            // Esto se debería ir a otro método
            if (showPassword != null)
            {
                showPassword.Visible = true;
                currentShowPasswordButton = showPassword;
                currentPasswordTextbox = currentTextbox;

                if (Control.IsKeyLocked(Keys.CapsLock))
                {
                    mayusLock.Visible = true;
                    currentTextbox.Size = new Size(165, 22);
                }
                else
                {
                    mayusLock.Visible = false;
                    currentTextbox.Size = new Size(195, 22);
                }
            }
        }

        private void TextboxLoseFocusAnimation(TextBox currentTextbox, Label currentTextboxLabel, Label currentErrorLabel, 
            Panel showPassword, Panel mayusLock, ProgressBar passwordStrength, Label passwordStrengthLabel)
        {
            if (currentTextbox.Text.Length == 0)
            {
                EmptyTextbox(currentTextboxLabel, currentErrorLabel, showPassword, mayusLock, passwordStrength, passwordStrengthLabel);
            }

            if (showPassword != null)
            {
                showPassword.Visible = false;
                mayusLock.Visible = false;
            }
        }

        private void CheckRegister_ResetTexboxErrors(TextBox currentTextbox)
        {
            switch (currentTextbox.Name)
            {
                case "NewEmailTextbox":
                    if (NewEmailTextbox.Text.Length == 0)
                        EmptyTextbox(NewEmailLabel, NewEmailErrorLabel, null, null, null, null);

                    else if (NewEmailTextbox.Text.Length < 4)
                        LessFourLetters(NewEmailErrorLabel, "Debe contener cuatro letras como mínimo", null, null);

                    else if (!IsEmailValid(NewEmailTextbox.Text))
                        GenericError(NewEmailErrorLabel, "Introduce una dirección de correo válida");

                    else
                        NewEmailErrorLabel.Visible = false;
                    break;

                case "NewUserNameTextbox":
                    if (NewUserNameTextbox.Text.Length == 0)
                        EmptyTextbox(NewUserNameLabel, NewUserNameErrorLabel, null, null, null, null);

                    else if (NewUserNameTextbox.Text.Length < 4)
                        LessFourLetters(NewUserNameErrorLabel, "Debe contener cuatro letras como mínimo", null, null);

                    else
                        NewUserNameErrorLabel.Visible = false;
                    break;

                case "NewPasswordTextbox":
                    if (NewPasswordTextbox.Text.Length == 0)
                    {
                        EmptyTextbox(NewPasswordLabel, NewPasswordErrorLabel, ShowNewPassword, NewPasswordMayusLock,
                            NewPasswordStrengthProgressBar, NewPasswordStrengthLabel);
                    }

                    if (NewPasswordTextbox.Text.Length < 4 && NewPasswordTextbox.Text.Length > 0)
                    {
                        LessFourLetters(NewPasswordErrorLabel, "Debe contener cuatro letras como mínimo",
                            NewPasswordStrengthProgressBar, NewPasswordStrengthLabel);
                    }
                    else
                    {
                        NewPasswordErrorLabel.Visible = false;
                    }

                    if (!NewPasswordTextbox.Text.Equals(ConfirmNewPasswordTextbox.Text) && ConfirmNewPasswordTextbox.Text.Length > 0)
                    {
                        GenericError(ConfirmNewPasswordErrorLabel, "Las contraseñas no coinciden");
                    }
                    else
                    {
                        ConfirmNewPasswordErrorLabel.Visible = false;
                    }

                    ShowNewPassword.Visible = false;
                    NewPasswordMayusLock.Visible = false;
                    break;

                case "ConfirmNewPasswordTextbox":
                    if (ConfirmNewPasswordTextbox.Text.Length == 0)
                    {
                        EmptyTextbox(ConfirmNewPasswordLabel, ConfirmNewPasswordErrorLabel, ShowConfirmNewPassword,
                            ConfirmNewPasswordMayusLock, null, null);
                    }
                    else if (!NewPasswordTextbox.Text.Equals(ConfirmNewPasswordTextbox.Text) && NewPasswordTextbox.Text.Length > 0)
                    {
                        GenericError(ConfirmNewPasswordErrorLabel, "Las contraseñas no coinciden");
                    }
                    else if (ConfirmNewPasswordTextbox.Text.Length < 4)
                    {
                        LessFourLetters(ConfirmNewPasswordErrorLabel, "Debe contener cuatro letras como mínimo", null, null);
                    }
                    else
                    {
                        ConfirmNewPasswordErrorLabel.Visible = false;
                    }

                    ShowConfirmNewPassword.Visible = false;
                    ConfirmNewPasswordMayusLock.Visible = false;
                    break;

                case "ResetPasswordTextbox":
                    if (ResetPasswordTextbox.Text.Length == 0)
                    {
                        EmptyTextbox(ResetPasswordLabel, ResetPasswordEventErrorText, ShowResetPassword, ResetPasswordMayusLock,
                            ResetPasswordStrengthProgressBar, ResetPasswordStrengthLabel);
                    }
                    else if (ResetPasswordTextbox.Text.Length < 4)
                    {
                        LessFourLetters(ResetPasswordEventErrorText, "Debe contener cuatro letras como mínimo", 
                            ResetPasswordStrengthProgressBar, ResetPasswordStrengthLabel);
                    }
                    else
                    {
                        ResetPasswordEventErrorText.Visible = false;
                    }
                    break;
            }
        }

        /// Hace más grande el texto que indica para que sirve la caja de texto, oculta el ojo, quita el aviso de que está el
        /// bloque de mayúsculas activado y quita la barra y la etiqueta que te indica la fuerza de la contraseña
        private void EmptyTextbox(Label currentTextboxLabel, Label currentErrorLabel,
            Panel showPassword, Panel mayusLock, ProgressBar passwordStrength, Label passwordStrengthLabel)
        {
            currentTextboxLabel.Font = new Font("Oxygen", 10);
            currentTextboxLabel.Location = new Point(14, 10);
            if (currentErrorLabel != null) currentErrorLabel.Visible = false;

            if (showPassword != null)
            {
                showPassword.Visible = false;
                mayusLock.Visible = false;
            }

            if (passwordStrength != null)
            {
                passwordStrength.Visible = false;
                passwordStrengthLabel.Visible = false;
            }
        }

        private void LessFourLetters(Label currentErrorLabel, string errorText, ProgressBar passwordStrength, Label passwordStrengthLabel)
        {
            if (passwordStrength != null)
            {
                passwordStrength.Visible = false;
                passwordStrengthLabel.Visible = false;
            }

            currentErrorLabel.Visible = true;
            currentErrorLabel.Text = errorText;
        }

        private void GenericError(Label currentErrorLabel, string errorText)
        {
            currentErrorLabel.Visible = true;
            currentErrorLabel.Text = errorText;
        }

        private bool IsEmailValid(string emailAddress)
        {
            try
            {
                MailAddress checkMailAdress = new MailAddress(emailAddress);
                return checkMailAdress.Address == emailAddress;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private void TextboxKeyUp(KeyEventArgs e, Panel currentMainPanel, TextBox currentTextbox, Panel passwordMayusLock)
        {
            if (e.KeyCode == Keys.Enter)
            {
                currentMainPanel.Focus();
            }

            if (passwordMayusLock != null)
            {
                if (Control.IsKeyLocked(Keys.CapsLock))
                {
                    passwordMayusLock.Visible = true;
                    currentTextbox.Size = new Size(165, 22);
                }
                else if (!Control.IsKeyLocked(Keys.CapsLock))
                {
                    passwordMayusLock.Visible = false;
                    currentTextbox.Size = new Size(195, 22);
                }
            }
        }

        private void CheckTextboxPasswordStrength(TextBox currentTextbox, Label currentErrorLabel, ProgressBar passwordStrength,
            Label passwordStrengthLabel)
        {
            currentErrorLabel.Visible = false;
            passwordStrength.Visible = true;
            passwordStrengthLabel.Visible = true;

            PasswordStrength.CheckPasswordStrength(currentTextbox, passwordStrength, passwordStrengthLabel);
        }

        #endregion Textboxes / Email / Password comprobations

        #region Others
        // Miscellany of methods and functions for various things
        #region Lose Focus
        // These methods lose focus to any box  -- Hace que todas las cajas de texto pierdan el foco
        private void OrdinaryFocusLose(object sender, EventArgs e)
        {
            BackgroundPanel.Focus();
        }

        private void EventFocusLose(object sender, EventArgs e)
        {
            ImagePanel.Focus();
        }
        #endregion

        #region Show and Hide Image Gradient Panel
        // Methods to show Image Gradient Panel
        private void ShowImageGradient()
        {
            BackgroundPanel.Visible = false;
            ConfigurationButton.Visible = false;
            ConfigurationPanel.Visible = false;
            EventsPanel.Visible = false;
            ImageGradient.Visible = true;

            LoginPanel.Visible = false;
            RegisterPanel.Visible = false;
            LoginProblemsPanel.Visible = false;
        }

        private void HideImageGradient()
        {
            BackgroundPanel.Visible = true;
            ConfigurationButton.Visible = true;
            ConfigurationPanel.Visible = false;
            EventsPanel.Visible = false;
            ImageGradient.Visible = false;
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
        #endregion

        #region Show Hide Password
        // These methods are used to show and hide the password in password textboxes
        private void ShowPassword_Click(object sender, EventArgs e)
        {
            Show_HidePassword(ref passwordVisible);
        }

        private void ShowConfirmPassword_Click(object sender, EventArgs e)
        {
            Show_HidePassword(ref confirmPasswordVisible);
        }

        private void ShowResetPassword_Click(object sender, EventArgs e)
        {
            Show_HidePassword(ref confirmPasswordVisible);
        }

        private void Show_HidePassword(ref bool currentPasswordVisibleIndicator)
        {
            currentPasswordVisibleIndicator = currentPasswordVisibleIndicator ? false : true;

            if (!currentPasswordVisibleIndicator)
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

        #region Enable / Disable Access button
        private void Login_RegisterButton(bool buttonEnabled, Panel login_RegisterButton, ref bool currentActionState)
        {
            if (buttonEnabled)
            {
                login_RegisterButton.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.loginSuccessful;
                login_RegisterButton.Cursor = System.Windows.Forms.Cursors.Hand;
                currentActionState = true;
            }
            else
            {
                login_RegisterButton.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.loginUnavaible;
                login_RegisterButton.Cursor = System.Windows.Forms.Cursors.Default;
                currentActionState = false;
            }
        }
        #endregion

        #endregion Others
    }
}