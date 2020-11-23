using Outlands_Adventure_Launcher.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Net.Mail;
using System.Resources;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace Outlands_Adventure_Launcher
{
    public partial class ClientLoginSmall : Form
    {
        private ClientLoginSmall clientLogin;
        private WindowsRegisterManager windowsRegisterManager;

        private string targetPanel;
        private bool operationInProgress;

        private bool loginAvaible;
        private bool loginErrors;

        private bool registerAvaible;
        private bool registerErrors;

        private bool loginProblemsAvaible;
        private bool loginProblemsErrors;
        private bool usernameLost;
        private bool passwordLost;

        private Panel currentShowPasswordButton;
        private TextBox currentPasswordTextbox;
        private bool passwordVisible;
        private bool confirmPasswordVisible;


        public ClientLoginSmall()
        {
            InitializeComponent();
        }

        public void ReceiveClassInstance(ClientLoginSmall clientLogin)
        {
            this.clientLogin = clientLogin;
        }

        #region Form Actions
        private void LauncherLogin_Load(object sender, EventArgs e)
        {            
            windowsRegisterManager = new WindowsRegisterManager();
            windowsRegisterManager.LoadWindowPosition(this);

            LanguageManager languageManager = new LanguageManager();
            languageManager.SelectCurrentAplicationWindow(null, null, clientLogin, null);
            languageManager.ReadSelectedLanguage(true, LanguageSelected);

            targetPanel = "login";
            operationInProgress = false;

            loginAvaible = false;
            loginErrors = false;

            registerAvaible = false;
            registerErrors = false;

            loginProblemsAvaible = false;
            loginProblemsErrors = false;

            passwordVisible = false;
            confirmPasswordVisible = false;

            ImageGradient.BackColor = Color.FromArgb(190, 0, 0, 0);
            ConfigurationPanel.BackColor = Color.FromArgb(255, 0, 0, 0);
        }

        private void LauncherLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (operationInProgress)
            {
                e.Cancel = true;
            }
            else
            {
                windowsRegisterManager.SaveWindowPosition(this);

                Environment.Exit(0);
            }
        }
        #endregion Form Actions

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
            if (e.KeyCode == Keys.Enter)
            {
                LoginButton_Click(null, EventArgs.Empty);
            }
            else
            {
                TextboxKeyUp(e, LoginPanel, null, null);
            }
        }

        private void PasswordTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoginButton_Click(null, EventArgs.Empty);
            }
            else
            {
                TextboxKeyUp(e, LoginPanel, PasswordTextbox, LoginMayusLock);
            }
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
                RememberMe.Font = new Font("Oxygen", 8, FontStyle.Bold);
            }
            else
            {
                RememberMe.Font = new Font("Oxygen", 8, FontStyle.Regular);
            }
        }
        #endregion

        #region Login in
        // This method manage login button when you click on it
        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (loginAvaible)
            {
                loginErrors = false;
                LoginButton.Focus();
                CheckLoginCredentials();

                if (loginAvaible && !loginErrors)
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

                    if (RememberMe.Checked)
                    {
                        Microsoft.Win32.RegistryKey key = windowsRegisterManager.OpenWindowsRegister(true);
                        key.SetValue("KeepSessionOpen", RememberMe.Checked);
                        key.SetValue("Username", UserNameTextbox.Text);
                    }

                    ResolutionManager resolutionManager = new ResolutionManager();
                    resolutionManager.LoadWindowResolution(false, null, null, clientLogin, null, UserNameTextbox.Text);
                }
            }
            else
            {
                LoginButton.Focus();
            }
        }

        private void CheckLoginCredentials()
        {
            OpenLoadingScreen(true);
            string sqlQuery = "SELECT COUNT(*) FROM user_information WHERE user_name LIKE '" + UserNameTextbox.Text + "' &&" +
                " user_password = SHA('" + PasswordTextbox.Text + "')";
            int rowsRecovered = SQLManager.CheckDuplicatedData(sqlQuery);

            if (rowsRecovered > 0)
            {
                loginAvaible = true;
                WrongCredentials.Visible = false;
                CloseLoadingScreen(true, "login");
            }
            else if (rowsRecovered == 0)
            {
                Login_RegisterButton(false, LoginButton, ref loginAvaible);
                WrongCredentials.Visible = true;
                loginErrors = true;
                CloseLoadingScreen(true, "login");
            }
            else
            {
                CloseLoadingScreen(false, "login");
                GenericPopUpMessage(LanguageResx.ClientLanguage.events_Database_ConnectionError);
                loginErrors = true;
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
                        OpenLoadingScreen(true);
                        string confirmationCode = CreateConfirmationCode.CreateCode();
                        Hash_SHA2.InitialiceVariables(confirmationCode);

                        string[] messageInfo = LanguageResx.ClientLanguage.sendEmail_NewAccount.Split('*');
                        bool messageError = SendEmail.SendNewEmail(NewEmailTextbox.Text, messageInfo[0], messageInfo[1], confirmationCode);

                        if (!messageError)
                        {
                            EventText.Location = new Point(40, 5);
                            EventCode.Visible = true;
                            EventSendButton.Visible = true;
                            EventExitButton.Location = new Point(305, EventSendButton.Location.Y);

                            EventText.Text = LanguageResx.ClientLanguage.events_Header_NewAccount;

                            targetPanel = "login";
                            registerErrors = false;

                            CloseLoadingScreen(false, "register");
                            EventsPanel.Visible = true;
                        }
                        else
                        {
                            CloseLoadingScreen(false, "register");
                            registerErrors = true;
                            GenericPopUpMessage(LanguageResx.ClientLanguage.events_SendEmailError);
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
            OpenLoadingScreen(true);
            registerErrors = false;
            string sqlQuery = "SELECT COUNT(*) FROM user_information WHERE user_email LIKE '" + NewEmailTextbox.Text + "'";
            int emailRowsRecovered = SQLManager.CheckDuplicatedData(sqlQuery);

            sqlQuery = "SELECT COUNT(*) FROM user_information WHERE user_name LIKE '" + NewUserNameTextbox.Text + "'";
            int nameRowsRecovered = SQLManager.CheckDuplicatedData(sqlQuery);

            if (emailRowsRecovered > -1 && nameRowsRecovered > -1)
            {
                if (emailRowsRecovered > 0)
                {
                    registerErrors = true;
                    GenericError(NewEmailErrorLabel, LanguageResx.ClientLanguage.textboxError_EmailRegistered);
                    Login_RegisterButton(false, RegisterButton, ref registerAvaible);
                    CloseLoadingScreen(true, "register");
                }
                else
                {
                    NewEmailErrorLabel.Visible = false;
                }

                if (nameRowsRecovered > 0)
                {
                    registerErrors = true;
                    GenericError(NewUserNameErrorLabel, LanguageResx.ClientLanguage.textboxError_UsernameRegistered);
                    Login_RegisterButton(false, RegisterButton, ref registerAvaible);
                    CloseLoadingScreen(true, "register");
                }
                else
                {
                    NewUserNameErrorLabel.Visible = false;
                }
            }
            else
            {
                CloseLoadingScreen(false, "register");
                registerErrors = true;
                GenericPopUpMessage(LanguageResx.ClientLanguage.events_Database_ConnectionError);
            }
        }

        private void RegisterNewUser()
        {
            OpenLoadingScreen(false);
            string sqlQuery = "INSERT INTO user_information VALUES ('" + NewEmailTextbox.Text + "', '"
                + NewUserNameTextbox.Text + "', SHA('" + NewPasswordTextbox.Text + "'))";
            string queryError = SQLManager.Insert_ModifyData(sqlQuery);

            if (queryError.Length > 0)
            {
                if (queryError.Contains("Unable to connect"))
                {
                    // Pop up de falta de internet - No te puedes conectar a la base de datos
                    GenericPopUpMessage(LanguageResx.ClientLanguage.events_Database_ConnectionError);
                }

                else
                {
                    // Cualquier otro tipo de error de la base de datos que tendra que salir en el pop up
                    GenericPopUpMessage(queryError);
                }

                registerErrors = true;
            }

            CloseLoadingScreen(false, "register");
        }
        #endregion

        #endregion Register Interface

        #region Login Problems Interface
        // Manage all items in login problems panel

        #region Forgotten Username / Password Button
        private void ForgottenUsernameButton_MouseEnter(object sender, EventArgs e)
        {
            ForgottenUsernameButton.BackColor = Color.FromArgb(25, 0, 203, 255);
        }

        private void ForgottenUsernameButton_MouseLeave(object sender, EventArgs e)
        {
            ForgottenUsernameButton.BackColor = Color.FromArgb(0, 0, 203, 255);
        }

        private void ForgottenPasswordButton_MouseEnter(object sender, EventArgs e)
        {
            ForgottenPasswordButton.BackColor = Color.FromArgb(25, 0, 203, 255);
        }

        private void ForgottenPasswordButton_MouseLeave(object sender, EventArgs e)
        {
            ForgottenPasswordButton.BackColor = Color.FromArgb(0, 0, 203, 255);
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

                ResetCredentialsHeader.Text = LanguageResx.ClientLanguage.loginProblems_LostUsername;
            }
            else
            {
                usernameLost = false;
                passwordLost = true;

                ResetCredentialsHeader.Text = LanguageResx.ClientLanguage.loginProblems_LostPassword;
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
        private void ResetCredentialsEmailText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ResetCredentialsButton_Click(null, EventArgs.Empty);
            }
        }

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
                OpenLoadingScreen(true);
                CheckLoginProblemsEmail();

                if (!loginProblemsErrors)
                {
                    bool messageError = false;

                    if (usernameLost)
                    {
                        EventText.Location = new Point(40, 25);
                        EventCode.Visible = false;
                        EventSendButton.Visible = false;
                        EventExitButton.Location = new Point(237, 105);

                        // Buscar en firebase para ver si existe el correo, si existe, mandar dicho correo
                        EventText.Text = LanguageResx.ClientLanguage.events_Header_UsernameLost;

                        if (loginProblemsAvaible)
                        {
                            string userNameRecovered = CheckUserName();

                            if (userNameRecovered.Length > 0)
                            {
                                string[] messageInfo = LanguageResx.ClientLanguage.sendEmail_LostUsername.Split('*');
                                messageError = SendEmail.SendNewEmail(ResetCredentialsEmailText.Text, messageInfo[0], messageInfo[1],
                                    userNameRecovered);
                            }
                        }
                    }
                    else if (passwordLost)
                    {
                        EventText.Location = new Point(40, 9);
                        EventCode.Visible = true;
                        EventSendButton.Visible = true;
                        EventExitButton.Location = new Point(305, EventSendButton.Location.Y);

                        // Buscar en firebase para ver si existe el correo, si existe, mandar dicho correo
                        EventText.Text = LanguageResx.ClientLanguage.events_Header_PasswordLost;

                        if (loginProblemsAvaible)
                        {
                            string confirmationCode = CreateConfirmationCode.CreateCode();
                            Hash_SHA2.InitialiceVariables(confirmationCode);
                            string[] messageInfo = LanguageResx.ClientLanguage.sendEmail_LostPassword.Split('*');
                            messageError = SendEmail.SendNewEmail(ResetCredentialsEmailText.Text, messageInfo[0], messageInfo[1], confirmationCode);
                        }
                    }

                    if (messageError)
                    {
                        loginProblemsErrors = true;
                        EventsPanel.Visible = true;
                        CloseLoadingScreen(false, "loginProblems");
                        GenericPopUpMessage(LanguageResx.ClientLanguage.events_SendEmailError);
                    }
                    else
                    {
                        targetPanel = "login";
                        CloseLoadingScreen(false, "loginProblems");
                        EventsPanel.Visible = true;
                    }
                }
            }
            else
            {
                ResetCredentialsButton.Focus();
            }
        }

        private void CheckLoginProblemsEmail()
        {
            string sqlQuery = "SELECT COUNT(*) FROM user_information WHERE user_email LIKE '" + ResetCredentialsEmailText.Text + "'";
            int emailRowsRecovered = SQLManager.CheckDuplicatedData(sqlQuery);

            if (emailRowsRecovered > -1)
            {
                if (emailRowsRecovered == 0)
                {
                    Login_RegisterButton(false, ResetCredentialsButton, ref loginProblemsAvaible);
                }
            }
            else
            {
                loginProblemsErrors = true;
                CloseLoadingScreen(false, "loginProblems");
                GenericPopUpMessage(LanguageResx.ClientLanguage.events_Database_ConnectionError);
            }
        }

        private string CheckUserName()
        {
            string sqlQuery = "SELECT user_name FROM user_information WHERE user_email LIKE '" + ResetCredentialsEmailText.Text + "'";
            string userNameRecovered = SQLManager.SearchQueryData(sqlQuery);

            if (!userNameRecovered.Equals("error"))
            {
                if (userNameRecovered.Length > 0)
                {
                    return userNameRecovered;
                }

                return "";
            }
            else
            {
                loginProblemsErrors = true;
                CloseLoadingScreen(false, "loginProblems");
                GenericPopUpMessage(LanguageResx.ClientLanguage.events_Database_ConnectionError);

                return "";
            }
        }
        #endregion

        #endregion Login Problems Interface

        #region Game Client Configuration
        // These methods manage game client configuration button when you click on it
        private void Configuration_Click(object sender, EventArgs e)
        {
            ConfigurationButton.Focus();
            ShowImageGradient();
            ConfigurationPanel.Visible = true;

            LanguageManager languageManager = new LanguageManager();
            languageManager.ReadSelectedLanguage(false, LanguageSelected);

            ResolutionManager resolutionManager = new ResolutionManager();
            resolutionManager.ReadSelectedResolution(ResolutionSelected);
        }

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
        #endregion Configuration Exit Button

        #region Configuration Refresh Resolution
        private void ResolutionRefresh_MouseEnter(object sender, EventArgs e)
        {
            MultipleResources.ShowToolTip(ResolutionRefresh, LanguageResx.ClientLanguage.RefreshResolution_Tooltip);
        }

        private void ResolutionRefresh_MouseLeave(object sender, EventArgs e)
        {
            MultipleResources.HideToolTip(ResolutionRefresh);
        }

        private void ResolutionRefresh_MouseClick(object sender, MouseEventArgs e)
        {
            ResolutionManager resolutionManager = new ResolutionManager();
            resolutionManager.ResolutionCombobox_ResolutionChanged(ResolutionSelected);
            resolutionManager.LoadWindowResolution(true, null, null, clientLogin, null, "");
        }
        #endregion Configuration Refresh Resolution

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
            if (usernameLost && !loginProblemsErrors)
            {
                ResetLoginProblemsPanelValues();
            }

            if (registerErrors)
            {
                targetPanel = "register";
                registerErrors = false;
            }
            else if (loginProblemsErrors || passwordLost)
            {
                targetPanel = "loginProblems";
                loginProblemsErrors = false;
            }
            else if (loginErrors)
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
                        GenericPopUpMessage(LanguageResx.ClientLanguage.registerSucessful);
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

        #region Reset Password Textbox
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
        #endregion Reset Password Textbox

        #region Send and Exit Button
        private void ResetPasswordSendButton_Click(object sender, EventArgs e)
        {
            CheckRegister_ResetTexboxErrors(ResetPasswordTextbox);

            if (!ResetPasswordEventErrorText.Visible)
            {
                ChangeUserPassword();
                if (!loginProblemsErrors)
                {
                    passwordLost = false;
                    loginProblemsErrors = false;

                    targetPanel = "login";
                    GenericPopUpMessage(LanguageResx.ClientLanguage.newPasswordSucessful);
                    ResetPasswordEventPanel.Visible = false;
                    Reset_ResetPasswordEventValues();
                    ResetLoginProblemsPanelValues();
                }
            }
        }

        private void ResetPasswordExitButton_Click(object sender, EventArgs e)
        {
            targetPanel = "loginProblems";
            loginProblemsErrors = false;
            CheckTargetPanel();
            Reset_ResetPasswordEventValues();
            ResetPasswordEventPanel.Visible = false;
        }
        #endregion Send and Exit Button

        private void ChangeUserPassword()
        {
            OpenLoadingScreen(false);
            string sqlQuery = "UPDATE user_information SET user_password=SHA('" + ResetPasswordTextbox.Text + "')" +
                " WHERE user_email LIKE '" + ResetCredentialsEmailText.Text + "'";
            string queryError = SQLManager.Insert_ModifyData(sqlQuery);

            if (queryError.Length > 0)
            {
                if (queryError.Contains("Unable to connect"))
                {
                    // Pop up de falta de internet - No te puedes conectar a la base de datos
                    GenericPopUpMessage(LanguageResx.ClientLanguage.events_Database_ConnectionError);
                }

                else
                {
                    // Cualquier otro tipo de error de la base de datos que tendra que salir en el pop up
                    GenericPopUpMessage(queryError);
                }
                loginProblemsErrors = true;
            }

            CloseLoadingScreen(false, "loginProblems");
        }

        #endregion Reset Password Event

        private void GenericPopUpMessage(string eventText)
        {
            EventText.Location = new Point(23, 25);
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
            ((Label)sender).Font = new Font("Oxygen", 8, FontStyle.Bold);
        }

        private void ActionLabel_MouseLeave(object sender, EventArgs e)
        {
            ((Label)sender).Font = new Font("Oxygen", 8, FontStyle.Regular);
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
                currentTextboxLabel.Font = new Font("Perpetua Titling MT", 5, FontStyle.Bold);
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
                    currentTextbox.Size = new Size(150, 14);
                }
                else
                {
                    mayusLock.Visible = false;
                    currentTextbox.Size = new Size(179, 14);
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
                        LessFourLetters(NewEmailErrorLabel, LanguageResx.ClientLanguage.textboxError_LessFourLetters, null, null);

                    else if (!IsEmailValid(NewEmailTextbox.Text))
                        GenericError(NewEmailErrorLabel, LanguageResx.ClientLanguage.textboxError_WrongEmail);

                    else
                        NewEmailErrorLabel.Visible = false;
                    break;

                case "NewUserNameTextbox":
                    if (NewUserNameTextbox.Text.Length == 0)
                        EmptyTextbox(NewUserNameLabel, NewUserNameErrorLabel, null, null, null, null);

                    else if (NewUserNameTextbox.Text.Length < 4)
                        LessFourLetters(NewUserNameErrorLabel, LanguageResx.ClientLanguage.textboxError_LessFourLetters, null, null);

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
                        LessFourLetters(NewPasswordErrorLabel, LanguageResx.ClientLanguage.textboxError_LessFourLetters,
                            NewPasswordStrengthProgressBar, NewPasswordStrengthLabel);
                    }
                    else
                    {
                        NewPasswordErrorLabel.Visible = false;
                    }

                    if (!NewPasswordTextbox.Text.Equals(ConfirmNewPasswordTextbox.Text) && ConfirmNewPasswordTextbox.Text.Length > 0)
                    {
                        GenericError(ConfirmNewPasswordErrorLabel, LanguageResx.ClientLanguage.textboxError_PasswordNotMatch);
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
                        GenericError(ConfirmNewPasswordErrorLabel, LanguageResx.ClientLanguage.textboxError_PasswordNotMatch);
                    }
                    else if (ConfirmNewPasswordTextbox.Text.Length < 4)
                    {
                        LessFourLetters(ConfirmNewPasswordErrorLabel, LanguageResx.ClientLanguage.textboxError_LessFourLetters, null, null);
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
                        LessFourLetters(ResetPasswordEventErrorText, LanguageResx.ClientLanguage.textboxError_LessFourLetters,
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
            currentTextboxLabel.Font = new Font("Oxygen", 8);
            currentTextboxLabel.Location = new Point(10, 8);
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
                    currentTextbox.Size = new Size(150, 14);
                }
                else if (!Control.IsKeyLocked(Keys.CapsLock))
                {
                    passwordMayusLock.Visible = false;
                    currentTextbox.Size = new Size(179, 14);
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
            ResetPasswordEventPanel.Visible = false;
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
            ResetPasswordEventPanel.Visible = false;
            ImageGradient.Visible = false;
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

        #region Enable / Disable Loading Screen
        private void OpenLoadingScreen(bool showImageGradient)
        {
            if (showImageGradient) ShowImageGradient();
            ImageGradient.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            operationInProgress = true;
        }

        private void CloseLoadingScreen(bool hideImageGradient, string targetPanel)
        {
            ImageGradient.Cursor = System.Windows.Forms.Cursors.Default;

            if (hideImageGradient)
            {
                this.targetPanel = targetPanel;
                CheckTargetPanel();
            }

            operationInProgress = false;
        }
        #endregion Enable / Disable Loading Screen

        #endregion Others

        #region Combobox controls and Language manager

        #region Combobox controls
        // Allow Combo Box to center aligned
        private void Language_ResolutionSelected_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboboxManager languageComboboxManager = new ComboboxManager();
            languageComboboxManager.Combobox_DrawItem(sender, e);
        }

        private void Language_ResolutionSelected_DropDownClosed(object sender, EventArgs e)
        {
            ComboboxManager languageComboboxManager = new ComboboxManager();
            languageComboboxManager.Combobox_DropDownClosed(ConfigurationPanel);
        }
        #endregion Combobox controls

        #region Language manager
        /// <summary>
        /// Cambia el idioma de todos los objetos que hay en la aplicación
        /// </summary>
        public void ChangeAplicationLanguage()
        {
            // Login Interface
            LoginText.Text = LanguageResx.ClientLanguage.login_Header;
            WrongCredentials.Text = LanguageResx.ClientLanguage.login_Error;
            UserNameLabel.Text = LanguageResx.ClientLanguage.textbox_Username;
            PasswordLabel.Text = LanguageResx.ClientLanguage.textbox_Password;
            RememberMe.Text = LanguageResx.ClientLanguage.login_RememberMe;
            RegisterLabel.Text = LanguageResx.ClientLanguage.login_Register;
            LoginProblems.Text = LanguageResx.ClientLanguage.login_LoginProblems;

            // Register Interface
            RegisterText.Text = LanguageResx.ClientLanguage.register_Header;
            NewEmailLabel.Text = LanguageResx.ClientLanguage.textbox_Email;
            NewUserNameLabel.Text = LanguageResx.ClientLanguage.textbox_Username;
            NewPasswordLabel.Text = LanguageResx.ClientLanguage.textbox_Password;
            ConfirmNewPasswordLabel.Text = LanguageResx.ClientLanguage.textbox_ConfirmPassword;
            LoginLabel.Text = LanguageResx.ClientLanguage.register_Login;

            // Login Problems Interface
            LoginProblemsHeader_1.Text = LanguageResx.ClientLanguage.loginProblems_Header;
            LoginProblemsHeader_2.Text = LanguageResx.ClientLanguage.loginProblems_Header2;
            ForgottenUsernameHeader.Text = LanguageResx.ClientLanguage.loginProblems_UsernameHeader;
            ForgottenPasswordHeader.Text = LanguageResx.ClientLanguage.loginProblems_PasswordHeader;
            if (usernameLost) ResetCredentialsHeader.Text = LanguageResx.ClientLanguage.loginProblems_LostUsername;
            else if (passwordLost) ResetCredentialsHeader.Text = LanguageResx.ClientLanguage.loginProblems_LostPassword;
            ResetCredentialsEmailLabel.Text = LanguageResx.ClientLanguage.textbox_Email;
            ReturnToLogin.Text = LanguageResx.ClientLanguage.loginProblems_Login;

            // Events
            EventSendButton.Text = LanguageResx.ClientLanguage.button_Confirm;
            EventExitButton.Text = LanguageResx.ClientLanguage.button_Close_Uppercase;
            ResetPasswordSendButton.Text = LanguageResx.ClientLanguage.button_Confirm;
            ResetPasswordExitButton.Text = LanguageResx.ClientLanguage.button_Close_Uppercase;

            // Settings
            ConfigurationHeader.Text = LanguageResx.ClientLanguage.settings_Header;
            ClientLanguageHeader.Text = LanguageResx.ClientLanguage.settings_LanguageHeader;
            ResolutionHeader.Text = LanguageResx.ClientLanguage.settings_ResolutionHeader;
            ConfigurationExitButton.Text = LanguageResx.ClientLanguage.button_Close_Uppercase;

            LanguageSelected.Items.Clear();
            string[] languagesAvaibles = LanguageResx.ClientLanguage.settings_Languages.Split('*');
            foreach (string currentLanguage in languagesAvaibles)
            {
                LanguageSelected.Items.Add(currentLanguage);
            }

            ResolutionSelected.Items.Clear();
            string[] resolutionsAvaibles = LanguageResx.ClientLanguage.settings_resolution.Split('*');
            foreach (string currentResolution in resolutionsAvaibles)
            {
                ResolutionSelected.Items.Add(currentResolution);
            }
            ResolutionManager resolutionManager = new ResolutionManager();
            resolutionManager.ReadSelectedResolution(ResolutionSelected);

            if (NewEmailTextbox.Text.Length > 0)
            {
                CheckRegister_ResetTexboxErrors(NewEmailTextbox);
            }

            if (NewUserNameTextbox.Text.Length > 0)
            {
                CheckRegister_ResetTexboxErrors(NewUserNameTextbox);
            }

            if (NewPasswordTextbox.Text.Length > 0)
            {
                CheckRegister_ResetTexboxErrors(NewEmailTextbox);
                PasswordStrength.CheckPasswordStrength(NewPasswordTextbox, NewPasswordStrengthProgressBar, NewPasswordStrengthLabel);
            }

            if (ConfirmNewPasswordTextbox.Text.Length > 0)
            {
                CheckRegister_ResetTexboxErrors(ConfirmNewPasswordTextbox);
            }
        }

        /// <summary>
        /// Cambia el idioma del almacenado en la entrada del registro de windows y cambia el idioma de la aplicación
        /// </summary>
        /// <param name="sender">Objeto que recibe los eventos</param>
        /// <param name="e">Eventos que le ocurren al objeto</param>
        private void LanguageSelected_SelectionChangeCommitted(object sender, EventArgs e)
        {
            LanguageManager languageManager = new LanguageManager();
            languageManager.SelectCurrentAplicationWindow(null, null, clientLogin, null);
            languageManager.LanguageCombobox_LanguageChanged(LanguageSelected);
        }
        #endregion Language manager

        #endregion Combobox controls and Language manager
    }
}