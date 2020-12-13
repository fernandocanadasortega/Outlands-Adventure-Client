using CG.Web.MegaApiClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Outlands_Adventure_Launcher
{
	/// <summary>
	/// Class in charge of managing the main application form
	/// </summary>
	public partial class ClientAplicationSmall : Form
	{
		private ClientAplicationSmall clientAplication;
		private WindowsRegisterManager windowsRegisterManager;
		public static bool aplicationClosing;
		private string userEmail = "";
		private string userName = "";

		private readonly string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
			"Outlands Adventure Client");
		private string downloadGameName = "";
		private string downloadState = "";
		private int downloadProgress = 0;
		private bool downloadError;
		private GameInfo downloadingGame;
		private List<GameInfo> queueGames;

		private readonly int objectHighlighted = 30;
		private readonly int objectUnmarked = 0;
		private readonly int objectSelected = 50;
		private bool storeOpen, libraryOpen, gameInfoOpen, downloadsInfoOpen;

		private bool operationInProgress;
		private bool canTriggerSelections;

		private ListView currentGameImagesListView;
		private List<GameInfo> currentGameInfoList;
		private GameInfo currentGameInfo;

		private string tileSizeSelected = "";

		/// <summary>
		/// Constructor of the class
		/// </summary>
		public ClientAplicationSmall()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Receive the instance of ClientAplication class and the username of the user
		/// </summary>
		/// <param name="clientAplication">Instance of ClientAplication class</param>
		/// <param name="userName">String, user name of the user that logged in</param>
		public void ReceiveClassInstance(ClientAplicationSmall clientAplication, string userName)
		{
			this.clientAplication = clientAplication;
			UserName.Text = userName;
			this.userName = userName;
		}

		#region Form Actions
		/// <summary>
		/// Method that is executed when the form finish loading, read the windows register, the language in use, the size of the 
		/// game images and initialize multiple variables
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void ClientAplication_Load(object sender, EventArgs e)
		{
			windowsRegisterManager = new WindowsRegisterManager();
			CreateClientFolder();
			windowsRegisterManager.LoadWindowPosition(this);

			LanguageManager languageManager = new LanguageManager();
			languageManager.SelectCurrentAplicationWindow(null, null, null, clientAplication);
			languageManager.ReadSelectedLanguage(true, LanguageSelected);

			currentGameInfoList = new List<GameInfo>();
			queueGames = new List<GameInfo>();

			ImageGradient.BackColor = Color.FromArgb(190, 0, 0, 0);
			GameInfoGradient.BackColor = Color.FromArgb(210, 0, 0, 0);

			aplicationClosing = false;
			operationInProgress = false;
			canTriggerSelections = true;

			// Comprobar si el panel de inicio establecido no es la información, si no es la información del usuario hacer el metodo 1
			SelectTileSize(true, false, false); // Metodo 1
			SetDefaultScreen();

			SideDownloadProgressbar.MarqueeAnimationSpeed = 40;
			UninstallProgress.MarqueeAnimationSpeed = 40;
		}

		/// <summary>
		/// Method that is executed when the form is closing but not closed yet, check if an SQL operation is on going then cancel
		/// the method, if there is no operation in progress then show a close pop-up to ask what the user want to do
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void ClientAplication_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!aplicationClosing)
			{
				if (operationInProgress)
				{
					e.Cancel = true;
				}
				else
				{
					CloseClientAplicationPopUp closePopup;
					if (downloadingGame != null)
					{
						closePopup = new CloseClientAplicationPopUp(this, tileSizeSelected, e, downloadingGame.DownloadCancellationTokenSource);
					}
					else
					{
						closePopup = new CloseClientAplicationPopUp(this, tileSizeSelected, e, null);
					}
					closePopup.ShowDialog();
				}
			}
		}
		#endregion Form Actions

		#region Side Menu Manager
		/// <summary>
		/// Change the background color of the object the cursor is on
		/// </summary>
		/// <param name="backgroundPanel">Panel, panel that will change of color when you move the mouse in, out or when you
		/// click on the panel</param>
		/// <param name="colorOpacity">Int, opacity of the color depending of the action done</param>
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
		/// <summary>
		/// Triggered when you move the mouse in the sidemenu user information panel, change the color of the panel
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void UserInformation_MouseEnter(object sender, EventArgs e)
		{
			if (canTriggerSelections)
			{
				ChangeBackgroundColor(UserInformation, objectHighlighted);
				ShowUserInfoMenu();
				canTriggerSelections = false;
			}
		}

		/// <summary>
		/// Triggered when you move the mouse out the sidemenu user information panel, change the color of the panel only if the mouse
		/// left the container panel and not the label inside
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void UserInformation_MouseLeave(object sender, EventArgs e)
		{
			if (sender.GetType() == typeof(Panel))
			{
				if ((Panel)sender == UserInformation)
				{
					if (!MouseIsOverControl(UserName) && !MouseIsOverControl(UserConfigurationArrow) &&
						!MouseIsOverControl(UserPhoto) && !MouseIsOverControl(GameInfoMenu))
					{
						HideUserInfoMenu();
						ChangeBackgroundColor(UserInformation, objectUnmarked);
						canTriggerSelections = true;
					}
				}
			}
		}
		#endregion User Information

		#region Store
		/// <summary>
		/// Triggered when you move the mouse in the sidemenu store panel, change the color of the panel
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void Store_MouseEnter(object sender, EventArgs e)
		{
			if (canTriggerSelections)
			{
				ChangeBackgroundColor(Store, objectHighlighted);
				canTriggerSelections = false;
			}
		}

		/// <summary>
		/// Triggered when you move the mouse out the sidemenu store panel, change the color of the panel only if the mouse
		/// left the container panel and not the label inside
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
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

		/// <summary>
		/// Triggered when you right click the mouse in the sidemenu store panel, change the color of the panel when you click the panel
		/// and change the visible panel to show the game store and charge the necessary games
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void Store_MouseDown(object sender, MouseEventArgs e)
		{
			if (e == null || e.Button == MouseButtons.Left)
			{
				Store.Focus();
				ChangeScreen(ref storeOpen, false);

				ChangeBackgroundColor(Store, objectSelected);
				ChangeBackgroundColor(GameLibrary, objectUnmarked);

				currentGameImagesListView = StoreAvailableGames;
				SelectTileSize(false, false, true);

				Store.Invalidate();
			}
		}

		/// <summary>
		/// Triggered when the sidemenu store panel is visible, paint a line to indicate the active sidemenu option (store, game library...)
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
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
		/// <summary>
		/// Triggered when you move the mouse in the sidemenu game library panel, change the color of the panel
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void GameLibrary_MouseEnter(object sender, EventArgs e)
		{
			if (canTriggerSelections)
			{
				ChangeBackgroundColor(GameLibrary, objectHighlighted);
				canTriggerSelections = false;
			}
		}

		/// <summary>
		/// Triggered when you move the mouse out the sidemenu game library panel, change the color of the panel only if the mouse
		/// left the container panel and not the label inside
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
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

		/// <summary>
		/// Triggered when you right click the mouse in the sidemenu game library panel, change the color of the panel when you 
		/// click the panel and change the visible panel to show the game library and charge the necessary games
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void GameLibrary_MouseDown(object sender, MouseEventArgs e)
		{
			if (e == null || e.Button == MouseButtons.Left)
			{
				GameLibrary.Focus();
				ChangeScreen(ref libraryOpen, false);

				ChangeBackgroundColor(GameLibrary, objectSelected);
				ChangeBackgroundColor(Store, objectUnmarked);

				currentGameImagesListView = GameLibraryAvailableGames;
				SelectTileSize(false, false, true);

				GameLibrary.Invalidate();
			}
		}

		/// <summary>
		/// Triggered when the sidemenu game library panel is visible, paint a line to indicate the active sidemenu option 
		/// (store, game library...)
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
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
		/// <summary>
		/// Open the configuration panel read the current language and default screen in use, then assign the values in their corresponding
		/// comboboxs
		/// </summary>
		private void OpenConfiguration()
		{
			ShowImageGradient();
			ConfigurationPanel.Visible = true;

			FilterGame.Text = "";

			LanguageManager languageManager = new LanguageManager();
			languageManager.ReadSelectedLanguage(false, LanguageSelected);

			Microsoft.Win32.RegistryKey key = windowsRegisterManager.OpenWindowsRegister(false);
			string selectedDefaultScreen = (string)key.GetValue("selectedDefaultScreen");

			if (selectedDefaultScreen.Equals("Biblioteca de juegos"))
			{
				DefaultScreen.SelectedIndex = 0;
			}
			else if (selectedDefaultScreen.Equals("Tienda"))
			{
				DefaultScreen.SelectedIndex = 1;
			}
			else
			{
				DefaultScreen.SelectedIndex = 2;
			}
			windowsRegisterManager.CloseWindowsRegister(key);

			if (DefaultScreen.SelectedItem == null || DefaultScreen.SelectedIndex == -1)
				DefaultScreen.SelectedIndex = 0;

			ResolutionManager resolutionManager = new ResolutionManager();
			resolutionManager.ReadSelectedResolution(ResolutionSelected);

			ConfigurationPanel.Focus();
		}

		#region Configuration Delete Account
		/// <summary>
		/// Create a code and pass it by email, then show a panel with a textbox requiring the code, if the code you introduced
		/// is the same that the code passed by email then delete the current user account
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void DeleteAccount_MouseClick(object sender, MouseEventArgs e)
		{
			
			if (e.Button == MouseButtons.Left)
			{
				OpenLoadingScreen();
				string confirmationCode = CreateConfirmationCode.CreateCode();
				Hash_SHA2.InitialiceVariables(confirmationCode);

				string[] messageInfo = LanguageResx.ClientLanguage.sendEmail_DeleteAccount.Split('*');
				string messageError = "";
				if (userEmail.Equals("error")) messageError = "Email direction not found";
				//else messageError = SendEmail.SendNewEmail(userEmail, messageInfo[0], messageInfo[1], confirmationCode);

				if (messageError.Equals(""))
				{
					GenericPopUpCode(LanguageResx.ClientLanguage.events_Header_NewAccount);
				}
				else
				{
					GenericPopUpMessage(LanguageResx.ClientLanguage.events_SendEmailError + "\n \n" + messageError);
				}

				CloseLoadingScreen(false);
			}
		}

		/// <summary>
		/// Triggered when the mouse enter in the delete account label, change the style of the label text
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void DeleteAccount_MouseEnter(object sender, EventArgs e)
		{
			((Label)sender).Font = new Font("Oxygen", 11, FontStyle.Bold);
		}

		/// <summary>
		/// Triggered when the mouse leaves the delete account label, change the style of the label text
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void DeleteAccount_MouseLeave(object sender, EventArgs e)
		{
			((Label)sender).Font = new Font("Oxygen", 11, FontStyle.Regular);
		}
		#endregion Configuration Delete Account

		#region Configuration Exit Button
		/// <summary>
		/// Triggered when the mouse enter in the configuration exit label, change the style of the label text
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void ConfigurationExitButton_MouseEnter(object sender, EventArgs e)
		{
			ConfigurationExitButton.Font = new Font("Oxygen", 10, FontStyle.Bold);
		}

		/// <summary>
		/// Triggered when the mouse leaves the configuration exit label, change the style of the label text
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void ConfigurationExitButton_MouseLeave(object sender, EventArgs e)
		{
			ConfigurationExitButton.Font = new Font("Oxygen", 10, FontStyle.Regular);
		}

		/// <summary>
		/// Triggered when the mouse click in the configuration exit label, close configuration panel
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void ConfigurationExitButton_Click(object sender, EventArgs e)
		{
			HideImageGradient();
		}
		#endregion Configuration Exit Button

		#region Configuration Refresh Resolution
		/// <summary>
		/// Show a tooltip when the mouse enter the icon
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void ResolutionRefresh_MouseEnter(object sender, EventArgs e)
		{
			MultipleResources.ShowToolTip(ResolutionRefresh, LanguageResx.ClientLanguage.RefreshResolution_Tooltip);
		}

		/// <summary>
		/// Hide the tooltip when the mouse exit the icon
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void ResolutionRefresh_MouseLeave(object sender, EventArgs e)
		{
			MultipleResources.HideToolTip(ResolutionRefresh);
		}

		/// <summary>
		/// Change the resolution of the application
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">MouseEvents that occur to the object</param>
		private void ResolutionRefresh_MouseClick(object sender, MouseEventArgs e)
		{
			if (downloadingGame != null)
			{
				CancelDownload_Click(null, EventArgs.Empty);
			}
			ResolutionManager resolutionManager = new ResolutionManager();
			resolutionManager.ResolutionCombobox_ResolutionChanged(ResolutionSelected);
			resolutionManager.LoadWindowResolution(false, null, null, null, clientAplication, userName);
		}
		#endregion Configuration Refresh Resolution

		#endregion Game Client Configuration

		#region Set default screen and change screens
		/// <summary>
		/// Read from windows register the chosen default screen and open it when the form starts
		/// </summary>
		private void SetDefaultScreen()
		{
			Microsoft.Win32.RegistryKey key = windowsRegisterManager.OpenWindowsRegister(true);

			string selectedDefaultScreen = (string)key.GetValue("selectedDefaultScreen");
			if (selectedDefaultScreen == null || selectedDefaultScreen.Equals(""))
			{
				key.SetValue("selectedDefaultScreen", "Tienda");
				selectedDefaultScreen = "Tienda";
			}

			windowsRegisterManager.CloseWindowsRegister(key);

			if (selectedDefaultScreen.Equals("Tienda"))
			{
				Store_MouseDown(null, null);
			}
			else if (selectedDefaultScreen.Equals("Biblioteca de juegos"))
			{
				GameLibrary_MouseDown(null, null);
			}
			else if (selectedDefaultScreen.Equals("Aleatorio"))
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

		/// <summary>
		/// Change between screens (store, game library, game info...)
		/// </summary>
		/// <param name="selectedScreen">Boolean, Requested screen (store, game library, game info...)</param>
		/// <param name="openGameInfo">Boolean, is only true when you open a game's info, if false then open the store or any other screen
		/// excluding the game info screen</param>
		private void ChangeScreen(ref bool selectedScreen, bool openGameInfo)
		{
			if (!openGameInfo)
			{
				storeOpen = libraryOpen = gameInfoOpen = downloadsInfoOpen = false;
				selectedScreen = true;

				StoreMenu.Visible = storeOpen;
				GameLibraryMenu.Visible = libraryOpen;
				GameInfoMenu.Visible = gameInfoOpen;
				DownloadsPanel.Visible = downloadsInfoOpen;

				Store_MouseLeave(Store, EventArgs.Empty);
				GameLibrary_MouseLeave(GameLibrary, EventArgs.Empty);

				FilterGame.Text = "";

				if (downloadsInfoOpen)
				{
					Store.Invalidate();
					GameLibrary.Invalidate();
				}
			}
			else
			{
				if (storeOpen)
				{
					Play_BuyGame.Text = LanguageResx.ClientLanguage.buy_Button;

					MoneyPanel.Visible = true;
					CurrentCurrency.Text = "900 " + LanguageResx.ClientLanguage.currency;
					this.GamePrice.Text = currentGameInfo.GamePrice + " " + LanguageResx.ClientLanguage.currency;
					GameSettingsBackground.Visible = false;

				}
				else if (libraryOpen)
				{
					if (CheckGameFiles())
					{
						Play_BuyGame.Text = LanguageResx.ClientLanguage.play_Button;
					}
					else
					{
						if (!currentGameInfo.GameDownloadLink.Equals(""))
						{
							if (downloadingGame == null)
							{
								Play_BuyGame.Text = LanguageResx.ClientLanguage.download_Avaible_Button;
							}
							else
							{
								bool coincidences = false;
								foreach (GameInfo currentQueueGame in queueGames)
								{
									if (currentQueueGame.GameName == currentGameInfo.GameName)
									{
										Play_BuyGame.Text = LanguageResx.ClientLanguage.InQueue;
										coincidences = true;
									}
								}

								if (!coincidences)
								{
									if (downloadingGame.GameName == currentGameInfo.GameName)
									{
										Play_BuyGame.Text = LanguageResx.ClientLanguage.game_Downloading_LowerCase;
									}
									else
									{
										Play_BuyGame.Text = LanguageResx.ClientLanguage.download_Avaible_Button;
									}
								}
							}
						}
						else
						{
							Play_BuyGame.Text = LanguageResx.ClientLanguage.download_Unavaible_Button;
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
		/// <summary>
		/// Allow Combo Box text to center aligned
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void Combobox_DrawItem(object sender, DrawItemEventArgs e)
		{
			ComboboxManager comboboxManager = new ComboboxManager();
			comboboxManager.Combobox_DrawItem(sender, e);
		}

		/// <summary>
		/// Close the combobox and lose the focus from the combobox
		/// </summary>
		/// <param name="configurationPanel">Panel, panel that will gain the focus</param>
		private void Combobox_DropDownClosed(object sender, EventArgs e)
		{
			ComboboxManager comboboxManager = new ComboboxManager();
			comboboxManager.Combobox_DropDownClosed(ConfigurationPanel);
		}
		#endregion Combobox controls

		#region Language manager
		/// <summary>
		/// Change the language of all the objects in the application
		/// Cambia el idioma de todos los objetos que hay en la aplicación
		/// </summary>
		public void ChangeAplicationLanguage()
		{
			// Events
			EventSendButton.Text = LanguageResx.ClientLanguage.button_Confirm;
			EventExitButton.Text = LanguageResx.ClientLanguage.button_Close_Uppercase;
			EventCodeError.Text = LanguageResx.ClientLanguage.eventCode_Error;

			// Download - Uninstall
			CheckDownload_UninstallInformationLanguage();

			// Settings
			ConfigurationHeader.Text = LanguageResx.ClientLanguage.settings_Header;
			ClientLanguageHeader.Text = LanguageResx.ClientLanguage.settings_LanguageHeader;
			DefaultScreenHeader.Text = LanguageResx.ClientLanguage.settings_DefaultScreenHeader;
			ConfigurationExitButton.Text = LanguageResx.ClientLanguage.button_Close_Uppercase;
			DeleteAccount.Text = LanguageResx.ClientLanguage.settings_DeleteAccount;
			ResolutionHeader.Text = LanguageResx.ClientLanguage.settings_ResolutionHeader;

			// Main menu
			FilterGame.Text = LanguageResx.ClientLanguage.filterGame;
			CurrentCurrencyHeader.Text = LanguageResx.ClientLanguage.currentCurrency;
			GamePriceHeader.Text = LanguageResx.ClientLanguage.currentGamePrice;
			StoreEmptyLabel.Text = LanguageResx.ClientLanguage.StoreEmpty;
			LibraryEmptyLabel.Text = LanguageResx.ClientLanguage.LibraryEmpty;

			// Side menu
			StoreLabel.Text = LanguageResx.ClientLanguage.store_Header;
			GameLibraryLabel.Text = LanguageResx.ClientLanguage.gameLibrary_Header;

			// DownloadsPanel
			DownloadingGameHeader.Text = LanguageResx.ClientLanguage.game_Downloading_LowerCase;
			QueueGameHeader.Text = LanguageResx.ClientLanguage.InQueue;

			LanguageSelected.Items.Clear();
			string[] languagesAvaibles = LanguageResx.ClientLanguage.settings_Languages.Split('*');
			foreach (string currentLanguage in languagesAvaibles)
			{
				LanguageSelected.Items.Add(currentLanguage);
			}

			DefaultScreen.Items.Clear();
			string[] ScreensAvaibles = LanguageResx.ClientLanguage.settings_DefaultScreen.Split('*');
			foreach (string currentScreen in ScreensAvaibles)
			{
				DefaultScreen.Items.Add(currentScreen);
			}

			ResolutionSelected.Items.Clear();
			string[] resolutionsAvaibles = LanguageResx.ClientLanguage.settings_resolution.Split('*');
			foreach (string currentResolution in resolutionsAvaibles)
			{
				ResolutionSelected.Items.Add(currentResolution);
			}
			ResolutionManager resolutionManager = new ResolutionManager();
			resolutionManager.ReadSelectedResolution(ResolutionSelected);
		}

		/// <summary>
		/// Change the game download information language when a download is in progress
		/// </summary>
		private void CheckDownload_UninstallInformationLanguage()
		{
			LanguageManager languageManager = new LanguageManager();
			string currentLanguage = ChangeLanguageTemporarily(languageManager);

			if (downloadState.Equals(LanguageResx.ClientLanguage.game_Downloading_Uppercase))
			{
				languageManager.ChangeCurrentLanguage(currentLanguage);
				SideDownloadState.Text = LanguageResx.ClientLanguage.game_Downloading_Uppercase;
			}
			else if (downloadState.Equals(LanguageResx.ClientLanguage.download_canceled))
            {
				languageManager.ChangeCurrentLanguage(currentLanguage);
				SideDownloadState.Text = LanguageResx.ClientLanguage.download_canceled;
			}
			else if (downloadState.Equals(LanguageResx.ClientLanguage.game_Install))
			{
				languageManager.ChangeCurrentLanguage(currentLanguage);
				SideDownloadState.Text = LanguageResx.ClientLanguage.game_Install;
			}
			else if (downloadState.Equals(LanguageResx.ClientLanguage.game_DownloadError))
			{
				languageManager.ChangeCurrentLanguage(currentLanguage);
				SideDownloadState.Text = LanguageResx.ClientLanguage.game_DownloadError;
			}
			else if (downloadState.Equals(LanguageResx.ClientLanguage.game_DownloadSucess))
			{
				languageManager.ChangeCurrentLanguage(currentLanguage);
				SideDownloadState.Text = LanguageResx.ClientLanguage.game_DownloadSucess;
			}
			else if (downloadState.Equals(LanguageResx.ClientLanguage.game_InstallError))
			{
				languageManager.ChangeCurrentLanguage(currentLanguage);
				SideDownloadState.Text = LanguageResx.ClientLanguage.game_InstallError;
			}
			else if (downloadState.Equals(LanguageResx.ClientLanguage.Uninstalling_Game))
			{
				languageManager.ChangeCurrentLanguage(currentLanguage);
				SideDownloadState.Text = LanguageResx.ClientLanguage.Uninstalling_Game;
			}
			else if (downloadState.Equals(LanguageResx.ClientLanguage.Uninstalled_Game))
			{
				languageManager.ChangeCurrentLanguage(currentLanguage);
				SideDownloadState.Text = LanguageResx.ClientLanguage.Uninstalled_Game;
			}
			else if (downloadState.Equals(LanguageResx.ClientLanguage.game_NoSpace))
			{
				languageManager.ChangeCurrentLanguage(currentLanguage);
				SideDownloadState.Text = LanguageResx.ClientLanguage.game_NoSpace;
			}

			languageManager.ChangeCurrentLanguage(currentLanguage);
		}

		/// <summary>
		/// Changes the language to spanish and returns the originally selected language by the user
		/// Cambia el idioma a español y devuelve el idioma que esta seleccionado originalmente por el usuario
		/// </summary>
		/// <param name="languageManager">LanguageManager, Class in charge of language changes in the application</param>
		/// <returns>String, original language selected by the user</returns>
		private string ChangeLanguageTemporarily(LanguageManager languageManager)
		{
			Microsoft.Win32.RegistryKey key = windowsRegisterManager.OpenWindowsRegister(true);
			string currentLanguage = (string)key.GetValue("selectedLanguage");

			languageManager.ChangeCurrentLanguage("es-ES");

			return currentLanguage;
		}

		/// <summary>
		/// Change the language stored in the windows registry entry and change the application language
		/// Cambia el idioma almacenado en la entrada del registro de windows y cambia el idioma de la aplicación
		/// </summary>
		/// <param name="sender">Objeto que recibe los eventos</param>
		/// <param name="e">Eventos que le ocurren al objeto</param>
		private void LanguageSelected_SelectionChangeCommitted(object sender, EventArgs e)
		{
			int currentDefaultScreen = DefaultScreen.SelectedIndex;

			LanguageManager languageManager = new LanguageManager();
			languageManager.SelectCurrentAplicationWindow(null, null, null, clientAplication);
			languageManager.LanguageCombobox_LanguageChanged(LanguageSelected);

			DefaultScreen.SelectedIndex = currentDefaultScreen;
		}
		#endregion Language manager

		#region Default Screen manager
		/// <summary>
		/// Call a method that change the default screen stored in the windows register when you change the value in the 
		/// default screen combobox
		/// </summary>
		/// <param name="sender">Objeto que recibe los eventos</param>
		/// <param name="e">Eventos que le ocurren al objeto</param>
		private void DefaultScreen_SelectionChangeCommitted(object sender, EventArgs e)
		{
			SaveDefaultScreen(DefaultScreen.SelectedIndex);
		}

		/// <summary>
		/// Change the default screen stored in the windows register
		/// Cambia la pantalla por defecto almacenada en la entrada del registro de windows
		/// </summary>
		/// <param name="selectedItemIndex"></param>
		private void SaveDefaultScreen(int selectedItemIndex)
		{
			string selectedDefaultScreen = "";

			if (selectedItemIndex == 0)
            {
				selectedDefaultScreen = "Biblioteca de juegos";
			}
			else if (selectedItemIndex == 1)
            {
				selectedDefaultScreen = "Tienda";
			}
			else
            {
				selectedDefaultScreen = "Aleatorio";
			}

			Microsoft.Win32.RegistryKey key = windowsRegisterManager.OpenWindowsRegister(true);
			key.SetValue("selectedDefaultScreen", selectedDefaultScreen);
			windowsRegisterManager.CloseWindowsRegister(key);
		}
		#endregion Default Screen manager

		#endregion Combobox controls and Language manager

		#region Tile Size - Game Filter - Figure Paint

		#region Paint Separative Line and Tile Size Icons
		/// <summary>
		/// Draw a separative line between Tile Size Icons and the games imageview
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">PaintEvents that occur to the object</param>
		private void TileSize_Paint(object sender, PaintEventArgs e)
		{
			Pen colorPen = new Pen(Color.FromArgb(130, 130, 130));
			Point p1 = new Point(30, 48);
			Point p2 = new Point(490, 48);
			e.Graphics.DrawLine(colorPen, p1, p2);
		}

		/// <summary>
		/// Draw the Small Tiles Icon (4 small boxes)
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">PaintEvents that occur to the object</param>
		private void SmallTiles_Paint(object sender, PaintEventArgs e)
		{
			Rectangle rectangleLeftUp = new Rectangle(7, 9, 5, 5);
			Rectangle rectangleRightUp = new Rectangle(17, 9, 5, 5);
			Rectangle rectangleLeftDown = new Rectangle(7, 19, 5, 5);
			Rectangle rectangleRightDown = new Rectangle(17, 19, 5, 5);

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

		/// <summary>
		/// Draw the Medium Tiles Icon (4 medium boxes)
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">PaintEvents that occur to the object</param>
		private void MediumTiles_Paint(object sender, PaintEventArgs e)
		{
			Rectangle rectangleLeftUp = new Rectangle(5, 7, 7, 7);
			Rectangle rectangleRightUp = new Rectangle(17, 7, 7, 7);
			Rectangle rectangleLeftDown = new Rectangle(5, 19, 7, 7);
			Rectangle rectangleRightDown = new Rectangle(17, 19, 7, 7);

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

		/// <summary>
		/// Draw the Large Tiles Icon (4 large boxes)
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">PaintEvents that occur to the object</param>
		private void LargeTiles_Paint(object sender, PaintEventArgs e)
		{
			Rectangle rectangleLeftUp = new Rectangle(4, 5, 9, 9);
			Rectangle rectangleRightUp = new Rectangle(16, 5, 9, 9);
			Rectangle rectangleLeftDown = new Rectangle(4, 18, 9, 9);
			Rectangle rectangleRightDown = new Rectangle(16, 18, 9, 9);

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

		/// <summary>
		/// Fill the box with an opaque color
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="rectangleLeftUp">Rectangle left up of the icon</param>
		/// <param name="rectangleRightUp">Rectangle right up of the icon</param>
		/// <param name="rectangleLeftDown">Rectangle left down of the icon</param>
		/// <param name="rectangleRightDown">Rectangle right down of the icon</param>
		/// <param name="rectangleColor">Color of the filed rectangles</param>
		private void AddGraphics(PaintEventArgs e, Rectangle rectangleLeftUp, Rectangle rectangleRightUp, Rectangle rectangleLeftDown,
			Rectangle rectangleRightDown, Brush rectangleColor)
		{
			e.Graphics.FillRectangle(rectangleColor, rectangleLeftUp);
			e.Graphics.FillRectangle(rectangleColor, rectangleRightUp);
			e.Graphics.FillRectangle(rectangleColor, rectangleLeftDown);
			e.Graphics.FillRectangle(rectangleColor, rectangleRightDown);
		}

		/// <summary>
		/// Invalidate the icons and force them to reload
		/// </summary>
		private void TileSizeButtonsColor()
		{
			SmallTiles.Invalidate();
			MediumTiles.Invalidate();
			LargeTiles.Invalidate();
		}
		#endregion Paint Separative Line and Tile Size Icons

		#region Tile Size Icons Manager
		/// <summary>
		/// Show a tooltip when the mouse enter the icon
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void SmallTiles_MouseEnter(object sender, EventArgs e)
		{
			MultipleResources.ShowToolTip(SmallTiles, LanguageResx.ClientLanguage.smallTiles_Tooltip);
		}

		/// <summary>
		/// Hide the tooltip when the mouse exit the icon
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void SmallTiles_MouseLeave(object sender, EventArgs e)
		{
			MultipleResources.HideToolTip(SmallTiles);
		}

		/// <summary>
		/// Change the game images size to small
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void SmallTiles_Click(object sender, EventArgs e)
		{
			if (!tileSizeSelected.Equals("SmallTile"))
			{
				SmallTiles.Focus();
				tileSizeSelected = "SmallTile";
				SelectTileSize(false, true, false);
			}
		}

		/// <summary>
		/// Show a tooltip when the mouse enter the icon
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void MediumTiles_MouseEnter(object sender, EventArgs e)
		{
			MultipleResources.ShowToolTip(MediumTiles, LanguageResx.ClientLanguage.mediumTiles_Tooltip);
		}

		/// <summary>
		/// Hide the tooltip when the mouse exit the icon
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void MediumTiles_MouseLeave(object sender, EventArgs e)
		{
			MultipleResources.HideToolTip(MediumTiles);
		}

		/// <summary>
		/// Change the game images size to medium
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void MediumTiles_Click(object sender, EventArgs e)
		{
			if (!tileSizeSelected.Equals("MediumTile"))
			{
				MediumTiles.Focus();
				tileSizeSelected = "MediumTile";
				SelectTileSize(false, true, false);
			}
		}

		/// <summary>
		/// Show a tooltip when the mouse enter the icon
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void LargeTiles_MouseEnter(object sender, EventArgs e)
		{
			MultipleResources.ShowToolTip(LargeTiles, LanguageResx.ClientLanguage.largeTiles_Tooltip);
		}

		/// <summary>
		/// Hide the tooltip when the mouse exit the icon
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void LargeTiles_MouseLeave(object sender, EventArgs e)
		{
			MultipleResources.HideToolTip(LargeTiles);
		}

		/// <summary>
		/// Change the game images size to large
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
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
		/// <summary>
		/// Put the mouse cursor to the start of the filter textbox
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void FilterGame_Click(object sender, EventArgs e)
		{
			if (FilterGame.Text.Equals(LanguageResx.ClientLanguage.filterGame) && FilterGame.ForeColor == Color.FromArgb(130, 130, 130))
				FilterGame.SelectionStart = 0;
		}

		/// <summary>
		/// Remove the hint text of the filter textbox
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">KeyEvents that occur to the object</param>
		private void FilterGame_KeyDown(object sender, KeyEventArgs e)
		{
			FilterGame.TextChanged -= FilterGame_TextChanged;

			if (e.KeyCode != Keys.Back && e.KeyCode != Keys.Enter)
			{
				FilterGame.Text = "";
				FilterGame.ForeColor = Color.FromArgb(0, 0, 0);
				FilterGame.KeyDown -= FilterGame_KeyDown;
			}

			FilterGame.TextChanged += FilterGame_TextChanged;
		}

		/// <summary>
		/// Filter the game list by the selected text or put back the hint text if the game filter textbox is empty
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void FilterGame_TextChanged(object sender, EventArgs e)
		{
			if (currentGameImagesListView != null)
			{
				RefillFilteredGames(FilterGame.Text, 0, 0);
			}

			if (FilterGame.Text.Equals(""))
			{
				FilterGame.TextChanged -= FilterGame_TextChanged;

				FilterGame.Text = LanguageResx.ClientLanguage.filterGame;
				FilterGame.ForeColor = Color.FromArgb(130, 130, 130);
				FilterGame.KeyDown += FilterGame_KeyDown;
				FilterGame.SelectionStart = 0;

				FilterGame.TextChanged += FilterGame_TextChanged;
			}
		}

		/// <summary>
		/// Put back the hint text if the filter textbox is empty when the game filter textbox lose focus
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void FilterGame_Leave(object sender, EventArgs e)
		{
			if (FilterGame.Text.Equals(""))
			{
				FilterGame.TextChanged -= FilterGame_TextChanged;

				FilterGame.Text = LanguageResx.ClientLanguage.filterGame;
				FilterGame.ForeColor = Color.FromArgb(130, 130, 130);
				FilterGame.KeyDown += FilterGame_KeyDown;
				FilterGame.SelectionStart = 0;

				FilterGame.TextChanged += FilterGame_TextChanged;
			}
		}
		#endregion Game Filter Manager

		#endregion Tile Size - Game Filter - Figure Paint

		#region Fill Games Listview and Resize Listview Images
		/// <summary>
		/// Set the size and the number of game images shown in the imageview
		/// </summary>
		/// <param name="formStarting">Boolean that indicates if you are changing the game tiles size at the start of the application 
		/// or in the middle of the runtime</param>
		/// <param name="changeSize">Boolean indicating if the user if resizing the game list images</param>
		/// <param name="chargeImages">Boolean indicating if the user if reloading the game list images</param>
		private void SelectTileSize(bool formStarting, bool changeSize, bool chargeImages)
		{
			if (formStarting)
			{
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
							if (FilterGame.Text.Equals(LanguageResx.ClientLanguage.filterGame))
								ResizeGameList(50, 70);
							else
								RefillFilteredGames(FilterGame.Text, 50, 70);
							break;

						case "MediumTile":
							if (FilterGame.Text.Equals(LanguageResx.ClientLanguage.filterGame))
								ResizeGameList(80, 120);
							else
								RefillFilteredGames(FilterGame.Text, 80, 120);
							break;

						case "LargeTile":
							if (FilterGame.Text.Equals(LanguageResx.ClientLanguage.filterGame))
								ResizeGameList(160, 226);
							else
								RefillFilteredGames(FilterGame.Text, 160, 226);
							break;
					}
				}
				else if (chargeImages)
				{
					switch (tileSizeSelected)
					{
						case "SmallTile":
							FillGameList(50, 70);
							break;

						case "MediumTile":
							FillGameList(80, 120);
							break;

						case "LargeTile":
							FillGameList(160, 226);
							break;
					}
				}
			}
		}

		/// <summary>
		/// Fill the game imageview with all the games
		/// </summary>
		/// <param name="xSize">Int, Width of the game image</param>
		/// <param name="ySize">Int, Height of the game image</param>
		private void FillGameList(int xSize, int ySize)
		{
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

			currentGameInfoList.Clear();
			currentGameImagesListView.Items.Clear();
			GameImages.Images.Clear();

			GameImages.ImageSize = new Size(xSize, ySize);
			GameImages.ColorDepth = ColorDepth.Depth32Bit;

			userEmail = GetUserEmail(userName);
			if (userEmail.Equals("error")) userEmail = "";

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

			this.Cursor = System.Windows.Forms.Cursors.Default;
		}

		/// <summary>
		/// Refill the game imageview with another size
		/// </summary>
		/// <param name="xSize">Int, Width of the game image</param>
		/// <param name="ySize">Int, Height of the game image</param>
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

		/// <summary>
		/// Refill the game imageview only with the games that match the filter
		/// </summary>
		/// <param name="xSize">Int, Width of the game image</param>
		/// <param name="ySize">Int, Height of the game image</param>
		private void RefillFilteredGames(string filteredGameName, int xSize, int ySize)
		{
			currentGameImagesListView.Items.Clear();
			GameImages.Images.Clear();
			List<GameInfo> filteredGameInfoList = new List<GameInfo>();

			if (xSize != 0 && ySize != 0)
			{
				GameImages.ImageSize = new Size(xSize, ySize);
				GameImages.ColorDepth = ColorDepth.Depth32Bit;
			}

			foreach (GameInfo currentGameInfo in currentGameInfoList)
			{
				if (currentGameInfo.GameName.ToLower().Contains(filteredGameName.ToLower()) || filteredGameName.Equals(""))
				{
					filteredGameInfoList.Add(currentGameInfo);
				}
			}


			int currentImage = 0;
			foreach (GameInfo currentGameInfo in filteredGameInfoList)
			{
				currentGameImagesListView.Items.Add(currentGameInfo.GameName, currentImage);
				GameImages.Images.Add(currentGameInfo.GameImage);

				currentImage++;
			}

			currentGameImagesListView.LargeImageList = GameImages;
		}
		#endregion Fill Games Listview and Resize Listview Images

		#region Listview Manager and Game Info Button (Play - Download - Buy Button)
		/// <summary>
		/// Show the game information panel when you leftclick on the game image in the imageview or open a dropdown menu when
		/// you rightclick
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">MouseEvents that occur to the object</param>
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

		/// <summary>
		/// Install or launch the game when you double click (Useless)
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">MouseEvents that occur to the object</param>
		private void GamesListView_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			//if (libraryOpen) Launch / Install game
		}

		/// <summary>
		/// Buy the game if you entered the game information panel from the store or allow you to download / play / uninstall
		/// the game if you entered the game information panel from the game library
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">MouseEvents that occur to the object</param>
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
							GenericPopUpMessage(LanguageResx.ClientLanguage.events_Database_ConnectionError);
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
						GenericPopUpMessage(LanguageResx.ClientLanguage.buyGameSucess);
					}

					CloseLoadingScreen(false);
				}
				else if (libraryOpen && !Play_BuyGame.Text.Equals(LanguageResx.ClientLanguage.game_Downloading_LowerCase) &&
					!Play_BuyGame.Text.Equals(LanguageResx.ClientLanguage.InQueue))
				{
					if (Play_BuyGame.Text.Equals(LanguageResx.ClientLanguage.play_Button))
					{
						var process = new System.Diagnostics.Process();
						process.StartInfo.WorkingDirectory = Path.Combine(downloadPath, currentGameInfo.GameName);
						process.StartInfo.FileName = currentGameInfo.GameName + ".exe";
						process.Start();
					}
					else if (Play_BuyGame.Text.Equals(LanguageResx.ClientLanguage.download_Avaible_Button))
					{
						if (downloadingGame == null)
						{
							downloadingGame = currentGameInfo;
							Download_InstallGame();
						}
						else
						{
							if (currentGameInfo.GameName != downloadingGame.GameName)
							{
								bool coincidences = false;
								foreach (GameInfo currentQueueGame in queueGames)
								{
									if (currentQueueGame.GameName == currentGameInfo.GameName)
									{
										coincidences = true;
									}
								}

								if (!coincidences)
								{
									queueGames.Add(currentGameInfo);
								}
							}
						}
						ChangeScreen(ref gameInfoOpen, true);
					}
					else if (Play_BuyGame.Text.Equals(LanguageResx.ClientLanguage.gameSettingsMenu_Uninstall))
					{
						UninstallGame();
					}
				}
			}
		}
		#endregion Listview Manager and Game Info Button (Play - Buy Button)

		#region Others

		#region Lost Focus
		/// <summary>
		/// Focus the SideMenu to enable the funtionality of focus lose
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void LostFocus_Click(object sender, EventArgs e)
		{
			SideMenu.Focus();
		}
		#endregion Lost Focus

		#region Mouse is over control
		/// <summary>
		/// Catch if the mouse entered certain object passed by param
		/// </summary>
		/// <param name="currentObject">Object we want to know if the mouse is over it</param>
		/// <returns>Boolean that indicates if the mouse is over the control or not</returns>
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
		/// <summary>
		/// Close the game information panel
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void CloseGameInfo_Click(object sender, EventArgs e)
		{
			gameInfoOpen = false;
			GameInfoMenu.Visible = gameInfoOpen;
		}

		/// <summary>
		/// Refresh the game information panel button text according to the current state of the game (downloading, installing, play...)
		/// </summary>
		/// <param name="state">String, state of the game (downloading, installing, play...)</param>
		private void RefreshGameInfo_Click(string state)
        {
			Play_BuyGame.Text = state;
		}
		#endregion Close Game Info

		#region Popup events
		/// <summary>
		/// Shows a popup with a custom message
		/// </summary>
		/// <param name="eventText">String, custom message</param>
		private void GenericPopUpMessage(string eventText)
		{
			EventText.Location = new Point(20, 15);
			EventText.Size = new Size(560, 120);
			EventCode.Visible = false;
			EventSendButton.Visible = false;
			EventExitButton.Location = new Point(237, 145);
			EventText.Text = eventText;
			EventsPanel.Visible = true;
		}

		/// <summary>
		/// Shows a popup with a custom message and other controls
		/// </summary>
		/// <param name="eventText">String, custom message</param>
		private void GenericPopUpCode(string eventText)
		{
			EventText.Location = new Point(20, 10);
			EventText.Size = new Size(560, 57);
			EventCode.Visible = true;
			EventSendButton.Visible = true;
			EventExitButton.Location = new Point(305, EventSendButton.Location.Y);
			EventText.Text = eventText;
			EventsPanel.Visible = true;
			EventCode.Focus();
		}

		/// <summary>
		/// Check if the code you writted is correct, if it is right then delete the account and takes you to the login screen
		/// </summary>
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
						GenericPopUpMessage(LanguageResx.ClientLanguage.events_Database_ConnectionError);
					}
					else
					{
						GenericPopUpMessage(queryError);
					}
				}
				else
				{
					try
					{
						Microsoft.Win32.RegistryKey key = windowsRegisterManager.OpenWindowsRegister(true);
						key.DeleteValue("KeepSessionOpen");
						key.DeleteValue("Username");
					}
					catch (Exception)
					{ }

					MultipleResources.RestartApp(Process.GetCurrentProcess().Id, Process.GetCurrentProcess().ProcessName);
				}

				CloseLoadingScreen(false);
			}
			else
			{
				EventCodeError.Visible = true;
			}
		}

		/// <summary>
		/// Close the popup
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">MouseEvents that occur to the object</param>
		private void EventExitButton_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				HideImageGradient();

				EventCode.Text = "";
				EventCodeError.Visible = false;
			}
		}

		/// <summary>
		/// Enlighten the send / exit button text in the popup
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void EventSend_ExitButton_MouseEnter(object sender, EventArgs e)
		{
			((Label)sender).Font = new Font("Oxygen", 10, FontStyle.Bold);
		}

		/// <summary>
		/// De-Enlighten the send / exit button text in the popup
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>	
		private void EventSend_ExitButton_MouseLeave(object sender, EventArgs e)
		{
			((Label)sender).Font = new Font("Oxygen", 10, FontStyle.Regular);
		}

		/// <summary>
		/// Call the method to check the entered code when you click on send button
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">MouseEvents that occur to the object</param>	
		private void EventSendButton_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				CheckHashResumes();
			}
		}

		/// <summary>
		/// Call the method to check the entered code when you press enter key
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">KeyEvents that occur to the object</param>
		private void EventCode_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				CheckHashResumes();
			}
		}

		/// <summary>
		/// Make every letter you write in the code textbox uppercase
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void EventCode_TextChanged(object sender, EventArgs e)
		{
			EventCode.Text = EventCode.Text.ToUpper();
			EventCode.Select(EventCode.Text.Length, 0);
		}
		#endregion Popup events

		#region Show and Hide Image Gradient Panel
		// Methods to show Image Gradient Panel
		/// <summary>
		/// Show darkened image and popup
		/// </summary>
		private void ShowImageGradient()
		{
			ConfigurationPanel.Visible = false;
			EventsPanel.Visible = false;

			SideMenu.Visible = false;
			MainMenu.Visible = false;
		}

		/// <summary>
		/// Hide darkened image and popup
		/// </summary
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
		/// <summary>
		/// Show darkened image and set wait cursor
		/// </summary>
		private void OpenLoadingScreen()
		{
			ShowImageGradient();
			ImageGradient.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			operationInProgress = true;
		}

		/// <summary>
		/// Hide darkened image if desired and set default cursor
		/// </summary>
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
		/// <summary>
		/// Show user settings dropdown menu
		/// </summary>
		private void ShowUserInfoMenu()
		{
			ContextMenuStrip.Name = "UserInfoMenu";
			ContextMenuStrip.Items.Clear();

			ContextMenuStrip.Items.Add(LanguageResx.ClientLanguage.userInfoMenu_Settings);
			ContextMenuStrip.Items.Add(new ToolStripSeparator());
			ContextMenuStrip.Items.Add(LanguageResx.ClientLanguage.download);
			ContextMenuStrip.Items.Add(new ToolStripSeparator());
			ContextMenuStrip.Items.Add(LanguageResx.ClientLanguage.userInfoMenu_logout);

			ContextMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.UserSettingsMenuStrip_ItemClicked);
			ContextMenuStrip.Show(UserInformation, new Point(UserInformation.Width, +10));
		}

		/// <summary>
		/// Hide user settings dropdown menu
		/// </summary>
		private void HideUserInfoMenu()
		{
			ContextMenuStrip.ItemClicked -= this.UserSettingsMenuStrip_ItemClicked;
			ContextMenuStrip.Hide();
			ContextMenuStrip.Items.Clear();
		}

		/// <summary>
		/// Executes the desired action depending on the option pressed
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">ToolStripClickEvents that occur to the object</param>
		private void UserSettingsMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			if (e.ClickedItem.Text == LanguageResx.ClientLanguage.userInfoMenu_Settings)
			{
				OpenConfiguration();
			}
			else if (e.ClickedItem.Text == LanguageResx.ClientLanguage.download)
			{
				ChangeScreen(ref downloadsInfoOpen, false);
				LoadDownloadsPanel();
			}
			else if (e.ClickedItem.Text == LanguageResx.ClientLanguage.userInfoMenu_logout)
			{
				aplicationClosing = true;
				Microsoft.Win32.RegistryKey key = windowsRegisterManager.OpenWindowsRegister(true);

				bool keepSessionOpen = Convert.ToBoolean(key.GetValue("KeepSessionOpen"));
				key.SetValue("selectedTileSize", tileSizeSelected);

				if (keepSessionOpen)
				{
					key.DeleteValue("KeepSessionOpen");
					key.DeleteValue("Username");
				}

				MultipleResources.RestartApp(Process.GetCurrentProcess().Id, Process.GetCurrentProcess().ProcessName);
				this.Close();
			}
		}
		#endregion User Settings Menu

		#region Game Settings Menu
		/// <summary>
		/// Show game settings dropdown menu
		/// </summary>
		private void GameSettings_Click(object sender, EventArgs e)
		{
			ShowGameSettingsMenu(GameSettingsBackground.Width + 3, 3, GameSettingsBackground);
		}

		/// <summary>
		/// Show the game settings dropdown menu according to the current state of the game (downloading, installing, play...)
		/// </summary>
		/// <param name="pointX">Int, xPosition where the dropdown menu will be shown</param>
		/// <param name="pointY">Int, yPosition where the dropdown menu will be shown</param>
		/// <param name="panelAttached">Panel in which the dropdown menu appears</param>
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
				ContextMenuStrip.Items.Add(new ToolStripMenuItem(LanguageResx.ClientLanguage.play_Button));
				ContextMenuStrip.Items.Add(new ToolStripSeparator());
				ContextMenuStrip.Items.Add(new ToolStripMenuItem(LanguageResx.ClientLanguage.gameSettingsMenu_SynchronizeProgress));
				ContextMenuStrip.Items.Add(new ToolStripSeparator());
				ContextMenuStrip.Items.Add(new ToolStripMenuItem(LanguageResx.ClientLanguage.gameSettingsMenu_DownloadProgress));
				ContextMenuStrip.Items.Add(new ToolStripSeparator());
				ContextMenuStrip.Items.Add(LanguageResx.ClientLanguage.gameSettingsMenu_Uninstall);

				string directoryPath = Path.Combine(downloadPath, currentGameInfo.GameName, "Files");
				if (Directory.Exists(directoryPath))
				{
					if (!File.Exists(Path.Combine(directoryPath, "PlayerInventory.json")))
					{
						ContextMenuStrip.Items[2].Enabled = false;
					}
				}
				else
				{
					ContextMenuStrip.Items[2].Enabled = false;
					ContextMenuStrip.Items[4].Enabled = false;
				}
			}
			else
			{
				if (!currentGameInfo.GameDownloadLink.Equals(""))
				{
					if (downloadingGame == null) // No hay juegos descargandose y el juego se puede descargar
					{
						ContextMenuStrip.Items.Add(new ToolStripMenuItem(LanguageResx.ClientLanguage.download_Avaible_Button));
					}
					else
					{
						foreach (GameInfo currentQueueGame in queueGames)
						{
							if (currentQueueGame.GameName == currentGameInfo.GameName) // El juego ya esta en la cola de descargas
							{
								ContextMenuStrip.Items.Add(new ToolStripMenuItem(LanguageResx.ClientLanguage.InQueue));
							}
						}

						if (ContextMenuStrip.Items.Count <= 0) // No hay juegos en la cola
						{
							if (downloadingGame.GameName == currentGameInfo.GameName) // El juego se esta descargando
                            {
								ContextMenuStrip.Items.Add(new ToolStripMenuItem(LanguageResx.ClientLanguage.game_Downloading_LowerCase));
							}
							else // El juego se puede descargar
                            {
								ContextMenuStrip.Items.Add(new ToolStripMenuItem(LanguageResx.ClientLanguage.download_Avaible_Button));
							}
						}
					}
				}
				else
				{
					ContextMenuStrip.Items.Add(new ToolStripMenuItem(LanguageResx.ClientLanguage.download_Unavaible_Button));
				}
			}

			ContextMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.GameSettingsMenuStrip_ItemClicked);
			if (panelAttached == null) ContextMenuStrip.Show(pointX, pointY);
			if (panelAttached != null) ContextMenuStrip.Show(panelAttached, new Point(pointX, pointY));
		}

		/// <summary>
		/// Executes the desired action depending on the option pressed
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">ToolStripClickEvents that occur to the object</param>
		private void GameSettingsMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			if (e.ClickedItem.Text.Equals(LanguageResx.ClientLanguage.play_Button))
			{
				var process = new System.Diagnostics.Process();
				process.StartInfo.WorkingDirectory = Path.Combine(downloadPath, currentGameInfo.GameName);
				process.StartInfo.FileName = currentGameInfo.GameName + ".exe";
				process.Start();
			}
			else if (e.ClickedItem.Text.Equals(LanguageResx.ClientLanguage.download_Avaible_Button))
			{
				if (!e.ClickedItem.Text.Equals(LanguageResx.ClientLanguage.game_Downloading_LowerCase) &&
					!e.ClickedItem.Text.Equals(LanguageResx.ClientLanguage.InQueue))
				{
					if (downloadingGame == null)
					{
						downloadingGame = currentGameInfo;
						Download_InstallGame();
					}
					else
					{
						if (currentGameInfo.GameName != downloadingGame.GameName)
                        {
							bool coincidences = false;
							foreach (GameInfo currentQueueGame in queueGames)
							{
								if (currentQueueGame.GameName == currentGameInfo.GameName)
								{
									coincidences = true;
								}
							}

							if (!coincidences)
                            {
								queueGames.Add(currentGameInfo);
							}
						}
					}
				}
			}
			// Guardar progreso en la base de datos
			else if (e.ClickedItem.Text.Equals(LanguageResx.ClientLanguage.gameSettingsMenu_SynchronizeProgress))
			{
				string filePath = Path.Combine(downloadPath, currentGameInfo.GameName, "Files", "PlayerInventory.json");
				if (File.Exists(filePath))
				{
					OpenLoadingScreen();
					byte[] gameProgressBytes = System.IO.File.ReadAllBytes(filePath);
					string queryError = SaveGameProgress(gameProgressBytes, currentGameInfo.GameName);

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
					}
					else
					{
						GenericPopUpMessage(LanguageResx.ClientLanguage.gameSettingsMenu_SynchronizedProgress);
					}
					CloseLoadingScreen(false);
				}
			}
			// Descargar Progreso de la base de datos
			else if (e.ClickedItem.Text.Equals(LanguageResx.ClientLanguage.gameSettingsMenu_DownloadProgress))
			{
				OpenLoadingScreen();
				string filePath = Path.Combine(downloadPath, currentGameInfo.GameName, "Files", "PlayerInventory.json");
				byte[] gameProgressBytes = DownloadGameProgress(currentGameInfo.GameName, filePath);

				if (gameProgressBytes == null)
				{
					GenericPopUpMessage(LanguageResx.ClientLanguage.events_Database_ConnectionError);
				}
				else if (gameProgressBytes.Length == 0)
				{
					GenericPopUpMessage(LanguageResx.ClientLanguage.events_Database_NoProgressFound);
				}
				else
				{
					if (File.Exists(filePath))
					{
						if (File.ReadAllBytes(filePath).Length == gameProgressBytes.Length)
						{
							File.WriteAllBytes(filePath, gameProgressBytes);
						}
						else
						{
							File.WriteAllText(filePath, String.Empty);
							File.WriteAllBytes(filePath, gameProgressBytes);
						}
						GenericPopUpMessage(LanguageResx.ClientLanguage.gameSettingsMenu_DownloadedProgress);
					}
					else
					{
						File.Create(filePath).Close();
						if (File.Exists(filePath))
						{
							File.WriteAllBytes(filePath, gameProgressBytes);
							GenericPopUpMessage(LanguageResx.ClientLanguage.gameSettingsMenu_DownloadedProgress);
						}
						else
						{
							GenericPopUpMessage(LanguageResx.ClientLanguage.fileError);
						}
					}
				}
				CloseLoadingScreen(false);
			}
			else if (e.ClickedItem.Text.Equals(LanguageResx.ClientLanguage.gameSettingsMenu_Uninstall))
			{
				UninstallGame();
			}
		}
		#endregion Game Settings Menu

		/// <summary>
		/// Hide user settings dropdown menu if the cursor exit the dropdown menu
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
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

		#region SQL Querys
		/// <summary>
		/// Check from the database the owned games of the user
		/// </summary>
		/// <returns>List<GameInfo> with all the owned games of the user</returns>
		private List<GameInfo> CheckOwnedGames()
		{
			string sqlQuery = "SELECT G_A.gameName, G_A.gameCover, G_A.gamePrice, G_A.gameDownloadLink " +
				"FROM games_avaibles G_A INNER JOIN games_owned G_O ON G_A.gameName = G_O.gameName " +
				"Where G_O.user_email LIKE '" + userEmail + "'";

			return SQLManager.ReadGameList(sqlQuery);
		}

		/// <summary>
		/// Check from the database the games that the user don't own
		/// </summary>
		/// <returns>List<GameInfo> with all the avaible games</returns>
		private List<GameInfo> CheckAvaibleGames()
		{
			string sqlQuery = "SELECT G_A.gameName, G_A.gameCover, G_A.gamePrice, G_A.gameDownloadLink FROM games_avaibles G_A " +
				"WHERE NOT EXISTS (SELECT G_O.gameName FROM games_owned G_O " +
				"WHERE G_A.gameName = G_O.gameName AND G_O.user_email Like '" + userEmail + "')";

			return SQLManager.ReadGameList(sqlQuery);
		}

		/// <summary>
		/// Get the user email from the username
		/// </summary>
		/// <param name="userName">string, username</param>
		/// <returns>string, query state</returns>
		private string GetUserEmail(string userName)
		{
			string sqlQuery = "SELECT user_email FROM user_information WHERE user_name LIKE '" + userName + "'";
			return SQLManager.SearchQueryData(sqlQuery);
		}

		/// <summary>
		/// Save user game progress in the database
		/// </summary>
		/// <param name="gameProgress">byte[], file content</param>
		/// <param name="gameName">string, game name</param>
		/// <returns>string, query state</returns>
		private string SaveGameProgress(byte[] gameProgress, string gameName)
		{
			return SQLManager.WriteGameProgress(gameProgress, userEmail, gameName);
		}

		/// <summary>
		/// Download the user game progress from the database
		/// </summary>
		/// <param name="gameName">string, game name</param>
		/// <returns>byte[], file content</returns>
		private byte[] DownloadGameProgress(string gameName, string filePath)
		{
			string sqlQuery = "SELECT gameProgression FROM games_owned WHERE user_email LIKE '" + userEmail + "' AND gameName LIKE '" + gameName + "'";
			return SQLManager.ReadGameProgress(sqlQuery);

		}
		#endregion SQL Querys

		#region Downloads / Installations - Uninstallations - File manager

		#region Downloads / Installations
		/// <summary>
		/// Async Task, Check if you have enough space in your disk, manage all download states (like downloading or catching all type of errors)
		/// and manage the download queue and the game download process, also show the download the download info side panel
		/// </summary>
		private async void Download_InstallGame()
		{
			long requeriedDriveSpace = 600000000; // 600 MB in bytes
			LanguageManager languageManager = new LanguageManager();
			downloadError = false;

			if (gameInfoOpen)
			{
				RefreshGameInfo_Click(LanguageResx.ClientLanguage.game_Downloading_LowerCase);
			}
			ContextMenuStrip.Hide();

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
							Uninstall_Information.Location = new Point(0, 211);
							Store.Location = new Point(0, 49);
							GameLibrary.Location = new Point(0, 130);
						}

						this.downloadProgress = 0;
						CloseShowDownloadInformation.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.arrow;
						SideDownloadInformationGameImage.BackgroundImage = downloadingGame.GameImage;
						SideDownloadInformationGameName.Text = downloadingGame.GameName;

						DownloadInformation.Visible = true;

						SideDownloadState.ForeColor = Color.FromArgb(230, 230, 230);

						languageManager.ChangeCurrentLanguage("es-ES");
						downloadState = LanguageResx.ClientLanguage.game_Downloading_Uppercase;
						await SetDownloadsPanelState(false, this.downloadProgress);
						CheckDownload_UninstallInformationLanguage();
						try
						{
							await Task.Run(async () =>
							{
								await DownloadGameFromMega(SideDownloadProgressbar, downloadingGame);
							});
						}
						catch (OperationCanceledException)
                        {
							languageManager.ChangeCurrentLanguage("es-ES");
							downloadState = LanguageResx.ClientLanguage.download_canceled;
							await SetDownloadsPanelState(false, 100);
							CheckDownload_UninstallInformationLanguage();
							downloadError = true;

							SideDownloadProgressbar.Style = ProgressBarStyle.Continuous;
							SideDownloadProgressbar.Value = 100;

							DeleteCorruptedFiles();
							break;
						}
						catch (Exception)
						{
							SideDownloadState.ForeColor = Color.Red;
							languageManager.ChangeCurrentLanguage("es-ES");
							downloadState = LanguageResx.ClientLanguage.game_DownloadError;
							await SetDownloadsPanelState(false, 100);
							CheckDownload_UninstallInformationLanguage();
							downloadError = true;

							SideDownloadProgressbar.Style = ProgressBarStyle.Continuous;
							SideDownloadProgressbar.Value = 100;

							DeleteCorruptedFiles();
							break;
						}

						SideDownloadProgressbar.Style = ProgressBarStyle.Marquee;
						SideDownloadProgressbar.Value = 40;
						languageManager.ChangeCurrentLanguage("es-ES");
						downloadState = LanguageResx.ClientLanguage.game_Install;
						await SetDownloadsPanelState(true, 80);
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
							SideDownloadState.ForeColor = Color.Red;
							languageManager.ChangeCurrentLanguage("es-ES");
							downloadState = LanguageResx.ClientLanguage.game_InstallError;
							await SetDownloadsPanelState(false, 100);
							CheckDownload_UninstallInformationLanguage();
							downloadError = true;

							SideDownloadProgressbar.Style = ProgressBarStyle.Continuous;
							SideDownloadProgressbar.Value = 100;

							DeleteCorruptedFiles();
							break;
						}

						languageManager.ChangeCurrentLanguage("es-ES");
						downloadState = LanguageResx.ClientLanguage.game_DownloadSucess;
						await SetDownloadsPanelState(false, 100);
						CheckDownload_UninstallInformationLanguage(); // Devuelve la aplicación al idioma elegido por el usuario

						SideDownloadProgressbar.Style = ProgressBarStyle.Continuous;
						SideDownloadProgressbar.Value = 100;
					}
					else
					{
						SideDownloadState.ForeColor = Color.Red;
						languageManager.ChangeCurrentLanguage("es-ES");
						downloadState = LanguageResx.ClientLanguage.game_NoSpace;
						await SetDownloadsPanelState(false, 100);
						CheckDownload_UninstallInformationLanguage(); // Devuelve la aplicación al idioma elegido por el usuario
						downloadError = true;

						SideDownloadProgressbar.Style = ProgressBarStyle.Continuous;
						SideDownloadProgressbar.Value = 100;
					}
				}
			}

			if (gameInfoOpen && !downloadError)
            {
				RefreshGameInfo_Click(LanguageResx.ClientLanguage.play_Button);
			}
			else if (gameInfoOpen && downloadError)
            {
				RefreshGameInfo_Click(LanguageResx.ClientLanguage.download_Avaible_Button);
			}
			ContextMenuStrip.Hide();

			if (queueGames.Count == 0)
            {
				downloadingGame = null;
				CloseShowDownloadInformation.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.close;
			}
			else
            {
				downloadingGame = queueGames[0];
				queueGames.RemoveAt(0);

				Download_InstallGame();
			}

			if (DownloadsPanel.Visible)
			{
				Invoke(new MethodInvoker(() =>
				{
					LoadDownloadsPanel();
				}));
			}
		}

		/// <summary>
		/// Async Task, Connect to Mega API to start the download of a game,enables the posibility to cancel the download and refresh the download
		/// state values (download percentage and state, preparing download, downloading...)
		/// </summary>
		/// <param name="downloadProgress">Progress bar, indicates the download progress</param>
		/// <param name="downloadingGameInfo">GameInfo, information of the game downloading (game name, download link...)</param>
		/// <returns>Task, task in execution</returns>
		private async Task DownloadGameFromMega(ProgressBar downloadProgress, GameInfo downloadingGameInfo)
		{
			MegaApiClient mega = new MegaApiClient();
			mega.LoginAnonymous();

			Uri fileLink = new Uri(downloadingGame.GameDownloadLink);

			INodeInfo node = mega.GetNodeFromLink(fileLink);
			downloadGameName = node.Name;

			if (File.Exists(Path.Combine(downloadPath, downloadGameName)))
			{
				File.Delete(Path.Combine(downloadPath, downloadGameName));
			}

			Progress<double> progress = new Progress<double>();
			progress.ProgressChanged += (s, progressValue) =>
			{
				//Update the UI with the progressValue 
				Invoke(new MethodInvoker(() =>
				{
					downloadProgress.Value = Convert.ToInt32(progressValue);
					this.downloadProgress = Convert.ToInt32(progressValue);

					if (DownloadsPanel.Visible)
                    {
						// Consigo la referencia del progressbar del panel de descargas y creo un label de porcentaje
						Control[] downloadingPanelControls = DownloadsPanel.Controls.Find("DownloadingPanel", true);

						Control[] progressbarControl = downloadingPanelControls[0].Controls.Find("DownloadProgressbar", false);
						ProgressBar downloadInfoProgressbar = (ProgressBar)progressbarControl[0];

						downloadInfoProgressbar.Value = Convert.ToInt32(progressValue);

						Label downloadInfoProgress = null;

						try
                        {
							Control[] downloadProgressControl = downloadingPanelControls[0].Controls.Find("DownloadProgress", false);
							downloadInfoProgress = (Label)downloadProgressControl[0];
						} catch (Exception)
                        {
							downloadInfoProgress = MultipleResources.CreateGenericLabel("DownloadProgress", true, 31, 14, 440, 23, 8,
								ContentAlignment.MiddleRight);
							downloadingPanelControls[0].Controls.Add(downloadInfoProgress);
						}
                        finally
                        {
							downloadInfoProgress.Text = Convert.ToInt32(progressValue).ToString() + " %";
						}

					}
				}));
			};

			// Reset cancel token in case it has been used
			CancellationTokenSource downloadCancellationTokenSource = new CancellationTokenSource();
			if (downloadCancellationTokenSource.IsCancellationRequested)
			{
				downloadCancellationTokenSource.Dispose();
				downloadCancellationTokenSource = new CancellationTokenSource();
			}

			downloadingGameInfo.DownloadCancellationTokenSource = downloadCancellationTokenSource;
			downloadingGame = downloadingGameInfo;

			// Upload With progress bar
			await mega.DownloadFileAsync(fileLink, Path.Combine(downloadPath, node.Name), progress, downloadCancellationTokenSource.Token);

			mega.Logout();
		}

		/// <summary>
		/// Async Task, unzip the downloaded game and when the zip is unzipped deletes the zip
		/// </summary>
		/// <returns>Task, task in execution</returns>
		private async Task Install_Game()
		{
			string zipPath = Path.Combine(downloadPath, downloadGameName);
			string extractPath = downloadPath;

			ZipFile.ExtractToDirectory(zipPath, extractPath);

			File.Delete(Path.Combine(downloadPath, downloadGameName));
		}

		/// <summary>
		/// Cancel the current download
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void CancelDownload_Click(object sender, EventArgs e)
		{
			if (!downloadingGame.DownloadCancellationTokenSource.IsCancellationRequested)
				downloadingGame.DownloadCancellationTokenSource.Cancel();
		}

		/// <summary>
		/// Cancel a game in the queue
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void CancelQueue_Click(object sender, EventArgs e)
        {
			string controlName = ((Panel)sender).Name;
			int arrayPosition = Convert.ToInt32(controlName.Split('_')[1]);
			queueGames.RemoveAt(arrayPosition);
			LoadDownloadsPanel();
		}

		/// <summary>
		/// Raise the download priority of a queued game
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void RankUpQueue_Click(object sender, EventArgs e)
		{
			string controlName = ((Panel)sender).Name;
			int arrayPosition = Convert.ToInt32(controlName.Split('_')[1]);

			if (arrayPosition > 0)
            {
				queueGames.Reverse(arrayPosition - 1, 2);
				ReloadQueue();
			}
		}

		/// <summary>
		/// Lower the download priority of a queued game
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void RankDownQueue_Click(object sender, EventArgs e)
		{
			string controlName = ((Panel)sender).Name;
			int arrayPosition = Convert.ToInt32(controlName.Split('_')[1]);

			if ((arrayPosition + 1) < queueGames.Count)
            {
				queueGames.Reverse(arrayPosition, 2);
				ReloadQueue();
			}
		}

		/// <summary>
		/// Async Task, Refresh the download information panel when the download change the current state (like from downloading to installing)
		/// </summary>
		/// <param name="infiniteProgressbar">Bool, true if the state is installing and false if the progress bar have a certain value (like 90%)</param>
		/// <param name="progressbarValue">Int, set the value of the progress bar</param>
		/// <returns>Task, task in execution</returns>
		private async Task SetDownloadsPanelState(bool infiniteProgressbar, int progressbarValue)
		{
			Invoke(new MethodInvoker(() =>
			{
				if (DownloadsPanel.Visible)
				{
					Control[] downloadingPanelControls = DownloadsPanel.Controls.Find("DownloadingPanel", true);
					Control[] stateLabelControl = downloadingPanelControls[0].Controls.Find("DownloadState", false);
					Label downloadStateLabel = (Label)stateLabelControl[0];

					if (downloadError)
					{
						downloadStateLabel.ForeColor = Color.Red;
					}

					downloadStateLabel.Text = downloadState;

					Control[] progressbarControl = downloadingPanelControls[0].Controls.Find("DownloadProgressbar", false);
					ProgressBar downloadInfoProgressbar = (ProgressBar)progressbarControl[0];

					if (infiniteProgressbar)
					{
						downloadInfoProgressbar.Style = ProgressBarStyle.Marquee;
						downloadInfoProgressbar.Value = progressbarValue;
						downloadInfoProgressbar.Step = 40;
					}
					else
					{
						downloadInfoProgressbar.Style = ProgressBarStyle.Blocks;
						downloadInfoProgressbar.Value = progressbarValue;
						downloadInfoProgressbar.Step = 10;
					}
				}
			}));
		}

		/// <summary>
		/// Shows the downloads panel when you can see the current download and every queued game, adds tooltips to some buttons and
		/// notice the user when there is no game in the queue or currently downloanding
		/// </summary>
		private void LoadDownloadsPanel()
		{
			QueueLayout.Controls.Clear();
			DownloadingGameHeader.Visible = true;
			QueueGameHeader.Visible = true;
			DownloadsNoInfoPanel.Visible = false;
			DownloadsNoInfoLabel.Visible = false;

			DownloadsPanel.Controls.RemoveByKey("DownloadingPanel");

			if (downloadingGame != null)
			{
				Panel downloadingPanel = new DownloadingGameInformationSmall(downloadingGame.GameImage, downloadingGame.GameName);
				downloadingPanel.Location = new Point(21, 40);
				downloadingPanel.Name = "DownloadingPanel";
				DownloadsPanel.Controls.Add(downloadingPanel);

				Panel cancelDownload = downloadingPanel.Controls.Find("DownloadCancelButton", false)[0] as Panel;
				cancelDownload.Click += new EventHandler(CancelDownload_Click);

				Label downloadStateLabel = MultipleResources.CreateGenericLabel("DownloadState", true, 366, 14, 57, 41, 8,
					ContentAlignment.MiddleLeft);
				downloadStateLabel.Text = downloadState;
				downloadingPanel.Controls.Add(downloadStateLabel);

				Label downloadInfoProgress = null;
				try
				{
					downloadInfoProgress = downloadingPanel.Controls.Find("DownloadProgress", false)[0] as Label;
				}
				catch (Exception)
				{
					downloadInfoProgress = MultipleResources.CreateGenericLabel("DownloadProgress", true, 45, 19, 630, 32, 10,
						ContentAlignment.MiddleRight);
					downloadingPanel.Controls.Add(downloadInfoProgress);
				}
				finally
				{
					downloadInfoProgress.Text = downloadProgress.ToString() + " %";
				}

				LanguageManager languageManager = new LanguageManager();
				string currentLanguage = ChangeLanguageTemporarily(languageManager);
				if (downloadState.Equals(LanguageResx.ClientLanguage.game_Install))
				{
					languageManager.ChangeCurrentLanguage(currentLanguage);
					SetDownloadsPanelState(true, 40);
				}
				else
				{
					languageManager.ChangeCurrentLanguage(currentLanguage);
					SetDownloadsPanelState(false, this.downloadProgress);
				}

				if (queueGames.Count > 0)
				{
					for (int currentDownloadGameInfo = 0; currentDownloadGameInfo < queueGames.Count; currentDownloadGameInfo++)
                    {
						Panel queuePanel = new QueueGameInformationSmall(queueGames[currentDownloadGameInfo].GameImage,
							queueGames[currentDownloadGameInfo].GameName);

						Label queueStateLabel = MultipleResources.CreateGenericLabel("QueueState", true, 293, 14, 59, 40, 8,
							ContentAlignment.MiddleLeft);
						queueStateLabel.Text = LanguageResx.ClientLanguage.currentPriority + "  " + (currentDownloadGameInfo + 1);
						queuePanel.Controls.Add(queueStateLabel);

						Panel cancelQueueDownload = queuePanel.Controls.Find("QueueCancelButton", false)[0] as Panel;
						cancelQueueDownload.Name = "QueueCancelButton_" + currentDownloadGameInfo;

						Panel rankUpQueueDownload = queuePanel.Controls.Find("QueueRankUp", false)[0] as Panel;
						rankUpQueueDownload.Name = "QueueRankUp_" + currentDownloadGameInfo;

						Panel rankDownQueueDownload = queuePanel.Controls.Find("QueueRankDown", false)[0] as Panel;
						rankDownQueueDownload.Name = "QueueRankDown_" + currentDownloadGameInfo;

						cancelQueueDownload.Click += new EventHandler(CancelQueue_Click);
						rankUpQueueDownload.Click += new EventHandler(RankUpQueue_Click);
						rankDownQueueDownload.Click += new EventHandler(RankDownQueue_Click);

						QueueLayout.Controls.Add(queuePanel);
					}
				}
				else
				{
					DownloadsNoInfoPanel.Visible = true;
					MultipleResources.CalculateHalfSize(QueueLayout, DownloadsNoInfoPanel, 0.35m, 0.60m);
					MultipleResources.CalculateCenterLocation(QueueLayout, DownloadsNoInfoPanel, 150);
					DownloadsNoInfoPanel.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.NoDownloadQueue;

					DownloadsNoInfoLabel.Visible = true;
					DownloadsNoInfoLabel.AutoSize = true;
					DownloadsNoInfoLabel.Text = LanguageResx.ClientLanguage.NoDownloadQueue;
					DownloadsNoInfoLabel.MinimumSize = new Size(DownloadsNoInfoPanel.Size.Width, 0);

					DownloadsNoInfoLabel.Location = new Point(
						QueueLayout.Width / 2 - DownloadsNoInfoLabel.Size.Width / 2,
						DownloadsNoInfoPanel.Location.Y + DownloadsNoInfoPanel.Size.Height + 20);
				}
			}
			else
			{
				DownloadingGameHeader.Visible = false;
				QueueGameHeader.Visible = false;
				DownloadsNoInfoPanel.Visible = true;
				MultipleResources.CalculateHalfSize(DownloadsPanel, DownloadsNoInfoPanel, 0.50m, 0.65m);
				MultipleResources.CalculateCenterLocation(DownloadsPanel, DownloadsNoInfoPanel, 0);
				DownloadsNoInfoPanel.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.NoCurrentDownloading;

				DownloadsNoInfoLabel.Visible = true;
				DownloadsNoInfoLabel.AutoSize = true;
				DownloadsNoInfoLabel.Text = LanguageResx.ClientLanguage.NoCurrentDownloading;
				DownloadsNoInfoLabel.MinimumSize = new Size(DownloadsNoInfoPanel.Size.Width, 0);

				DownloadsNoInfoLabel.Location = new Point(
					DownloadsPanel.Width / 2 - DownloadsNoInfoLabel.Size.Width / 2,
					DownloadsNoInfoPanel.Location.Y + DownloadsNoInfoPanel.Size.Height + 20);
			}
		}

		/// <summary>
		/// Reload the Queue panel when you change the download priority of any game
		/// </summary>
		private void ReloadQueue()
        {
			QueueLayout.Controls.Clear();

			for (int currentDownloadGameInfo = 0; currentDownloadGameInfo < queueGames.Count; currentDownloadGameInfo++)
			{
				Panel queuePanel = new QueueGameInformationSmall(queueGames[currentDownloadGameInfo].GameImage,
					queueGames[currentDownloadGameInfo].GameName);

				Label queueStateLabel = MultipleResources.CreateGenericLabel("QueueState", true, 293, 14, 59, 40, 8,
					ContentAlignment.MiddleLeft);
				queueStateLabel.Text = LanguageResx.ClientLanguage.currentPriority + "  " + (currentDownloadGameInfo + 1);
				queuePanel.Controls.Add(queueStateLabel);

				Panel cancelQueueDownload = queuePanel.Controls.Find("QueueCancelButton", false)[0] as Panel;
				cancelQueueDownload.Name = "QueueCancelButton_" + currentDownloadGameInfo;

				Panel rankUpQueueDownload = queuePanel.Controls.Find("QueueRankUp", false)[0] as Panel;
				rankUpQueueDownload.Name = "QueueRankUp_" + currentDownloadGameInfo;

				Panel rankDownQueueDownload = queuePanel.Controls.Find("QueueRankDown", false)[0] as Panel;
				rankDownQueueDownload.Name = "QueueRankDown_" + currentDownloadGameInfo;

				cancelQueueDownload.Click += new EventHandler(CancelQueue_Click);
				rankUpQueueDownload.Click += new EventHandler(RankUpQueue_Click);
				rankDownQueueDownload.Click += new EventHandler(RankDownQueue_Click);

				QueueLayout.Controls.Add(queuePanel);
			}
		}
		#endregion Downloads / Installations

		#region Uninstallations
		/// <summary>
		/// Async task, Uninstall the desired game and show the progress in a panel in the side menu
		/// </summary>
		private async void UninstallGame()
		{
			if (Directory.Exists(downloadPath))
			{
				if (gameInfoOpen)
				{
					RefreshGameInfo_Click(LanguageResx.ClientLanguage.Uninstalling_Game);
				}
				ContextMenuStrip.Hide();

				if (DownloadInformation.Visible)
				{
					Uninstall_Information.Location = new Point(0, 211);
					Store.Location = new Point(0, 49);
					GameLibrary.Location = new Point(0, 130);
				}
				else
				{
					Uninstall_Information.Location = new Point(0, 280);
					Store.Location = new Point(0, 118);
					GameLibrary.Location = new Point(0, 199);
				}

				Uninstall_InformationGameImage.BackgroundImage = currentGameInfo.GameImage;
				Uninstall_InformationGameName.Text = currentGameInfo.GameName;

				UninstallProgress.Style = ProgressBarStyle.Marquee;
				UninstallProgress.Value = 40;
				Uninstall_Information.Visible = true;
				CloseUninstall_Information.Visible = false;

				UninstallState.ForeColor = Color.FromArgb(230, 230, 230);
				UninstallState.Text = LanguageResx.ClientLanguage.Uninstalling_Game;

				if (Directory.Exists(Path.Combine(downloadPath, currentGameInfo.GameName)))
				{
					Directory.Delete(Path.Combine(downloadPath, currentGameInfo.GameName), true);
				}

				UninstallState.Text = LanguageResx.ClientLanguage.Uninstalled_Game;
				UninstallProgress.Style = ProgressBarStyle.Continuous;
				UninstallProgress.Value = 100;

				if (gameInfoOpen)
				{
					CloseGameInfo_Click(null, EventArgs.Empty);
				}
				ContextMenuStrip.Hide();
				CloseUninstall_Information.Visible = true;
			}
		}
		#endregion Uninstallations

		#region File manager
		/// <summary>
		/// Create the folder 'Outlands Adventure Client' where the games will be installed
		/// </summary>
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

		/// <summary>
		/// Check if the game is already installed
		/// </summary>
		/// <returns>Boolean, true if installed already</returns>
		private bool CheckGameFiles()
		{
			if (Directory.Exists(Path.Combine(downloadPath, currentGameInfo.GameName)))
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Delete the game zip if the download went wrong or when needed
		/// </summary>
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
		#endregion File manager

		#region Others
		/// <summary>
		/// Opens the screen where you can see the current download and every queued game or close DownloadInformation side panel when
		/// the download is completed
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">MouseEvents that occur to the object</param>
		private void CloseShowDownloadInformation_MouseClick(object sender, MouseEventArgs e)
		{
			LanguageManager languageManager = new LanguageManager();
			languageManager.ChangeCurrentLanguage("es-ES");

			if (e.Button == MouseButtons.Left)
			{
				if (downloadState.Equals(LanguageResx.ClientLanguage.game_Downloading_Uppercase) || downloadState.Equals(LanguageResx.ClientLanguage.game_Install))
				{
					CheckDownload_UninstallInformationLanguage(); // Devuelve la aplicación al idioma elegido por el usuario
					ChangeScreen(ref downloadsInfoOpen, false);
					LoadDownloadsPanel();
				}
				else
                {
					CheckDownload_UninstallInformationLanguage(); // Devuelve la aplicación al idioma elegido por el usuario
					DownloadInformation.Visible = false;

					if (Uninstall_Information.Visible)
					{
						Uninstall_Information.Location = new Point(0, 280);
						Store.Location = new Point(0, 118);
						GameLibrary.Location = new Point(0, 199);
					}
				}
			}
			CheckDownload_UninstallInformationLanguage();
		}

		/// <summary>
		/// Close the uninstall side panel
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">MouseEvents that occur to the object</param>
		private void CloseUninstall_Information_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Uninstall_Information.Visible = false;

				Store.Location = new Point(0, 118);
				GameLibrary.Location = new Point(0, 199);
			}
		}

		/// <summary>
		/// Color change animation when the mouse enter the DownloadInformation panel or Uninstall_Information panel
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void Download_UninstallInformation_MouseEnter(object sender, EventArgs e)
		{
			if (sender == DownloadInformation)
			{
				if (canTriggerSelections)
				{
					ChangeBackgroundColor(DownloadInformation, objectHighlighted);
					canTriggerSelections = false;
				}
			}
			else if (sender == Uninstall_Information)
			{
				if (canTriggerSelections)
				{
					ChangeBackgroundColor(Uninstall_Information, objectHighlighted);
					canTriggerSelections = false;
				}
			}
		}

		/// <summary>
		/// Color change animation when the mouse exit the DownloadInformation panel or Uninstall_Information panel
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void Download_UninstallInformation_MouseLeave(object sender, EventArgs e)
		{
			if (sender == DownloadInformation)
			{
				if (sender.GetType() == typeof(Panel))
				{
					if ((Panel)sender == DownloadInformation)
					{
						if (!MouseIsOverControl(SideDownloadInformationGameName) &&
							!MouseIsOverControl(SideDownloadInformationGameImage) &&
							!MouseIsOverControl(SideDownloadProgressbar) &&
							!MouseIsOverControl(SideDownloadState) &&
							!MouseIsOverControl(CloseShowDownloadInformation))
						{
							ChangeBackgroundColor(DownloadInformation, objectUnmarked);
							canTriggerSelections = true;
						}
					}
				}
			}
			else if (sender == Uninstall_Information)
			{
				if (sender.GetType() == typeof(Panel))
				{
					if ((Panel)sender == Uninstall_Information)
					{
						if (!MouseIsOverControl(Uninstall_InformationGameName) &&
							!MouseIsOverControl(Uninstall_InformationGameImage) &&
							!MouseIsOverControl(UninstallProgress) && 
							!MouseIsOverControl(UninstallState) &&
							!MouseIsOverControl(CloseUninstall_Information))
						{
							ChangeBackgroundColor(Uninstall_Information, objectUnmarked);
							canTriggerSelections = true;
						}
					}
				}
			}
		}

		/// <summary>
		/// Show the tooltip that appers when mouse enter CloseShowDownloadInformation panel, el texto cambia si el juego se está 
		/// descargando / instalando o si la descarga ha terminado
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void DownloadShowInformation_MouseEnter(object sender, EventArgs e)
		{
			LanguageManager languageManager = new LanguageManager();
			languageManager.ChangeCurrentLanguage("es-ES");

			if (downloadState.Equals(LanguageResx.ClientLanguage.game_Downloading_Uppercase) || downloadState.Equals(LanguageResx.ClientLanguage.game_Install))
			{
				CheckDownload_UninstallInformationLanguage(); // Devuelve la aplicación al idioma elegido por el usuario
				MultipleResources.ShowToolTip(CloseShowDownloadInformation, LanguageResx.ClientLanguage.Open_DownloadsInformation);
			}
			else
            {
				CheckDownload_UninstallInformationLanguage(); // Devuelve la aplicación al idioma elegido por el usuario
				MultipleResources.ShowToolTip(CloseShowDownloadInformation, LanguageResx.ClientLanguage.button_Close_Lowercase);
            }
		}

		/// <summary>
		/// Hide the tooltip that appers when mouse exit CloseShowDownloadInformation panel
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void DownloadShowInformation_MouseLeave(object sender, EventArgs e)
		{
			MultipleResources.HideToolTip(CloseShowDownloadInformation);
		}

		/// <summary>
		/// Show the tooltip that appers when mouse enter CloseUninstall_Information panel
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void UninstallShowInformation_MouseEnter(object sender, EventArgs e)
		{
			MultipleResources.ShowToolTip(CloseUninstall_Information, LanguageResx.ClientLanguage.button_Close_Lowercase);
		}

		/// <summary>
		/// Hide the tooltip that appers when mouse exit CloseUninstall_Information panel
		/// </summary>
		/// <param name="sender">Object that receive the events</param>
		/// <param name="e">Events that occur to the object</param>
		private void UninstallShowInformation_MouseLeave(object sender, EventArgs e)
		{
			MultipleResources.HideToolTip(CloseUninstall_Information);
		}
		#endregion Others

		#endregion Downloads - Instalations - File manager
	}
}