using CG.Web.MegaApiClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

// Cuando cambies de idioma vete al registro de windows y cambia el texto de DefaultScreen antes de cerrar el registro de windows

namespace Outlands_Adventure_Launcher
{
    public partial class ClientAplication : Form
    {
        private ClientAplication clientAplication;
        private string userEmail;

        private string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Outlands Adventure Client");
        private string downloadGameName = "";
        private string downloadState = "";
        private bool downloadInProgress;
        private bool uninstallInProgress;

        private readonly int objectHighlighted = 30;
        private readonly int objectUnmarked = 0;
        private readonly int objectSelected = 50;
        private bool storeOpen, libraryOpen, gameInfoOpen;

        private bool operationInProgress;
        private bool canTriggerSelections;

        private ListView currentGameImagesListView;
        List<GameInfo> currentGameInfoList;
        GameInfo currentGameInfo;

        private ToolTip toolTip;
        private string tileSizeSelected = "";

        public ClientAplication()
        {
            InitializeComponent();
        }

        public void ReceiveClassInstance(ClientAplication clientAplication)
        {
            this.clientAplication = clientAplication;
            userEmail = "thenapo212@gmail.com"; // cambiar por nombre de usuario obtenido cuando te logeas
        }

        #region Form Actions
        private void ClientAplication_Load(object sender, EventArgs e)
        {
            CreateClientFolder();

            WindowsRegisterManager windowsRegisterManager = new WindowsRegisterManager();
            windowsRegisterManager.LoadWindowPosition(this);

            LanguageManager languageManager = new LanguageManager();
            languageManager.SelectCurrentAplicationWindow(null, clientAplication);
            languageManager.ReadSelectedLanguage(true, LanguageSelected);

            currentGameInfoList = new List<GameInfo>();
            ImageGradient.BackColor = Color.FromArgb(190, 0, 0, 0);
            GameInfoGradient.BackColor = Color.FromArgb(210, 0, 0, 0);
            toolTip = new ToolTip();

            operationInProgress = false;
            canTriggerSelections = true;

            // Comprobar si el panel de inicio establecido no es la información, si no es la información del usuario hacer el metodo 1
            SelectTileSize(true, false, false); // Metodo 1
            SetDefaultScreen();

            DownloadProgress.MarqueeAnimationSpeed = 40;
            UninstallProgress.MarqueeAnimationSpeed = 40;
        }

        private void ClientAplication_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (operationInProgress)
            {
                e.Cancel = true;
            }
            else
            {
                WindowsRegisterManager windowsRegisterManager = new WindowsRegisterManager();
                windowsRegisterManager.SaveWindowPosition(this);

                Microsoft.Win32.RegistryKey key = windowsRegisterManager.OpenWindowsRegister(true);
                key.SetValue("selectedTileSize", tileSizeSelected);

                windowsRegisterManager.CloseWindowsRegister(key);
            }
        }
        #endregion Form Actions

        #region Side Menu Manager
        private void ChangeBackgroundColor(Object backgroundPanel, int colorOpacity)
        {
            if (backgroundPanel.GetType() == typeof(Panel))
            {
                ((Panel)backgroundPanel).BackColor = Color.FromArgb(colorOpacity, 255, 255, 255);
            }
            else
            {
                ((Label)backgroundPanel).BackColor = Color.FromArgb(colorOpacity, 255, 255, 255);
            }
        }

        #region User Information
        private void UserInformation_MouseEnter(object sender, EventArgs e)
        {
            if (canTriggerSelections)
            {
                ChangeBackgroundColor(UserInformation, objectHighlighted);
                ShowUserInfoMenu();
                canTriggerSelections = false;
            }
        }

        private void UserInformation_MouseLeave(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(Panel))
            {
                if ((Panel)sender == UserInformation)
                {
                    if (!MouseIsOverControl(UserName) && !MouseIsOverControl(UserConfigurationArrow) &&
                        !MouseIsOverControl(UserPhoto) && !MouseIsOverControl(GameInfoMenu))
                    {
                        ChangeBackgroundColor(UserInformation, objectUnmarked);
                        HideUserInfoMenu();
                        canTriggerSelections = true;
                    }
                }
            }
        }
        #endregion User Information

        #region Store
        private void Store_MouseEnter(object sender, EventArgs e)
        {
            if (canTriggerSelections)
            {
                ChangeBackgroundColor(Store, objectHighlighted);
                canTriggerSelections = false;
            }
        }

        private void Store_MouseLeave(object sender, EventArgs e)
        {
            if (sender != null && sender.GetType() == typeof(Panel))
            {
                if ((Panel)sender == Store)
                {
                    if (!MouseIsOverControl(StoreLabel))
                    {
                        if (!storeOpen) ChangeBackgroundColor(Store, objectUnmarked);
                        else ChangeBackgroundColor(Store, objectSelected);

                        canTriggerSelections = true;
                    }
                }
            }
        }

        private void Store_MouseDown(object sender, MouseEventArgs e)
        {
            if (e == null || e.Button == MouseButtons.Left)
            {
                if (!storeOpen)
                {
                    Store.Focus();
                    ChangeScreen(ref storeOpen, false);

                    ChangeBackgroundColor(Store, objectSelected);
                    ChangeBackgroundColor(GameLibrary, objectUnmarked);

                    currentGameImagesListView = StoreAvailableGames;
                    SelectTileSize(false, false, true);
                }

                Store.Invalidate();
            }
        }

        private void Store_Paint(object sender, PaintEventArgs e)
        {
            if (storeOpen)
            {
                Pen orangePen = new Pen(Color.FromArgb(255, 102, 0), 4);
                Point p1 = new Point(2, 0);
                Point p2 = new Point(2, Store.Height);
                e.Graphics.DrawLine(orangePen, p1, p2);
                orangePen.Dispose();
            }
        }
        #endregion Store

        #region Game Library
        private void GameLibrary_MouseEnter(object sender, EventArgs e)
        {
            if (canTriggerSelections)
            {
                ChangeBackgroundColor(GameLibrary, objectHighlighted);
                canTriggerSelections = false;
            }
        }

        private void GameLibrary_MouseLeave(object sender, EventArgs e)
        {
            if (sender != null && sender.GetType() == typeof(Panel))
            {
                if ((Panel)sender == GameLibrary)
                {
                    if (!MouseIsOverControl(GameLibraryLabel))
                    {
                        if (!libraryOpen) ChangeBackgroundColor(GameLibrary, objectUnmarked);
                        else ChangeBackgroundColor(GameLibrary, objectSelected);

                        canTriggerSelections = true;
                    }
                }
            }
        }

        private void GameLibrary_MouseDown(object sender, MouseEventArgs e)
        {
            if (e == null || e.Button == MouseButtons.Left)
            {
                if (!libraryOpen)
                {
                    GameLibrary.Focus();
                    ChangeScreen(ref libraryOpen, false);

                    ChangeBackgroundColor(GameLibrary, objectSelected);
                    ChangeBackgroundColor(Store, objectUnmarked);

                    currentGameImagesListView = GameLibraryAvailableGames;
                    SelectTileSize(false, false, true);
                }

                GameLibrary.Invalidate();
            }
        }

        private void GameLibrary_Paint(object sender, PaintEventArgs e)
        {
            if (libraryOpen)
            {
                Pen orangePen = new Pen(Color.FromArgb(255, 102, 0), 4);
                Point p1 = new Point(2, 0);
                Point p2 = new Point(2, GameLibrary.Height);
                e.Graphics.DrawLine(orangePen, p1, p2);
                orangePen.Dispose();
            }
        }
        #endregion Game Library

        #endregion Side Menu Manager

        #region Game Client Configuration
        // These methods manage game client configuration button when you click on it
        private void OpenConfiguration()
        {
            ShowImageGradient();
            ConfigurationPanel.Visible = true;

            FilterGame.Text = "";

            LanguageManager languageManager = new LanguageManager();
            languageManager.ReadSelectedLanguage(false, LanguageSelected);

            WindowsRegisterManager windowsRegisterManager = new WindowsRegisterManager();
            Microsoft.Win32.RegistryKey key = windowsRegisterManager.OpenWindowsRegister(false);
            string selectedDefaultScreen = (string)key.GetValue("selectedDefaultScreen");

            DefaultScreen.SelectedItem = selectedDefaultScreen;
            windowsRegisterManager.CloseWindowsRegister(key);

            if (DefaultScreen.SelectedItem == null || DefaultScreen.SelectedIndex == -1)
                DefaultScreen.SelectedIndex = 0;

            ConfigurationPanel.Focus();
        }

        private void DeleteAccount_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                OpenLoadingScreen();
                string confirmationCode = CreateConfirmationCode.CreateCode();
                Hash_SHA2.InitialiceVariables(confirmationCode);

                string[] messageInfo = ClientLanguage.sendEmail_DeleteAccount.Split('*');
                bool messageError = SendEmail.SendNewEmail(userEmail, messageInfo[0], messageInfo[1], confirmationCode);

                if (!messageError)
                {
                    EventText.Location = new Point(20, 10);
                    EventCode.Visible = true;
                    EventSendButton.Visible = true;
                    EventExitButton.Location = new Point(305, EventSendButton.Location.Y);

                    EventText.Text = ClientLanguage.events_Header_NewAccount;

                    EventsPanel.Visible = true;
                }
                else
                {
                    GenericPopUpMessage(ClientLanguage.events_SendEmailError);
                }

                CloseLoadingScreen(false);
            }
        }

        private void DeleteAccount_MouseEnter(object sender, EventArgs e)
        {
            ((Label)sender).Font = new Font("Oxygen", 12, FontStyle.Bold);
        }

        private void DeleteAccount_MouseLeave(object sender, EventArgs e)
        {
            ((Label)sender).Font = new Font("Oxygen", 12, FontStyle.Regular);
        }

        private void ConfigurationExitButton_Click(object sender, EventArgs e)
        {
            HideImageGradient();
        }

        private void ConfigurationExitButton_MouseEnter(object sender, EventArgs e)
        {
            ConfigurationExitButton.Font = new Font("Oxygen", 12, FontStyle.Bold);
        }

        private void ConfigurationExitButton_MouseLeave(object sender, EventArgs e)
        {
            ConfigurationExitButton.Font = new Font("Oxygen", 12, FontStyle.Regular);
        }
        #endregion Game Client Configuration

        #region Set default screen and change screens
        private void SetDefaultScreen()
        {
            WindowsRegisterManager windowsRegisterManager = new WindowsRegisterManager();
            Microsoft.Win32.RegistryKey key = windowsRegisterManager.OpenWindowsRegister(true);

            string selectedDefaultScreen = (string)key.GetValue("selectedDefaultScreen");
            if (selectedDefaultScreen == null || selectedDefaultScreen.Equals(""))
            {
                key.SetValue("selectedDefaultScreen", ClientLanguage.store_Header);
            }

            windowsRegisterManager.CloseWindowsRegister(key);

            if (selectedDefaultScreen.Equals(ClientLanguage.store_Header))
            {
                Store_MouseDown(null, null);
            }
            else if (selectedDefaultScreen.Equals(ClientLanguage.gameLibrary_Header))
            {
                GameLibrary_MouseDown(null, null);
            }
            else if (selectedDefaultScreen.Equals(ClientLanguage.randomDefaultScreen))
            {
                Random random = new Random();
                int randomNumer = random.Next(1, 3);

                if (randomNumer == 1) Store_MouseDown(null, null);
                else if (randomNumer == 2) GameLibrary_MouseDown(null, null);
            }
            else
            {
                Store_MouseDown(null, null);
            }
        }

        private void ChangeScreen(ref bool selectedScreen, bool openGameInfo)
        {
            // Change between Screens (store, library...)
            if (!openGameInfo)
            {
                storeOpen = libraryOpen = gameInfoOpen = false;
                selectedScreen = true;

                StoreMenu.Visible = storeOpen;
                GameLibraryMenu.Visible = libraryOpen;
                GameInfoMenu.Visible = gameInfoOpen;

                Store_MouseLeave(null, EventArgs.Empty);
                GameLibrary_MouseLeave(null, EventArgs.Empty);

                FilterGame.Text = "";
            }
            else
            {
                if (storeOpen)
                {
                    Play_BuyGame.Text = ClientLanguage.buy_Button;
                    
                    MoneyPanel.Visible = true;
                    CurrentCurrency.Text = "900 " + ClientLanguage.currency;
                    this.GamePrice.Text = currentGameInfo.GamePrice + " " + ClientLanguage.currency;
                    GameSettingsBackground.Visible = false;

                }
                else if (libraryOpen)
                {
                    if (CheckGameFiles())
                    {
                        Play_BuyGame.Text = ClientLanguage.play_Button;
                    }
                    else
                    {
                        if (!currentGameInfo.GameDownloadLink.Equals(""))
                        {
                            if (!downloadInProgress) // Descarga no en curso
                            {
                                Play_BuyGame.Text = ClientLanguage.download_Avaible_Button;
                            }
                            else // Descarga en curso
                            {
                                Play_BuyGame.Text = ClientLanguage.download_Avaible_Button + " - " + ClientLanguage.action_InProgress;
                            }
                        }
                        else
                        {
                            Play_BuyGame.Text = ClientLanguage.download_Unavaible_Button;
                        }
                    }

                    MoneyPanel.Visible = false;
                    GameSettingsBackground.Visible = true;
                }

                selectedScreen = true;
                GameInfoMenu.Visible = gameInfoOpen;
                GameInfoMenu.BackgroundImage = currentGameInfo.GameImage;
            }
        }
        #endregion Set default screen and change screens

        #region Combobox controls, Language manager and Default Screen Manager

        #region Combobox controls
        // Allow Combo Box to center aligned
        private void Combobox_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboboxManager comboboxManager = new ComboboxManager();
            comboboxManager.Combobox_DrawItem(sender, e);
        }

        private void Combobox_DropDownClosed(object sender, EventArgs e)
        {
            ComboboxManager comboboxManager = new ComboboxManager();
            comboboxManager.Combobox_DropDownClosed(ConfigurationPanel);
        }
        #endregion Combobox controls

        #region Language manager
        /// <summary>
        /// Cambia el idioma de todos los objetos que hay en la aplicación
        /// </summary>
        public void ChangeAplicationLanguage()
        {
            // Events
            EventSendButton.Text = ClientLanguage.button_Confirm;
            EventExitButton.Text = ClientLanguage.button_Close;
            EventCodeError.Text = ClientLanguage.eventCode_Error;

            // Download - Uninstall
            CheckDownload_UninstallInformationLanguage();

            // Settings
            ConfigurationHeader.Text = ClientLanguage.settings_Header;
            ClientLanguageHeader.Text = ClientLanguage.settings_LanguageHeader;
            DefaultScreenHeader.Text = ClientLanguage.settings_DefaultScreenHeader;
            ConfigurationExitButton.Text = ClientLanguage.button_Close;
            DeleteAccount.Text = ClientLanguage.settings_DeleteAccount;

            // Main menu
            FilterGame.Text = ClientLanguage.filterGame;
            CurrentCurrencyHeader.Text = ClientLanguage.currentCurrency;
            GamePriceHeader.Text = ClientLanguage.currentGamePrice;
            StoreEmptyLabel.Text = ClientLanguage.StoreEmpty;
            LibraryEmptyLabel.Text = ClientLanguage.LibraryEmpty;

            // Side menu
            StoreLabel.Text = ClientLanguage.store_Header;
            GameLibraryLabel.Text = ClientLanguage.gameLibrary_Header;

            LanguageSelected.Items.Clear();
            string[] languagesAvaibles = ClientLanguage.settings_Languages.Split('*');
            foreach (string currentLanguage in languagesAvaibles)
            {
                LanguageSelected.Items.Add(currentLanguage);
            }

            DefaultScreen.Items.Clear();
            string[] ScreensAvaibles = ClientLanguage.settings_DefaultScreen.Split('*');
            foreach (string currentScreen in ScreensAvaibles)
            {
                DefaultScreen.Items.Add(currentScreen);
            }
        }

        private void CheckDownload_UninstallInformationLanguage()
        {
            LanguageManager languageManager = new LanguageManager();
            string currentLanguage = ChangeLanguageTemporarily(languageManager);

            if (downloadState.Equals(ClientLanguage.game_Download))
            {
                languageManager.ChangeCurrentLanguage(currentLanguage);
                DownloadState.Text = ClientLanguage.game_Download;
            }
            else if (downloadState.Equals(ClientLanguage.game_Install))
            {
                languageManager.ChangeCurrentLanguage(currentLanguage);
                DownloadState.Text = ClientLanguage.game_Install;
            }
            else if (downloadState.Equals(ClientLanguage.game_DownloadError))
            {
                languageManager.ChangeCurrentLanguage(currentLanguage);
                DownloadState.Text = ClientLanguage.game_DownloadError;
            }
            else if (downloadState.Equals(ClientLanguage.game_DownloadSucess))
            {
                languageManager.ChangeCurrentLanguage(currentLanguage);
                DownloadState.Text = ClientLanguage.game_DownloadSucess;
            }
            else if (downloadState.Equals(ClientLanguage.game_InstallError))
            {
                languageManager.ChangeCurrentLanguage(currentLanguage);
                DownloadState.Text = ClientLanguage.game_InstallError;
            }
            else if (downloadState.Equals(ClientLanguage.Uninstalling_Game))
            {
                languageManager.ChangeCurrentLanguage(currentLanguage);
                DownloadState.Text = ClientLanguage.Uninstalling_Game;
            }
            else if (downloadState.Equals(ClientLanguage.Uninstalled_Game))
            {
                languageManager.ChangeCurrentLanguage(currentLanguage);
                DownloadState.Text = ClientLanguage.Uninstalled_Game;
            }
            else if (downloadState.Equals(ClientLanguage.game_NoSpace))
            {
                languageManager.ChangeCurrentLanguage(currentLanguage);
                DownloadState.Text = ClientLanguage.game_NoSpace;
            }

            languageManager.ChangeCurrentLanguage(currentLanguage);
        }

        // Cambia el idioma a español y devuelve el idioma que esta seleccionado originalmente por el usuario
        private string ChangeLanguageTemporarily(LanguageManager languageManager)
        {
            WindowsRegisterManager windowsRegisterManager = new WindowsRegisterManager();
            Microsoft.Win32.RegistryKey key = windowsRegisterManager.OpenWindowsRegister(true);
            string currentLanguage = (string)key.GetValue("selectedLanguage");

            languageManager.ChangeCurrentLanguage("es-ES");

            return currentLanguage;
        }

        /// <summary>
        /// Cambia el idioma almacenado en la entrada del registro de windows y cambia el idioma de la aplicación
        /// </summary>
        /// <param name="sender">Objeto que recibe los eventos</param>
        /// <param name="e">Eventos que le ocurren al objeto</param>
        private void LanguageSelected_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int currentDefaultScreen = DefaultScreen.SelectedIndex;

            LanguageManager languageManager = new LanguageManager();
            languageManager.SelectCurrentAplicationWindow(null, clientAplication);
            languageManager.LanguageCombobox_LanguageChanged(LanguageSelected);

            SaveDefaultScreen(currentDefaultScreen);
            DefaultScreen.SelectedIndex = currentDefaultScreen;
        }
        #endregion Language manager

        #region Default Screen manager
        /// <summary>
        /// Cambia la pantalla por defecto almacenada en la entrada del registro de windows
        /// </summary>
        /// <param name="sender">Objeto que recibe los eventos</param>
        /// <param name="e">Eventos que le ocurren al objeto</param>
        private void DefaultScreen_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SaveDefaultScreen(DefaultScreen.SelectedIndex);
        }

        private void SaveDefaultScreen(int selectedItemIndex)
        {
            WindowsRegisterManager windowsRegisterManager = new WindowsRegisterManager();
            Microsoft.Win32.RegistryKey key = windowsRegisterManager.OpenWindowsRegister(true);
            key.SetValue("selectedDefaultScreen", DefaultScreen.Items[selectedItemIndex]);
            windowsRegisterManager.CloseWindowsRegister(key);
        }
        #endregion Default Screen manager

        #endregion Combobox controls and Language manager

        #region Tile Size - Game Filter - Figure Paint

        #region Paint Separative Line and Tile Size Icons
        private void TileSize_Paint(object sender, PaintEventArgs e)
        {
            Pen colorPen = new Pen(Color.FromArgb(130, 130, 130));
            Point p1 = new Point(63, 62);
            Point p2 = new Point(682, 62);
            e.Graphics.DrawLine(colorPen, p1, p2);
        }

        private void SmallTiles_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rectangleLeftUp = new Rectangle(10, 12, 6, 6);
            Rectangle rectangleRightUp = new Rectangle(20, 12, 6, 6);
            Rectangle rectangleLeftDown = new Rectangle(10, 22, 6, 6);
            Rectangle rectangleRightDown = new Rectangle(20, 22, 6, 6);

            Brush rectangleColor;
            if (tileSizeSelected.Equals("SmallTile"))
            {
                rectangleColor = new SolidBrush(Color.FromArgb(0, 0, 0));
            }
            else
            {
                rectangleColor = new SolidBrush(Color.FromArgb(150, 150, 150));
            }

            AddGraphics(e, rectangleLeftUp, rectangleRightUp, rectangleLeftDown, rectangleRightDown, rectangleColor);
        }

        private void MediumTiles_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rectangleLeftUp = new Rectangle(7, 9, 8, 8);
            Rectangle rectangleRightUp = new Rectangle(21, 9, 8, 8);
            Rectangle rectangleLeftDown = new Rectangle(7, 23, 8, 8);
            Rectangle rectangleRightDown = new Rectangle(21, 23, 8, 8);

            Brush rectangleColor;
            if (tileSizeSelected.Equals("MediumTile"))
            {
                rectangleColor = new SolidBrush(Color.FromArgb(0, 0, 0));
            }
            else
            {
                rectangleColor = new SolidBrush(Color.FromArgb(150, 150, 150));
            }

            AddGraphics(e, rectangleLeftUp, rectangleRightUp, rectangleLeftDown, rectangleRightDown, rectangleColor);
        }

        private void LargeTiles_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rectangleLeftUp = new Rectangle(6, 7, 10, 10);
            Rectangle rectangleRightUp = new Rectangle(21, 7, 10, 10);
            Rectangle rectangleLeftDown = new Rectangle(6, 23, 10, 10);
            Rectangle rectangleRightDown = new Rectangle(21, 23, 10, 10);

            Brush rectangleColor;
            if (tileSizeSelected.Equals("LargeTile"))
            {
                rectangleColor = new SolidBrush(Color.FromArgb(0, 0, 0));
            }
            else
            {
                rectangleColor = new SolidBrush(Color.FromArgb(150, 150, 150));
            }

            AddGraphics(e, rectangleLeftUp, rectangleRightUp, rectangleLeftDown, rectangleRightDown, rectangleColor);
        }

        private void AddGraphics(PaintEventArgs e, Rectangle rectangleLeftUp, Rectangle rectangleRightUp, Rectangle rectangleLeftDown, 
            Rectangle rectangleRightDown, Brush rectangleColor)
        {
            e.Graphics.FillRectangle(rectangleColor, rectangleLeftUp);
            e.Graphics.FillRectangle(rectangleColor, rectangleRightUp);
            e.Graphics.FillRectangle(rectangleColor, rectangleLeftDown);
            e.Graphics.FillRectangle(rectangleColor, rectangleRightDown);
        }

        private void TileSizeButtonsColor()
        {
            SmallTiles.Invalidate();
            MediumTiles.Invalidate();
            LargeTiles.Invalidate();
        }
        #endregion Paint Separative Line and Tile Size Icons

        #region Tile Size Icons Manager
        private void SmallTiles_MouseEnter(object sender, EventArgs e)
        {
            ShowToolTip(SmallTiles, ClientLanguage.smallTiles_Tooltip);
        }

        private void SmallTiles_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Hide(SmallTiles);
        }

        private void SmallTiles_Click(object sender, EventArgs e)
        {
            if (!tileSizeSelected.Equals("SmallTile"))
            {
                SmallTiles.Focus();
                tileSizeSelected = "SmallTile";
                SelectTileSize(false, true, false);
            }
        }

        private void MediumTiles_MouseEnter(object sender, EventArgs e)
        {
            ShowToolTip(MediumTiles, ClientLanguage.mediumTiles_Tooltip);
        }

        private void MediumTiles_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Hide(MediumTiles);
        }

        private void MediumTiles_Click(object sender, EventArgs e)
        {
            if (!tileSizeSelected.Equals("MediumTile"))
            {
                MediumTiles.Focus();
                tileSizeSelected = "MediumTile";
                SelectTileSize(false, true, false);
            }
        }

        private void LargeTiles_MouseEnter(object sender, EventArgs e)
        {
            ShowToolTip(LargeTiles, ClientLanguage.largeTiles_Tooltip);
        }

        private void LargeTiles_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Hide(LargeTiles);
        }

        private void LargeTiles_Click(object sender, EventArgs e)
        {
            if (!tileSizeSelected.Equals("LargeTile"))
            {
                LargeTiles.Focus();
                tileSizeSelected = "LargeTile";
                SelectTileSize(false, true, false);
            }
        }
        #endregion Tile Size Icons Manager

        #region Game Filter Manager
        private void FilterGame_Click(object sender, EventArgs e)
        {
            if (FilterGame.Text.Equals(ClientLanguage.filterGame) && FilterGame.ForeColor == Color.FromArgb(130, 130, 130))
                FilterGame.SelectionStart = 0;
        }

        private void FilterGame_KeyDown(object sender, KeyEventArgs e)
        {
            FilterGame.TextChanged -= FilterGame_TextChanged;

            FilterGame.Text = "";
            FilterGame.ForeColor = Color.FromArgb(0, 0, 0);
            FilterGame.KeyDown -= FilterGame_KeyDown;

            FilterGame.TextChanged += FilterGame_TextChanged;
        }

        private void FilterGame_TextChanged(object sender, EventArgs e)
        {
            if (currentGameImagesListView != null)
            {
                RefillFilteredGames(FilterGame.Text, 0, 0);
            }

            if (FilterGame.Text.Equals(""))
            {
                FilterGame.TextChanged -= FilterGame_TextChanged;

                FilterGame.Text = ClientLanguage.filterGame;
                FilterGame.ForeColor = Color.FromArgb(130, 130, 130);
                FilterGame.KeyDown += FilterGame_KeyDown;
                FilterGame.SelectionStart = 0;

                FilterGame.TextChanged += FilterGame_TextChanged;
            }
        }
        #endregion Game Filter Manager

        #endregion Tile Size - Game Filter - Figure Paint

        #region Fill Games Listview and Resize Listview Images
        private void SelectTileSize(bool formStarting, bool changeSize, bool chargeImages)
        {
            if (formStarting)
            {
                WindowsRegisterManager windowsRegisterManager = new WindowsRegisterManager();
                Microsoft.Win32.RegistryKey key = windowsRegisterManager.OpenWindowsRegister(true);

                string selectedTileSize = (string)key.GetValue("selectedTileSize");
                if (selectedTileSize == null || selectedTileSize.Equals(""))
                {
                    selectedTileSize = "LargeTile";
                    key.SetValue("selectedTileSize", selectedTileSize);
                }

                windowsRegisterManager.CloseWindowsRegister(key);

                tileSizeSelected = selectedTileSize;
                TileSizeButtonsColor();
            }

            if (changeSize || chargeImages)
            {
                if (changeSize)
                {
                    TileSizeButtonsColor();

                    switch (tileSizeSelected)
                    {
                        case "SmallTile":
                            if (FilterGame.Text.Equals(ClientLanguage.filterGame))
                                ResizeGameList(80, 120);
                            else
                                RefillFilteredGames(FilterGame.Text, 80, 120);
                            break;

                        case "MediumTile":
                            if (FilterGame.Text.Equals(ClientLanguage.filterGame))
                                ResizeGameList(130, 196);
                            else
                                RefillFilteredGames(FilterGame.Text, 130, 196);
                            break;

                        case "LargeTile":
                            if (FilterGame.Text.Equals(ClientLanguage.filterGame))
                                ResizeGameList(196, 256);
                            else
                                RefillFilteredGames(FilterGame.Text, 196, 256);
                            break;
                    }
                }
                else if (chargeImages)
                {
                    switch (tileSizeSelected)
                    {
                        case "SmallTile":
                            FillGameList(80, 120);
                            break;

                        case "MediumTile":
                            FillGameList(130, 196);
                            break;

                        case "LargeTile":
                            FillGameList(196, 256);
                            break;
                    }
                }
            }
        }

        private void FillGameList(int xSize, int ySize)
        {
            currentGameInfoList.Clear();
            currentGameImagesListView.Items.Clear();
            GameImages.Images.Clear();

            GameImages.ImageSize = new Size(xSize, ySize);
            GameImages.ColorDepth = ColorDepth.Depth32Bit;


            if (storeOpen) currentGameInfoList = CheckAvaibleGames();
            else if (libraryOpen) currentGameInfoList = CheckOwnedGames();

            if (currentGameInfoList == null || currentGameInfoList.Count == 0)
            {
                if (storeOpen)
                {
                    StoreEmpty.Visible = true;
                    StoreAvailableGames.Visible = false;
                }
                else if (libraryOpen)
                {
                    LibraryEmpty.Visible = true;
                    GameLibraryAvailableGames.Visible = false;
                }
            }
            else
            {
                StoreEmpty.Visible = false;
                LibraryEmpty.Visible = false;
                StoreAvailableGames.Visible = true;
                GameLibraryAvailableGames.Visible = true;

                try
                {
                    for (int currentImage = 0; currentImage < currentGameInfoList.Count; currentImage++)
                    {
                        try
                        {
                            GameImages.Images.Add(currentGameInfoList[currentImage].GameImage);
                            currentGameImagesListView.Items.Add(currentGameInfoList[currentImage].GameName, currentImage);
                        }
                        catch (OutOfMemoryException)
                        { }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                currentGameImagesListView.LargeImageList = GameImages;
            }
        }

        private void ResizeGameList(int xSize, int ySize)
        {
            currentGameImagesListView.Items.Clear();
            GameImages.Images.Clear();

            GameImages.ImageSize = new Size(xSize, ySize);
            GameImages.ColorDepth = ColorDepth.Depth32Bit;

            int currentImage = 0;
            foreach (GameInfo currentGameInfo in currentGameInfoList)
            {
                currentGameImagesListView.Items.Add(currentGameInfoList[currentImage].GameName, currentImage);
                GameImages.Images.Add(currentGameInfoList[currentImage].GameImage);

                currentImage++;
            }

            currentGameImagesListView.LargeImageList = GameImages;
        }

        private void RefillFilteredGames(string filteredGameName, int xSize, int ySize)
        {
            currentGameImagesListView.Items.Clear();
            GameImages.Images.Clear();

            if (xSize != 0 && ySize != 0)
            {
                GameImages.ImageSize = new Size(xSize, ySize);
                GameImages.ColorDepth = ColorDepth.Depth32Bit;
            }

            int currentImage = 0;
            foreach (GameInfo currentGameInfo in currentGameInfoList)
            {
                if (currentGameInfoList[currentImage].GameName.ToLower().Contains(filteredGameName.ToLower()) || filteredGameName.Equals(""))
                {
                    currentGameImagesListView.Items.Add(currentGameInfoList[currentImage].GameName, currentImage);
                    GameImages.Images.Add(currentGameInfoList[currentImage].GameImage);

                    currentImage++;
                }
            }

            currentGameImagesListView.LargeImageList = GameImages;
        }
        #endregion Fill Games Listview and Resize Listview Images

        #region Listview Manager and Game Info Button (Play - Download - Buy Button)
        private void GamesListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                string gameName = currentGameImagesListView.SelectedItems[0].SubItems[0].Text;

                foreach (GameInfo currentGameInfo in currentGameInfoList)
                {
                    if (currentGameInfo.GameName.Equals(gameName))
                    {
                        this.currentGameInfo = currentGameInfo;
                        break;
                    }
                }

                ChangeScreen(ref gameInfoOpen, true);
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (libraryOpen) ShowGameSettingsMenu(MousePosition.X + 10, MousePosition.Y + 15, null);
            }

            // Unselect all items
            if (this.currentGameImagesListView.SelectedIndices.Count > 0)
                for (int i = 0; i < this.currentGameImagesListView.SelectedIndices.Count; i++)
                {
                    this.currentGameImagesListView.Items[this.currentGameImagesListView.SelectedIndices[i]].Selected = false;
                }
        }

        private void GamesListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //if (libraryOpen) Launch / Install game
        }

        private void Play_BuyGame_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (storeOpen)
                {
                    OpenLoadingScreen();
                    string sqlQuery = "INSERT INTO games_owned(user_email, gameName, gameProgression) VALUES " +
                        "('" + userEmail + "', '" + currentGameInfo.GameName + "', null)";
                    string queryError = SQLManager.Insert_ModifyData(sqlQuery);

                    if (queryError.Length > 0)
                    {
                        if (queryError.Contains("Unable to connect"))
                        {
                            // Pop up de falta de internet - No te puedes conectar a la base de datos
                            GenericPopUpMessage(ClientLanguage.events_Database_ConnectionError);
                        }
                        else
                        {
                            // Cualquier otro tipo de error de la base de datos que tendra que salir en el pop up
                            GenericPopUpMessage(queryError);
                        }
                    }
                    else
                    {
                        SelectTileSize(false, false, true);
                        ChangeScreen(ref storeOpen, false);
                        GenericPopUpMessage(ClientLanguage.buyGameSucess);
                    }

                    CloseLoadingScreen(false);
                }
                else if (libraryOpen)
                {
                    if (Play_BuyGame.Text.Equals(ClientLanguage.play_Button))
                    {
                        string gamePath = Path.Combine(downloadPath, currentGameInfo.GameName, currentGameInfo.GameName + ".exe");
                        Process.Start(gamePath);
                    }
                    else if (Play_BuyGame.Text.Equals(ClientLanguage.download_Avaible_Button))
                    {
                        Download_InstallGame();
                    }
                    else if (Play_BuyGame.Text.Equals(ClientLanguage.gameSettingsMenu_Uninstall))
                    {
                        UninstallGame();
                    }
                }
            }
        }
        #endregion Listview Manager and Game Info Button (Play - Buy Button)

        #region Others

        #region Lost Focus
        private void LostFocus_Click(object sender, EventArgs e)
        {
            SideMenu.Focus();
        }
        #endregion Lost Focus

        #region ToolTip
        private void ShowToolTip(Panel currentPanel, string toolTipText)
        {
            toolTip.UseFading = true;
            toolTip.UseAnimation = true;
            toolTip.IsBalloon = true;
            toolTip.SetToolTip(currentPanel, toolTipText);
        }
        #endregion ToolTip

        #region Mouse is over control
        private bool MouseIsOverControl(Object currentObject)
        {
            // Check if the mouse is over a panel or label
            if (currentObject.GetType() == typeof(Panel))
            {
                Panel currentPanel = (Panel)currentObject;
                return currentPanel.ClientRectangle.Contains(currentPanel.PointToClient(Cursor.Position));
            }
            else if (currentObject.GetType() == typeof(Label))
            {
                Label currentLabel = (Label)currentObject;
                return currentLabel.ClientRectangle.Contains(currentLabel.PointToClient(Cursor.Position));
            }
            else
            {
                return false; // Else de relleno, no hace nada
            }
        }
        #endregion Mouse is over control

        #region Close Game Info
        private void CloseGameInfo_Click(object sender, EventArgs e)
        {
            gameInfoOpen = false;
            GameInfoMenu.Visible = gameInfoOpen;
        }
        #endregion Close Game Info

        #region Popup events
        private void GenericPopUpMessage(string eventText)
        {
            EventText.Location = new Point(20, 25);
            EventCode.Visible = false;
            EventSendButton.Visible = false;
            EventExitButton.Location = new Point(237, 105);
            EventText.Text = eventText;
            EventsPanel.Visible = true;
        }

        private void CheckHashResumes()
        {
            if (Hash_SHA2.VerifyResume(EventCode.Text))
            {
                EventCodeError.Visible = false;
                EventsPanel.Visible = false;

                EventCode.Text = "";
                EventCodeError.Visible = false;

                OpenLoadingScreen();

                string sqlQuery = "DELETE FROM user_information WHERE user_email LIKE '" + userEmail + "'";
                string queryError = SQLManager.Insert_ModifyData(sqlQuery);

                if (queryError.Length > 0)
                {
                    if (queryError.Contains("Unable to connect"))
                    {
                        // Pop up de falta de internet - No te puedes conectar a la base de datos
                        GenericPopUpMessage(ClientLanguage.events_Database_ConnectionError);
                    }
                    else
                    {
                        // Cualquier otro tipo de error de la base de datos que tendra que salir en el pop up
                        GenericPopUpMessage(queryError);
                    }
                }
                else
                {
                    // "Log Out" - Devuelvele a la pantalla de login
                }

                CloseLoadingScreen(false);
            }
            else
            {
                EventCodeError.Visible = true;
            }
        }

        private void EventExitButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                HideImageGradient();

                EventCode.Text = "";
                EventCodeError.Visible = false;
            }
        }

        private void EventSend_ExitButton_MouseEnter(object sender, EventArgs e)
        {
            ((Label)sender).Font = new Font("Oxygen", 12, FontStyle.Bold);
        }

        private void EventSend_ExitButton_MouseLeave(object sender, EventArgs e)
        {
            ((Label)sender).Font = new Font("Oxygen", 12, FontStyle.Regular);
        }

        private void EventSendButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                CheckHashResumes();
            }
        }

        private void EventCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CheckHashResumes();
            }
        }
        #endregion Popup events

        #region Show and Hide Image Gradient Panel
        // Methods to show Image Gradient Panel
        private void ShowImageGradient()
        {
            ConfigurationPanel.Visible = false;
            EventsPanel.Visible = false;

            SideMenu.Visible = false;
            MainMenu.Visible = false;
        }

        private void HideImageGradient()
        {
            ConfigurationPanel.Visible = false;
            EventsPanel.Visible = false;

            SideMenu.Visible = true;
            MainMenu.Visible = true;

            SelectTileSize(false, true, false);
        }
        #endregion Show and Hide Image Gradient Panel

        #region Enable / Disable Loading Screen
        private void OpenLoadingScreen()
        {
            ShowImageGradient();
            ImageGradient.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            operationInProgress = true;
        }

        private void CloseLoadingScreen(bool hideImageGradient)
        {
            if (hideImageGradient) HideImageGradient();
            ImageGradient.Cursor = System.Windows.Forms.Cursors.Default;
            operationInProgress = false;
        }
        #endregion Enable / Disable Loading Screen

        #endregion Others

        #region Context Menu Strip Manager (Drop-down menu)

        #region User Settings Menu
        private void ShowUserInfoMenu()
        {
            ContextMenuStrip.Name = "UserInfoMenu";
            ContextMenuStrip.Items.Clear();

            ContextMenuStrip.Items.Add(ClientLanguage.userInfoMenu_Settings);
            ContextMenuStrip.Items.Add(new ToolStripSeparator());
            ContextMenuStrip.Items.Add(ClientLanguage.userInfoMenu_logout);

            ContextMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.UserSettingsMenuStrip_ItemClicked);
            ContextMenuStrip.Show(UserInformation, new Point(UserInformation.Width, +10));
        }

        private void HideUserInfoMenu()
        {
            ContextMenuStrip.ItemClicked -= this.UserSettingsMenuStrip_ItemClicked;
            ContextMenuStrip.Hide();
            ContextMenuStrip.Items.Clear();
        }

        private void UserSettingsMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (ContextMenuStrip.Items[0] == e.ClickedItem)
            {
                OpenConfiguration();
            }
            else if (ContextMenuStrip.Items[2] == e.ClickedItem)
            {
                // Cerrar sesión
            }
        }
        #endregion User Settings Menu

        #region Game Settings Menu
        private void GameSettings_Click(object sender, EventArgs e)
        {
            ShowGameSettingsMenu(GameSettingsBackground.Width + 3, 3, GameSettingsBackground);
        }

        private void ShowGameSettingsMenu(int pointX, int pointY, Panel panelAttached)
        {
            ContextMenuStrip.Name = "GameSettingsMenu";
            ContextMenuStrip.Items.Clear();

            if (!gameInfoOpen)
            {
                string gameName = currentGameImagesListView.SelectedItems[0].SubItems[0].Text;

                foreach (GameInfo currentGameInfo in currentGameInfoList)
                {
                    if (currentGameInfo.GameName.Equals(gameName))
                    {
                        this.currentGameInfo = currentGameInfo;
                        break;
                    }
                }
            }

            if (CheckGameFiles())
            {
                ContextMenuStrip.Items.Add(new ToolStripMenuItem(ClientLanguage.play_Button));
                ContextMenuStrip.Items.Add(new ToolStripSeparator());
                ContextMenuStrip.Items.Add(ClientLanguage.gameSettingsMenu_Uninstall);
            }
            else
            {
                if (!currentGameInfo.GameDownloadLink.Equals(""))
                {
                    if (!downloadInProgress) // Descarga no en curso
                    {
                        ContextMenuStrip.Items.Add(new ToolStripMenuItem(ClientLanguage.download_Avaible_Button));
                    }
                    else // Descarga en curso
                    {
                        ContextMenuStrip.Items.Add(new ToolStripMenuItem(ClientLanguage.download_Avaible_Button + " - " + 
                            ClientLanguage.action_InProgress));
                    }
                }
                else
                {
                    ContextMenuStrip.Items.Add(new ToolStripMenuItem(ClientLanguage.download_Unavaible_Button));
                }
            }

            ContextMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.GameSettingsMenuStrip_ItemClicked);
            if (panelAttached == null) ContextMenuStrip.Show(pointX, pointY);
            if (panelAttached != null) ContextMenuStrip.Show(panelAttached, new Point(pointX, pointY));
        }

        private void GameSettingsMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text.Equals(ClientLanguage.play_Button))
            {
                // Jugar juego
            }
            else if (e.ClickedItem.Text.Equals(ClientLanguage.download_Avaible_Button))
            {
                Download_InstallGame();
            }
            else if (e.ClickedItem.Text.Equals(ClientLanguage.gameSettingsMenu_Uninstall))
            {
                UninstallGame();
            }
        }
        #endregion Game Settings Menu

        private void ContextMenuStrip_MouseLeave(object sender, EventArgs e)
        {
            if (ContextMenuStrip.Name.Equals("UserInfoMenu"))
            {
                ChangeBackgroundColor(UserInformation, objectUnmarked);

                ContextMenuStrip.ItemClicked -= this.UserSettingsMenuStrip_ItemClicked;
                ContextMenuStrip.Hide();
                ContextMenuStrip.Items.Clear();

                canTriggerSelections = true;
            }
            else
            {
                ContextMenuStrip.ItemClicked -= this.GameSettingsMenuStrip_ItemClicked;
            }
        }
        #endregion Context Menu Strip Manager (Drop-down menu)

        #region SQL Check games status
        private List<GameInfo> CheckOwnedGames()
        {
            string sqlQuery = "SELECT G_A.gameName, G_A.gameCover, G_A.gamePrice, G_A.gameDownloadLink " +
                "FROM games_avaibles G_A INNER JOIN games_owned G_O ON G_A.gameName = G_O.gameName " +
                "Where G_O.user_email LIKE '" + userEmail + "'";

            return SQLManager.ReadGameList(sqlQuery);
        }

        private List<GameInfo> CheckAvaibleGames()
        {
            string sqlQuery = "SELECT G_A.gameName, G_A.gameCover, G_A.gamePrice, G_A.gameDownloadLink FROM games_avaibles G_A " +
                "WHERE NOT EXISTS (SELECT G_O.gameName FROM games_owned G_O " +
                "WHERE G_A.gameName = G_O.gameName AND G_O.user_email Like '" + userEmail + "')";

            return SQLManager.ReadGameList(sqlQuery);
        }
        #endregion SQL Check games status

        private void CreateClientFolder()
        {
            if (!Directory.Exists(downloadPath))
            {
                try
                {
                    Directory.CreateDirectory(downloadPath);
                }
                catch (Exception)
                { }
            }
        }

        private async void Download_InstallGame()
        {
            long requeriedDriveSpace = 600000000; // 600 MB in bytes
            LanguageManager languageManager = new LanguageManager();

            CloseGameInfo_Click(null, EventArgs.Empty);
            ContextMenuStrip.Hide();
            downloadInProgress = true;

            while (!Directory.Exists(downloadPath))
            {
                CreateClientFolder();
            }

            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (downloadPath.Contains(drive.Name))
                {
                    if (drive.TotalFreeSpace >= requeriedDriveSpace)
                    {
                        if (Uninstall_Information.Visible)
                        {
                            Uninstall_Information.Location = new Point(0, 332);
                            Store.Location = new Point(0, 85);
                            GameLibrary.Location = new Point(0, 203);
                        }

                        DownloadInformationGameImage.BackgroundImage = currentGameInfo.GameImage;
                        DownloadInformationGameName.Text = currentGameInfo.GameName;

                        DownloadProgress.Style = ProgressBarStyle.Marquee;
                        DownloadProgress.Value = 40;
                        DownloadInformation.Visible = true;
                        CloseDownloadInformation.Visible = false;

                        DownloadState.ForeColor = Color.FromArgb(230, 230, 230);

                        languageManager.ChangeCurrentLanguage("es-ES");
                        downloadState = ClientLanguage.game_Download;
                        CheckDownload_UninstallInformationLanguage();
                        try
                        {
                            await Task.Run(async () =>
                            {
                            await DownloadGameFromMega();
                            });
                        }
                        catch (Exception)
                        {
                            DownloadState.ForeColor = Color.Red;
                            languageManager.ChangeCurrentLanguage("es-ES");
                            downloadState = ClientLanguage.game_DownloadError;
                            CheckDownload_UninstallInformationLanguage();

                            DownloadProgress.Style = ProgressBarStyle.Continuous;
                            DownloadProgress.Value = 100;

                            DeleteCorruptedFiles();
                            break;
                        }

                        languageManager.ChangeCurrentLanguage("es-ES");
                        downloadState = ClientLanguage.game_Install;
                        CheckDownload_UninstallInformationLanguage();
                        try
                        {
                            await Task.Run(async () =>
                            {
                                await Install_Game();
                            });
                        }
                        catch (Exception)
                        {
                            DownloadState.ForeColor = Color.Red;
                            languageManager.ChangeCurrentLanguage("es-ES");
                            downloadState = ClientLanguage.game_InstallError;
                            CheckDownload_UninstallInformationLanguage();

                            DownloadProgress.Style = ProgressBarStyle.Continuous;
                            DownloadProgress.Value = 100;

                            DeleteCorruptedFiles();
                            break;
                        }

                        languageManager.ChangeCurrentLanguage("es-ES");
                        downloadState = ClientLanguage.game_DownloadSucess;
                        CheckDownload_UninstallInformationLanguage();

                        DownloadProgress.Style = ProgressBarStyle.Continuous;
                        DownloadProgress.Value = 100;
                    }
                    else
                    {
                        DownloadState.ForeColor = Color.Red;
                        languageManager.ChangeCurrentLanguage("es-ES");
                        downloadState = ClientLanguage.game_NoSpace;
                        CheckDownload_UninstallInformationLanguage();

                        DownloadProgress.Style = ProgressBarStyle.Continuous;
                        DownloadProgress.Value = 100;
                    }
                }
            }

            CloseGameInfo_Click(null, EventArgs.Empty);
            ContextMenuStrip.Hide();
            downloadInProgress = false;
            CloseDownloadInformation.Visible = true;
        }

        private async void UninstallGame()
        {
            if (Directory.Exists(downloadPath))
            {
                CloseGameInfo_Click(null, EventArgs.Empty);
                ContextMenuStrip.Hide();
                uninstallInProgress = true;

                if (DownloadInformation.Visible)
                {
                    Uninstall_Information.Location = new Point(0, 332);
                    Store.Location = new Point(0, 85);
                    GameLibrary.Location = new Point(0, 203);
                }
                else
                {
                    this.Uninstall_Information.Location = new Point(0, 414);
                }

                UninstallProgress.Style = ProgressBarStyle.Marquee;
                UninstallProgress.Value = 40;
                Uninstall_Information.Visible = true;
                CloseUninstall_Information.Visible = false;

                UninstallState.ForeColor = Color.FromArgb(230, 230, 230);
                UninstallState.Text = ClientLanguage.Uninstalling_Game;

                if (Directory.Exists(Path.Combine(downloadPath, currentGameInfo.GameName)))
                {
                    Directory.Delete(Path.Combine(downloadPath, currentGameInfo.GameName), true);
                }

                UninstallState.Text = ClientLanguage.Uninstalled_Game;
                UninstallProgress.Style = ProgressBarStyle.Continuous;
                UninstallProgress.Value = 100;

                CloseGameInfo_Click(null, EventArgs.Empty);
                ContextMenuStrip.Hide();
                uninstallInProgress = false;
                CloseUninstall_Information.Visible = true;
            }
        }

        private void CloseDownloadInformation_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DownloadInformation.Visible = false;

                if (Uninstall_Information.Visible)
                {
                    Uninstall_Information.Location = new Point(0, 414);
                    Store.Location = new Point(0, 150);
                    GameLibrary.Location = new Point(0, 268);
                }
            }
        }

        private void CloseUninstall_Information_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Uninstall_Information.Visible = false;

                Store.Location = new Point(0, 150);
                GameLibrary.Location = new Point(0, 268);
            }
        }

        // Check if the game is already installed
        private bool CheckGameFiles()
        {
            if (Directory.Exists(Path.Combine(downloadPath, currentGameInfo.GameName)))
            {
                return true;
            }

            return false;
        }

        private void DeleteCorruptedFiles()
        {
            try
            {
                if (downloadGameName.Length > 0)
                {
                    if (File.Exists(Path.Combine(downloadPath, downloadGameName)))
                    {
                        File.Delete(Path.Combine(downloadPath, downloadGameName));
                    }
                    else if (Directory.Exists(Path.Combine(downloadPath, Path.GetFileNameWithoutExtension(downloadGameName))))
                    {
                        Directory.Delete(Path.Combine(downloadPath, Path.GetFileNameWithoutExtension(downloadGameName)));
                    }
                }
            }
            catch (Exception)
            { }
        }

        private async Task DownloadGameFromMega()
        {
            MegaApiClient mega = new MegaApiClient();
            mega.LoginAnonymous();

            Uri fileLink = new Uri(currentGameInfo.GameDownloadLink);

            INodeInfo node = mega.GetNodeFromLink(fileLink);

            downloadGameName = node.Name;

            if (File.Exists(Path.Combine(downloadPath, downloadGameName)))
            {
                File.Delete(Path.Combine(downloadPath, downloadGameName));
            }

            mega.DownloadFile(fileLink, Path.Combine(downloadPath, node.Name));
            mega.Logout();
        }

        private async Task Install_Game()
        {
            string zipPath = Path.Combine(downloadPath, downloadGameName);
            string extractPath = downloadPath;

            ZipFile.ExtractToDirectory(zipPath, extractPath);

            File.Delete(Path.Combine(downloadPath, downloadGameName));
        }
    }
}