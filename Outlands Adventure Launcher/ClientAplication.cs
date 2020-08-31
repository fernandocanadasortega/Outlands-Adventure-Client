using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Cuando cambies de idioma vete al registro de windows y cambia el texto de DefaultScreen antes de cerrar el registro de windows

namespace Outlands_Adventure_Launcher
{
    public partial class ClientAplication : Form
    {
        private ClientAplication clientAplication;

        private readonly int objectHighlighted = 30;
        private readonly int objectUnmarked = 0;
        private readonly int objectSelected = 50;
        private bool storeOpen, libraryOpen, gameInfoOpen;

        private bool canTriggerSelections;

        private ListView currentGameImagesListView;
        private Dictionary<string, Image> currentGameImages;

        private ToolTip toolTip;
        private string tileSizeSelected = "";

        public ClientAplication()
        {
            InitializeComponent();
        }

        private void ClientAplication_Load(object sender, EventArgs e)
        {
            currentGameImages = new Dictionary<string, Image>();
            ImageGradient.BackColor = Color.FromArgb(190, 0, 0, 0);
            GameInfoGradient.BackColor = Color.FromArgb(190, 0, 0, 0);
            toolTip = new ToolTip();
            canTriggerSelections = true;

            // Comprobar si el panel de inicio establecido no es la información, si no es la información del usuario hacer el metodo 1
            SelectTileSize(true, false, false); // Metodo 1
            SetDefaultScreen();
        }

        public void ReceiveClassInstance(ClientAplication clientAplication)
        {
            this.clientAplication = clientAplication;
        }

        #region Side Menu Manager

        private void SetDefaultScreen()
        {
            WindowsRegisterManager windowsRegisterManager = new WindowsRegisterManager();
            Microsoft.Win32.RegistryKey key = windowsRegisterManager.OpenWindowsRegister(true);

            string selectedDefaultScreen = (string)key.GetValue("selectedDefaultScreen");
            if (selectedDefaultScreen == null || selectedDefaultScreen.Equals(""))
            {
                key.SetValue("selectedDefaultScreen", "Tienda"); //store_Header
            }

            switch (selectedDefaultScreen)
            {
                case "Tienda": //store_Header
                    Store_MouseDown(null, null);
                    break;

                case "Libreria de juegos": //gameLibrary_Header
                    GameLibrary_MouseDown(null, null);
                    break;

                case "Aleatorio": //randomDefaultScreen
                    Random random = new Random();
                    int randomNumer = random.Next(1, 3);

                    if (randomNumer == 1) Store_MouseDown(null, null);
                    else if (randomNumer == 2) GameLibrary_MouseDown(null, null);
                    break;
            }
        }

        private void ChangeScreen(ref bool selectedScreen, Image gameImage)
        {
            // Change between Screens (store, library...)
            if (gameImage == null)
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
                //if (storeOpen) // Poner el dinero
                //else if (libraryOpen)

                selectedScreen = true;
                GameInfoMenu.Visible = gameInfoOpen;
                GameInfoMenu.BackgroundImage = gameImage;
            }
        }

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

                ContextMenuStrip.Name = "UserInfoMenu";
                ContextMenuStrip.Items.Clear();

                ContextMenuStrip.Items.Add("Ajustes de la aplicación"); //userInfoMenu_Settings
                ContextMenuStrip.Items.Add(new ToolStripSeparator());
                ContextMenuStrip.Items.Add("Cerrar sesión"); //userInfoMenu_logout

                ContextMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.UserSettingsMenuStrip_ItemClicked);
                ContextMenuStrip.Show(UserInformation, new Point(UserInformation.Width, + 10));

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

                        ContextMenuStrip.ItemClicked -= this.UserSettingsMenuStrip_ItemClicked;
                        ContextMenuStrip.Hide();
                        ContextMenuStrip.Items.Clear();

                        canTriggerSelections = true;
                    }
                }
            }
        }

        private void UserInformation_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e == null || e.Button == MouseButtons.Left)
            {
                MessageBox.Show("click");
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
                    ChangeScreen(ref storeOpen, null);

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
                    ChangeScreen(ref libraryOpen, null);

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

            ConfigurationPanel.Focus();
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
        }
        #endregion Show and Hide Image Gradient Panel

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

            // Settings
            ConfigurationHeader.Text = ClientLanguage.settings_Header;
            ClientLanguageHeader.Text = ClientLanguage.settings_LanguageHeader;

            // faltan el resto de elementos

            LanguageSelected.Items.Clear();
            string[] languagesAvaibles = ClientLanguage.settings_Languages.Split('*');
            foreach (string currentLanguage in languagesAvaibles)
            {
                LanguageSelected.Items.Add(currentLanguage);
            }

            ConfigurationExitButton.Text = ClientLanguage.button_Close;
        }

        /// <summary>
        /// Cambia el idioma almacenado en la entrada del registro de windows y cambia el idioma de la aplicación
        /// </summary>
        /// <param name="sender">Objeto que recibe los eventos</param>
        /// <param name="e">Eventos que le ocurren al objeto</param>
        private void LanguageSelected_SelectionChangeCommitted(object sender, EventArgs e)
        {
            LanguageManager languageManager = new LanguageManager();
            languageManager.SelectCurrentAplicationWindow(null, clientAplication);
            languageManager.LanguageCombobox_LanguageChanged(LanguageSelected);
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
            WindowsRegisterManager windowsRegisterManager = new WindowsRegisterManager();
            Microsoft.Win32.RegistryKey key = windowsRegisterManager.OpenWindowsRegister(true);
            key.SetValue("selectedDefaultScreen", DefaultScreen.SelectedItem.ToString());
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
            ShowToolTip(SmallTiles, "Small tiles"); //smallTiles_Tooltip
        }

        private void SmallTiles_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Hide(SmallTiles);
        }

        private void SmallTiles_Click(object sender, EventArgs e)
        {
            SmallTiles.Focus();
            tileSizeSelected = "SmallTile";
            SelectTileSize(false, true, false);
        }

        private void MediumTiles_MouseEnter(object sender, EventArgs e)
        {
            ShowToolTip(MediumTiles, "Medium tiles"); //mediumTiles_Tooltip
        }

        private void MediumTiles_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Hide(MediumTiles);
        }

        private void MediumTiles_Click(object sender, EventArgs e)
        {
            MediumTiles.Focus();
            tileSizeSelected = "MediumTile";
            SelectTileSize(false, true, false);
        }

        private void LargeTiles_MouseEnter(object sender, EventArgs e)
        {
            ShowToolTip(LargeTiles, "Large tiles"); //largeTiles_Tooltip
        }

        private void LargeTiles_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Hide(LargeTiles);
        }

        private void LargeTiles_Click(object sender, EventArgs e)
        {
            LargeTiles.Focus();
            tileSizeSelected = "LargeTile";
            SelectTileSize(false, true, false);
        }
        #endregion Tile Size Icons Manager

        #region Game Filter Manager
        private void FilterGame_Click(object sender, EventArgs e)
        {
            if (FilterGame.Text.Equals("Filter Library") && FilterGame.ForeColor == Color.FromArgb(130, 130, 130))
                FilterGame.SelectionStart = 0; //filterGame
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

                FilterGame.Text = "Filter Library"; //filterGame
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
                tileSizeSelected = "LargeTile"; // Cambiar por el que exista en el registro de windows
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
                            if (FilterGame.Text.Equals("Filter Library")) //filterGame
                                ResizeGameList(80, 120);
                            else
                                RefillFilteredGames(FilterGame.Text, 80, 120);
                            break;

                        case "MediumTile":
                            if (FilterGame.Text.Equals("Filter Library")) //filterGame
                                ResizeGameList(130, 196);
                            else
                                RefillFilteredGames(FilterGame.Text, 130, 196);
                            break;

                        case "LargeTile":
                            if (FilterGame.Text.Equals("Filter Library")) //filterGame
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
            currentGameImages.Clear();
            currentGameImagesListView.Items.Clear();
            GameImages.Images.Clear();

            string folderPath = "C:/Users/thena/Desktop/GTX 1060/memes"; // Cambiar por imagenes de la base de datos
            GameImages.ImageSize = new Size(xSize, ySize);
            GameImages.ColorDepth = ColorDepth.Depth32Bit;

            string[] paths = Directory.GetFiles(folderPath);

            try
            {
                for (int currentImage = 0; currentImage < paths.Length; currentImage++)
                {
                    Image gameImage = Image.FromFile(paths[currentImage]);
                    string gameName = Path.GetFileNameWithoutExtension(paths[currentImage]);

                    GameImages.Images.Add(gameImage);
                    currentGameImagesListView.Items.Add(gameName, currentImage);

                    currentGameImages.Add(gameName, gameImage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            currentGameImagesListView.LargeImageList = GameImages;
        }

        private void ResizeGameList(int xSize, int ySize)
        {
            currentGameImagesListView.Items.Clear();
            GameImages.Images.Clear();

            GameImages.ImageSize = new Size(xSize, ySize);
            GameImages.ColorDepth = ColorDepth.Depth32Bit;

            int currentImage = 0;
            foreach (KeyValuePair<string, Image> currentGameDictionaryPair in currentGameImages)
            {
                currentGameImagesListView.Items.Add(currentGameDictionaryPair.Key, currentImage);
                GameImages.Images.Add(currentGameDictionaryPair.Value);

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
            foreach (KeyValuePair<string, Image> currentGameDictionaryPair in currentGameImages)
            {
                if ((currentGameDictionaryPair.Key).ToLower().Contains(filteredGameName.ToLower()) || filteredGameName.Equals(""))
                {
                    currentGameImagesListView.Items.Add(currentGameDictionaryPair.Key, currentImage);
                    GameImages.Images.Add(currentGameDictionaryPair.Value);

                    currentImage++;
                }
            }

            currentGameImagesListView.LargeImageList = GameImages;
        }
        #endregion Fill Games Listview and Resize Listview Images

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

        #endregion Others

        private void GamesListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                string gameName = currentGameImagesListView.SelectedItems[0].SubItems[0].Text;
                currentGameImages.TryGetValue(gameName, out Image gameImage);
                ChangeScreen(ref gameInfoOpen, gameImage);
            }
            else if (e.Button == MouseButtons.Right)
            {
                ShowGameSettingsMenu(MousePosition.X + 10, MousePosition.Y + 15, null);
            }

            // Unselect all items
            if (this.currentGameImagesListView.SelectedIndices.Count > 0)
                for (int i = 0; i < this.currentGameImagesListView.SelectedIndices.Count; i++)
                {
                    this.currentGameImagesListView.Items[this.currentGameImagesListView.SelectedIndices[i]].Selected = false;
                }
        }

        private void StoreGamesListView_DoubleClick(object sender, EventArgs e)
        {
            // Abrir juego inmediatamente o instalarlo si no esta
        }

        private void CloseGameInfo_Click(object sender, EventArgs e)
        {
            gameInfoOpen = false;
            GameInfoMenu.Visible = gameInfoOpen;
        }

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

        private void ShowGameSettingsMenu(int pointX, int pointY, Panel panelAttached)
        {
            ContextMenuStrip.Name = "GameSettingsMenu";
            ContextMenuStrip.Items.Clear();

            ContextMenuStrip.Items.Add(new ToolStripMenuItem("Jugar")); //gameSettingsMenu_Play
            ContextMenuStrip.Items.Add(new ToolStripSeparator());
            ContextMenuStrip.Items.Add(new ToolStripMenuItem("Actualizar")); //gameSettingsMenu_Update
            ContextMenuStrip.Items.Add(new ToolStripSeparator());
            ContextMenuStrip.Items.Add("Reparar"); //gameSettingsMenu_Repair
            ContextMenuStrip.Items.Add(new ToolStripSeparator());
            ContextMenuStrip.Items.Add("Desinstalar"); //gameSettingsMenu_Uninstall

            ContextMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.GameSettingsMenuStrip_ItemClicked);
            if (panelAttached == null) ContextMenuStrip.Show(pointX, pointY);
            if (panelAttached != null) ContextMenuStrip.Show(panelAttached, new Point(pointX, pointY));
        }

        private void GameSettings_Click(object sender, EventArgs e)
        {
            ShowGameSettingsMenu(GameSettingsBackground.Width + 3, 3, GameSettingsBackground);
        }

        private void UserSettingsMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (ContextMenuStrip.Items[0] == e.ClickedItem)
            {
                OpenConfiguration();
            }
            else if (ContextMenuStrip.Items[1] == e.ClickedItem)
            {
                // Cerrar sesión
            }
        }

        private void GameSettingsMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }


        private void StoreAvailableGames_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }
    }
}