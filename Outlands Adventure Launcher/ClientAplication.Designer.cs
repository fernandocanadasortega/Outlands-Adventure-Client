﻿namespace Outlands_Adventure_Launcher
{
    partial class ClientAplication
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientAplication));
            this.SideMenu = new System.Windows.Forms.Panel();
            this.Store = new System.Windows.Forms.Panel();
            this.StoreLabel = new System.Windows.Forms.Label();
            this.GameLibrary = new System.Windows.Forms.Panel();
            this.GameLibraryLabel = new System.Windows.Forms.Label();
            this.DownloadInformation = new System.Windows.Forms.Panel();
            this.DownloadInformationGameName = new System.Windows.Forms.Label();
            this.DownloadInformationGameImage = new System.Windows.Forms.Panel();
            this.DownloadProgress = new System.Windows.Forms.ProgressBar();
            this.DownloadState = new System.Windows.Forms.Label();
            this.CloseDownloadInformation = new System.Windows.Forms.Panel();
            this.Uninstall_Information = new System.Windows.Forms.Panel();
            this.Uninstall_InformationGameName = new System.Windows.Forms.Label();
            this.Uninstall_InformationGameImage = new System.Windows.Forms.Panel();
            this.UninstallProgress = new System.Windows.Forms.ProgressBar();
            this.UninstallState = new System.Windows.Forms.Label();
            this.CloseUninstall_Information = new System.Windows.Forms.Panel();
            this.UserInformation = new System.Windows.Forms.Panel();
            this.UserName = new System.Windows.Forms.Label();
            this.UserConfigurationArrow = new System.Windows.Forms.Panel();
            this.UserPhoto = new System.Windows.Forms.Panel();
            this.MainMenu = new System.Windows.Forms.Panel();
            this.GameInfoMenu = new System.Windows.Forms.Panel();
            this.GameInfoGradient = new System.Windows.Forms.Panel();
            this.CloseGameInfoBackground = new System.Windows.Forms.Panel();
            this.CloseGameInfo = new System.Windows.Forms.Panel();
            this.GameSettingsBackground = new System.Windows.Forms.Panel();
            this.GameSettings = new System.Windows.Forms.Panel();
            this.MoneyPanel = new System.Windows.Forms.Panel();
            this.CurrentCurrencyHeader = new System.Windows.Forms.Label();
            this.CurrentCurrency = new System.Windows.Forms.Label();
            this.GamePriceHeader = new System.Windows.Forms.Label();
            this.GamePrice = new System.Windows.Forms.Label();
            this.Play_BuyGame = new System.Windows.Forms.Button();
            this.TileSize_GameFilter = new System.Windows.Forms.Panel();
            this.SmallTiles = new System.Windows.Forms.Panel();
            this.MediumTiles = new System.Windows.Forms.Panel();
            this.LargeTiles = new System.Windows.Forms.Panel();
            this.FilterGame = new System.Windows.Forms.TextBox();
            this.StoreMenu = new System.Windows.Forms.Panel();
            this.StoreEmpty = new System.Windows.Forms.Panel();
            this.StoreEmptyImage = new System.Windows.Forms.Panel();
            this.StoreEmptyLabel = new System.Windows.Forms.Label();
            this.StoreAvailableGames = new System.Windows.Forms.ListView();
            this.GameLibraryMenu = new System.Windows.Forms.Panel();
            this.LibraryEmpty = new System.Windows.Forms.Panel();
            this.LibraryEmptyImage = new System.Windows.Forms.Panel();
            this.LibraryEmptyLabel = new System.Windows.Forms.Label();
            this.GameLibraryAvailableGames = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.GameImages = new System.Windows.Forms.ImageList(this.components);
            this.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ImagePanel = new System.Windows.Forms.Panel();
            this.ImageGradient = new System.Windows.Forms.Panel();
            this.ConfigurationPanel = new System.Windows.Forms.Panel();
            this.ConfigurationHeader = new System.Windows.Forms.Label();
            this.ClientLanguageHeader = new System.Windows.Forms.Label();
            this.LanguageSelected = new System.Windows.Forms.ComboBox();
            this.DefaultScreenHeader = new System.Windows.Forms.Label();
            this.DefaultScreen = new System.Windows.Forms.ComboBox();
            this.DeleteAccount = new System.Windows.Forms.Label();
            this.ConfigurationExitButton = new System.Windows.Forms.Label();
            this.EventsPanel = new System.Windows.Forms.Panel();
            this.EventText = new System.Windows.Forms.Label();
            this.EventCodeError = new System.Windows.Forms.Label();
            this.EventCode = new System.Windows.Forms.TextBox();
            this.EventSendButton = new System.Windows.Forms.Label();
            this.EventExitButton = new System.Windows.Forms.Label();
            this.SideMenu.SuspendLayout();
            this.Store.SuspendLayout();
            this.GameLibrary.SuspendLayout();
            this.DownloadInformation.SuspendLayout();
            this.Uninstall_Information.SuspendLayout();
            this.UserInformation.SuspendLayout();
            this.MainMenu.SuspendLayout();
            this.GameInfoMenu.SuspendLayout();
            this.GameInfoGradient.SuspendLayout();
            this.CloseGameInfoBackground.SuspendLayout();
            this.GameSettingsBackground.SuspendLayout();
            this.MoneyPanel.SuspendLayout();
            this.TileSize_GameFilter.SuspendLayout();
            this.StoreMenu.SuspendLayout();
            this.StoreEmpty.SuspendLayout();
            this.GameLibraryMenu.SuspendLayout();
            this.LibraryEmpty.SuspendLayout();
            this.ImagePanel.SuspendLayout();
            this.ImageGradient.SuspendLayout();
            this.ConfigurationPanel.SuspendLayout();
            this.EventsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // SideMenu
            // 
            this.SideMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(40)))));
            this.SideMenu.Controls.Add(this.Store);
            this.SideMenu.Controls.Add(this.GameLibrary);
            this.SideMenu.Controls.Add(this.DownloadInformation);
            this.SideMenu.Controls.Add(this.Uninstall_Information);
            this.SideMenu.Controls.Add(this.UserInformation);
            this.SideMenu.Location = new System.Drawing.Point(0, 0);
            this.SideMenu.Name = "SideMenu";
            this.SideMenu.Size = new System.Drawing.Size(375, 708);
            this.SideMenu.TabIndex = 12;
            this.SideMenu.Click += new System.EventHandler(this.LostFocus_Click);
            // 
            // Store
            // 
            this.Store.Controls.Add(this.StoreLabel);
            this.Store.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Store.Location = new System.Drawing.Point(0, 185);
            this.Store.Name = "Store";
            this.Store.Size = new System.Drawing.Size(375, 80);
            this.Store.TabIndex = 3;
            this.Store.Paint += new System.Windows.Forms.PaintEventHandler(this.Store_Paint);
            this.Store.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Store_MouseDown);
            this.Store.MouseEnter += new System.EventHandler(this.Store_MouseEnter);
            this.Store.MouseLeave += new System.EventHandler(this.Store_MouseLeave);
            // 
            // StoreLabel
            // 
            this.StoreLabel.AutoSize = true;
            this.StoreLabel.BackColor = System.Drawing.Color.Transparent;
            this.StoreLabel.Font = new System.Drawing.Font("Oxygen", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StoreLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.StoreLabel.Location = new System.Drawing.Point(40, 22);
            this.StoreLabel.Name = "StoreLabel";
            this.StoreLabel.Size = new System.Drawing.Size(104, 36);
            this.StoreLabel.TabIndex = 0;
            this.StoreLabel.Text = "Tienda";
            this.StoreLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Store_MouseDown);
            this.StoreLabel.MouseEnter += new System.EventHandler(this.Store_MouseEnter);
            this.StoreLabel.MouseLeave += new System.EventHandler(this.Store_MouseLeave);
            // 
            // GameLibrary
            // 
            this.GameLibrary.Controls.Add(this.GameLibraryLabel);
            this.GameLibrary.Cursor = System.Windows.Forms.Cursors.Hand;
            this.GameLibrary.Location = new System.Drawing.Point(0, 330);
            this.GameLibrary.Name = "GameLibrary";
            this.GameLibrary.Size = new System.Drawing.Size(375, 80);
            this.GameLibrary.TabIndex = 4;
            this.GameLibrary.Paint += new System.Windows.Forms.PaintEventHandler(this.GameLibrary_Paint);
            this.GameLibrary.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GameLibrary_MouseDown);
            this.GameLibrary.MouseEnter += new System.EventHandler(this.GameLibrary_MouseEnter);
            this.GameLibrary.MouseLeave += new System.EventHandler(this.GameLibrary_MouseLeave);
            // 
            // GameLibraryLabel
            // 
            this.GameLibraryLabel.AutoSize = true;
            this.GameLibraryLabel.BackColor = System.Drawing.Color.Transparent;
            this.GameLibraryLabel.Font = new System.Drawing.Font("Oxygen", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GameLibraryLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.GameLibraryLabel.Location = new System.Drawing.Point(40, 22);
            this.GameLibraryLabel.Name = "GameLibraryLabel";
            this.GameLibraryLabel.Size = new System.Drawing.Size(248, 36);
            this.GameLibraryLabel.TabIndex = 0;
            this.GameLibraryLabel.Text = "Librería de juegos";
            this.GameLibraryLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GameLibrary_MouseDown);
            this.GameLibraryLabel.MouseEnter += new System.EventHandler(this.GameLibrary_MouseEnter);
            this.GameLibraryLabel.MouseLeave += new System.EventHandler(this.GameLibrary_MouseLeave);
            // 
            // DownloadInformation
            // 
            this.DownloadInformation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DownloadInformation.Controls.Add(this.DownloadInformationGameName);
            this.DownloadInformation.Controls.Add(this.DownloadInformationGameImage);
            this.DownloadInformation.Controls.Add(this.DownloadProgress);
            this.DownloadInformation.Controls.Add(this.DownloadState);
            this.DownloadInformation.Controls.Add(this.CloseDownloadInformation);
            this.DownloadInformation.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.DownloadInformation.Location = new System.Drawing.Point(0, 510);
            this.DownloadInformation.Name = "DownloadInformation";
            this.DownloadInformation.Size = new System.Drawing.Size(375, 98);
            this.DownloadInformation.TabIndex = 3;
            this.DownloadInformation.Visible = false;
            // 
            // DownloadInformationGameName
            // 
            this.DownloadInformationGameName.BackColor = System.Drawing.Color.Transparent;
            this.DownloadInformationGameName.Cursor = System.Windows.Forms.Cursors.Default;
            this.DownloadInformationGameName.Font = new System.Drawing.Font("Oxygen", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DownloadInformationGameName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.DownloadInformationGameName.Location = new System.Drawing.Point(101, 12);
            this.DownloadInformationGameName.Name = "DownloadInformationGameName";
            this.DownloadInformationGameName.Size = new System.Drawing.Size(243, 23);
            this.DownloadInformationGameName.TabIndex = 0;
            this.DownloadInformationGameName.Text = "qwertyuiopasdfghjklñzxcvbnmqwe\r\n\r\n";
            this.DownloadInformationGameName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DownloadInformationGameImage
            // 
            this.DownloadInformationGameImage.BackColor = System.Drawing.Color.Transparent;
            this.DownloadInformationGameImage.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.Pantalla_launcher;
            this.DownloadInformationGameImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.DownloadInformationGameImage.Cursor = System.Windows.Forms.Cursors.Default;
            this.DownloadInformationGameImage.Location = new System.Drawing.Point(15, 9);
            this.DownloadInformationGameImage.Name = "DownloadInformationGameImage";
            this.DownloadInformationGameImage.Size = new System.Drawing.Size(80, 80);
            this.DownloadInformationGameImage.TabIndex = 0;
            // 
            // DownloadProgress
            // 
            this.DownloadProgress.Cursor = System.Windows.Forms.Cursors.Default;
            this.DownloadProgress.Location = new System.Drawing.Point(107, 45);
            this.DownloadProgress.Name = "DownloadProgress";
            this.DownloadProgress.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.DownloadProgress.Size = new System.Drawing.Size(230, 10);
            this.DownloadProgress.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.DownloadProgress.TabIndex = 2;
            this.DownloadProgress.Value = 40;
            // 
            // DownloadState
            // 
            this.DownloadState.BackColor = System.Drawing.Color.Transparent;
            this.DownloadState.Cursor = System.Windows.Forms.Cursors.Default;
            this.DownloadState.Font = new System.Drawing.Font("Oxygen", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DownloadState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.DownloadState.Location = new System.Drawing.Point(101, 65);
            this.DownloadState.Name = "DownloadState";
            this.DownloadState.Size = new System.Drawing.Size(243, 23);
            this.DownloadState.TabIndex = 1;
            this.DownloadState.Text = "qwertyuiopasdfghjklñzxcvbnmqwe\r\n\r\n";
            this.DownloadState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CloseDownloadInformation
            // 
            this.CloseDownloadInformation.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.close;
            this.CloseDownloadInformation.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CloseDownloadInformation.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CloseDownloadInformation.Location = new System.Drawing.Point(350, 5);
            this.CloseDownloadInformation.Name = "CloseDownloadInformation";
            this.CloseDownloadInformation.Size = new System.Drawing.Size(20, 20);
            this.CloseDownloadInformation.TabIndex = 3;
            this.CloseDownloadInformation.Visible = false;
            this.CloseDownloadInformation.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CloseDownloadInformation_MouseClick);
            // 
            // Uninstall_Information
            // 
            this.Uninstall_Information.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Uninstall_Information.Controls.Add(this.Uninstall_InformationGameName);
            this.Uninstall_Information.Controls.Add(this.Uninstall_InformationGameImage);
            this.Uninstall_Information.Controls.Add(this.UninstallProgress);
            this.Uninstall_Information.Controls.Add(this.UninstallState);
            this.Uninstall_Information.Controls.Add(this.CloseUninstall_Information);
            this.Uninstall_Information.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Uninstall_Information.Location = new System.Drawing.Point(0, 408);
            this.Uninstall_Information.Name = "Uninstall_Information";
            this.Uninstall_Information.Size = new System.Drawing.Size(375, 98);
            this.Uninstall_Information.TabIndex = 5;
            this.Uninstall_Information.Visible = false;
            // 
            // Uninstall_InformationGameName
            // 
            this.Uninstall_InformationGameName.BackColor = System.Drawing.Color.Transparent;
            this.Uninstall_InformationGameName.Cursor = System.Windows.Forms.Cursors.Default;
            this.Uninstall_InformationGameName.Font = new System.Drawing.Font("Oxygen", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Uninstall_InformationGameName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.Uninstall_InformationGameName.Location = new System.Drawing.Point(101, 12);
            this.Uninstall_InformationGameName.Name = "Uninstall_InformationGameName";
            this.Uninstall_InformationGameName.Size = new System.Drawing.Size(243, 23);
            this.Uninstall_InformationGameName.TabIndex = 0;
            this.Uninstall_InformationGameName.Text = "qwertyuiopasdfghjklñzxcvbnmqwe\r\n\r\n";
            this.Uninstall_InformationGameName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Uninstall_InformationGameImage
            // 
            this.Uninstall_InformationGameImage.BackColor = System.Drawing.Color.Transparent;
            this.Uninstall_InformationGameImage.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.Pantalla_launcher;
            this.Uninstall_InformationGameImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Uninstall_InformationGameImage.Cursor = System.Windows.Forms.Cursors.Default;
            this.Uninstall_InformationGameImage.Location = new System.Drawing.Point(15, 9);
            this.Uninstall_InformationGameImage.Name = "Uninstall_InformationGameImage";
            this.Uninstall_InformationGameImage.Size = new System.Drawing.Size(80, 80);
            this.Uninstall_InformationGameImage.TabIndex = 0;
            // 
            // UninstallProgress
            // 
            this.UninstallProgress.Cursor = System.Windows.Forms.Cursors.Default;
            this.UninstallProgress.Location = new System.Drawing.Point(107, 45);
            this.UninstallProgress.Name = "UninstallProgress";
            this.UninstallProgress.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UninstallProgress.Size = new System.Drawing.Size(230, 10);
            this.UninstallProgress.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.UninstallProgress.TabIndex = 2;
            this.UninstallProgress.Value = 40;
            // 
            // UninstallState
            // 
            this.UninstallState.BackColor = System.Drawing.Color.Transparent;
            this.UninstallState.Cursor = System.Windows.Forms.Cursors.Default;
            this.UninstallState.Font = new System.Drawing.Font("Oxygen", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UninstallState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.UninstallState.Location = new System.Drawing.Point(101, 65);
            this.UninstallState.Name = "UninstallState";
            this.UninstallState.Size = new System.Drawing.Size(243, 23);
            this.UninstallState.TabIndex = 1;
            this.UninstallState.Text = "qwertyuiopasdfghjklñzxcvbnmqwe\r\n\r\n";
            this.UninstallState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CloseUninstall_Information
            // 
            this.CloseUninstall_Information.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.close;
            this.CloseUninstall_Information.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CloseUninstall_Information.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CloseUninstall_Information.Location = new System.Drawing.Point(350, 5);
            this.CloseUninstall_Information.Name = "CloseUninstall_Information";
            this.CloseUninstall_Information.Size = new System.Drawing.Size(20, 20);
            this.CloseUninstall_Information.TabIndex = 3;
            this.CloseUninstall_Information.Visible = false;
            this.CloseUninstall_Information.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CloseUninstall_Information_MouseClick);
            // 
            // UserInformation
            // 
            this.UserInformation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.UserInformation.Controls.Add(this.UserName);
            this.UserInformation.Controls.Add(this.UserConfigurationArrow);
            this.UserInformation.Controls.Add(this.UserPhoto);
            this.UserInformation.Cursor = System.Windows.Forms.Cursors.Hand;
            this.UserInformation.Location = new System.Drawing.Point(0, 610);
            this.UserInformation.Name = "UserInformation";
            this.UserInformation.Size = new System.Drawing.Size(375, 98);
            this.UserInformation.TabIndex = 2;
            this.UserInformation.MouseEnter += new System.EventHandler(this.UserInformation_MouseEnter);
            this.UserInformation.MouseLeave += new System.EventHandler(this.UserInformation_MouseLeave);
            // 
            // UserName
            // 
            this.UserName.BackColor = System.Drawing.Color.Transparent;
            this.UserName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.UserName.Enabled = false;
            this.UserName.Font = new System.Drawing.Font("Oxygen", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.UserName.Location = new System.Drawing.Point(113, 35);
            this.UserName.Name = "UserName";
            this.UserName.Size = new System.Drawing.Size(210, 23);
            this.UserName.TabIndex = 0;
            this.UserName.Text = "qwertyuiopasdfghjklñzxcvbnmqwe\r\n\r\n";
            // 
            // UserConfigurationArrow
            // 
            this.UserConfigurationArrow.BackColor = System.Drawing.Color.Transparent;
            this.UserConfigurationArrow.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("UserConfigurationArrow.BackgroundImage")));
            this.UserConfigurationArrow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.UserConfigurationArrow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.UserConfigurationArrow.Enabled = false;
            this.UserConfigurationArrow.Location = new System.Drawing.Point(346, 35);
            this.UserConfigurationArrow.Name = "UserConfigurationArrow";
            this.UserConfigurationArrow.Size = new System.Drawing.Size(23, 23);
            this.UserConfigurationArrow.TabIndex = 1;
            // 
            // UserPhoto
            // 
            this.UserPhoto.BackColor = System.Drawing.Color.Transparent;
            this.UserPhoto.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.Pantalla_launcher;
            this.UserPhoto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.UserPhoto.Cursor = System.Windows.Forms.Cursors.Hand;
            this.UserPhoto.Enabled = false;
            this.UserPhoto.Location = new System.Drawing.Point(15, 9);
            this.UserPhoto.Name = "UserPhoto";
            this.UserPhoto.Size = new System.Drawing.Size(80, 80);
            this.UserPhoto.TabIndex = 0;
            // 
            // MainMenu
            // 
            this.MainMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.MainMenu.Controls.Add(this.GameInfoMenu);
            this.MainMenu.Controls.Add(this.TileSize_GameFilter);
            this.MainMenu.Controls.Add(this.StoreMenu);
            this.MainMenu.Controls.Add(this.GameLibraryMenu);
            this.MainMenu.Location = new System.Drawing.Point(375, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(990, 708);
            this.MainMenu.TabIndex = 15;
            // 
            // GameInfoMenu
            // 
            this.GameInfoMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.GameInfoMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.GameInfoMenu.Controls.Add(this.GameInfoGradient);
            this.GameInfoMenu.Location = new System.Drawing.Point(0, 0);
            this.GameInfoMenu.Name = "GameInfoMenu";
            this.GameInfoMenu.Size = new System.Drawing.Size(990, 708);
            this.GameInfoMenu.TabIndex = 18;
            this.GameInfoMenu.Visible = false;
            // 
            // GameInfoGradient
            // 
            this.GameInfoGradient.Controls.Add(this.CloseGameInfoBackground);
            this.GameInfoGradient.Controls.Add(this.GameSettingsBackground);
            this.GameInfoGradient.Controls.Add(this.MoneyPanel);
            this.GameInfoGradient.Controls.Add(this.Play_BuyGame);
            this.GameInfoGradient.Location = new System.Drawing.Point(0, 0);
            this.GameInfoGradient.Name = "GameInfoGradient";
            this.GameInfoGradient.Size = new System.Drawing.Size(990, 708);
            this.GameInfoGradient.TabIndex = 4;
            // 
            // CloseGameInfoBackground
            // 
            this.CloseGameInfoBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(40)))));
            this.CloseGameInfoBackground.Controls.Add(this.CloseGameInfo);
            this.CloseGameInfoBackground.Location = new System.Drawing.Point(933, 0);
            this.CloseGameInfoBackground.Name = "CloseGameInfoBackground";
            this.CloseGameInfoBackground.Size = new System.Drawing.Size(42, 52);
            this.CloseGameInfoBackground.TabIndex = 1;
            // 
            // CloseGameInfo
            // 
            this.CloseGameInfo.BackColor = System.Drawing.Color.Transparent;
            this.CloseGameInfo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CloseGameInfo.BackgroundImage")));
            this.CloseGameInfo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CloseGameInfo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CloseGameInfo.Location = new System.Drawing.Point(7, 15);
            this.CloseGameInfo.Name = "CloseGameInfo";
            this.CloseGameInfo.Size = new System.Drawing.Size(30, 30);
            this.CloseGameInfo.TabIndex = 0;
            this.CloseGameInfo.Click += new System.EventHandler(this.CloseGameInfo_Click);
            // 
            // GameSettingsBackground
            // 
            this.GameSettingsBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(40)))));
            this.GameSettingsBackground.Controls.Add(this.GameSettings);
            this.GameSettingsBackground.Location = new System.Drawing.Point(15, 0);
            this.GameSettingsBackground.Name = "GameSettingsBackground";
            this.GameSettingsBackground.Size = new System.Drawing.Size(42, 52);
            this.GameSettingsBackground.TabIndex = 2;
            // 
            // GameSettings
            // 
            this.GameSettings.BackColor = System.Drawing.Color.Transparent;
            this.GameSettings.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.game_settings;
            this.GameSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.GameSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.GameSettings.Location = new System.Drawing.Point(7, 15);
            this.GameSettings.Name = "GameSettings";
            this.GameSettings.Size = new System.Drawing.Size(30, 30);
            this.GameSettings.TabIndex = 0;
            this.GameSettings.Click += new System.EventHandler(this.GameSettings_Click);
            // 
            // MoneyPanel
            // 
            this.MoneyPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(40)))));
            this.MoneyPanel.Controls.Add(this.CurrentCurrencyHeader);
            this.MoneyPanel.Controls.Add(this.CurrentCurrency);
            this.MoneyPanel.Controls.Add(this.GamePriceHeader);
            this.MoneyPanel.Controls.Add(this.GamePrice);
            this.MoneyPanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.MoneyPanel.Location = new System.Drawing.Point(290, 390);
            this.MoneyPanel.Name = "MoneyPanel";
            this.MoneyPanel.Size = new System.Drawing.Size(396, 118);
            this.MoneyPanel.TabIndex = 4;
            this.MoneyPanel.Visible = false;
            // 
            // CurrentCurrencyHeader
            // 
            this.CurrentCurrencyHeader.AutoSize = true;
            this.CurrentCurrencyHeader.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurrentCurrencyHeader.Location = new System.Drawing.Point(25, 22);
            this.CurrentCurrencyHeader.Name = "CurrentCurrencyHeader";
            this.CurrentCurrencyHeader.Size = new System.Drawing.Size(131, 26);
            this.CurrentCurrencyHeader.TabIndex = 0;
            this.CurrentCurrencyHeader.Text = "Saldo Actual";
            // 
            // CurrentCurrency
            // 
            this.CurrentCurrency.AutoSize = true;
            this.CurrentCurrency.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurrentCurrency.Location = new System.Drawing.Point(300, 22);
            this.CurrentCurrency.Name = "CurrentCurrency";
            this.CurrentCurrency.Size = new System.Drawing.Size(62, 26);
            this.CurrentCurrency.TabIndex = 2;
            this.CurrentCurrency.Text = "100 €";
            // 
            // GamePriceHeader
            // 
            this.GamePriceHeader.AutoSize = true;
            this.GamePriceHeader.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GamePriceHeader.Location = new System.Drawing.Point(25, 70);
            this.GamePriceHeader.Name = "GamePriceHeader";
            this.GamePriceHeader.Size = new System.Drawing.Size(161, 26);
            this.GamePriceHeader.TabIndex = 1;
            this.GamePriceHeader.Text = "Precio del juego";
            // 
            // GamePrice
            // 
            this.GamePrice.AutoSize = true;
            this.GamePrice.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GamePrice.Location = new System.Drawing.Point(300, 70);
            this.GamePrice.Name = "GamePrice";
            this.GamePrice.Size = new System.Drawing.Size(62, 26);
            this.GamePrice.TabIndex = 3;
            this.GamePrice.Text = "100 €";
            // 
            // Play_BuyGame
            // 
            this.Play_BuyGame.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(40)))));
            this.Play_BuyGame.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Play_BuyGame.FlatAppearance.BorderSize = 0;
            this.Play_BuyGame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Play_BuyGame.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Play_BuyGame.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Play_BuyGame.Location = new System.Drawing.Point(330, 550);
            this.Play_BuyGame.Name = "Play_BuyGame";
            this.Play_BuyGame.Size = new System.Drawing.Size(316, 55);
            this.Play_BuyGame.TabIndex = 3;
            this.Play_BuyGame.Text = "Jugar";
            this.Play_BuyGame.UseVisualStyleBackColor = false;
            this.Play_BuyGame.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Play_BuyGame_MouseClick);
            // 
            // TileSize_GameFilter
            // 
            this.TileSize_GameFilter.Controls.Add(this.SmallTiles);
            this.TileSize_GameFilter.Controls.Add(this.MediumTiles);
            this.TileSize_GameFilter.Controls.Add(this.LargeTiles);
            this.TileSize_GameFilter.Controls.Add(this.FilterGame);
            this.TileSize_GameFilter.Location = new System.Drawing.Point(0, 0);
            this.TileSize_GameFilter.Name = "TileSize_GameFilter";
            this.TileSize_GameFilter.Size = new System.Drawing.Size(990, 85);
            this.TileSize_GameFilter.TabIndex = 18;
            this.TileSize_GameFilter.Click += new System.EventHandler(this.LostFocus_Click);
            this.TileSize_GameFilter.Paint += new System.Windows.Forms.PaintEventHandler(this.TileSize_Paint);
            // 
            // SmallTiles
            // 
            this.SmallTiles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SmallTiles.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SmallTiles.Location = new System.Drawing.Point(92, 15);
            this.SmallTiles.Name = "SmallTiles";
            this.SmallTiles.Size = new System.Drawing.Size(50, 50);
            this.SmallTiles.TabIndex = 2;
            this.SmallTiles.Click += new System.EventHandler(this.SmallTiles_Click);
            this.SmallTiles.Paint += new System.Windows.Forms.PaintEventHandler(this.SmallTiles_Paint);
            this.SmallTiles.MouseEnter += new System.EventHandler(this.SmallTiles_MouseEnter);
            this.SmallTiles.MouseLeave += new System.EventHandler(this.SmallTiles_MouseLeave);
            // 
            // MediumTiles
            // 
            this.MediumTiles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MediumTiles.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MediumTiles.Location = new System.Drawing.Point(142, 15);
            this.MediumTiles.Name = "MediumTiles";
            this.MediumTiles.Size = new System.Drawing.Size(50, 50);
            this.MediumTiles.TabIndex = 1;
            this.MediumTiles.Click += new System.EventHandler(this.MediumTiles_Click);
            this.MediumTiles.Paint += new System.Windows.Forms.PaintEventHandler(this.MediumTiles_Paint);
            this.MediumTiles.MouseEnter += new System.EventHandler(this.MediumTiles_MouseEnter);
            this.MediumTiles.MouseLeave += new System.EventHandler(this.MediumTiles_MouseLeave);
            // 
            // LargeTiles
            // 
            this.LargeTiles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LargeTiles.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LargeTiles.Location = new System.Drawing.Point(192, 15);
            this.LargeTiles.Name = "LargeTiles";
            this.LargeTiles.Size = new System.Drawing.Size(50, 50);
            this.LargeTiles.TabIndex = 0;
            this.LargeTiles.Click += new System.EventHandler(this.LargeTiles_Click);
            this.LargeTiles.Paint += new System.Windows.Forms.PaintEventHandler(this.LargeTiles_Paint);
            this.LargeTiles.MouseEnter += new System.EventHandler(this.LargeTiles_MouseEnter);
            this.LargeTiles.MouseLeave += new System.EventHandler(this.LargeTiles_MouseLeave);
            // 
            // FilterGame
            // 
            this.FilterGame.BackColor = System.Drawing.Color.White;
            this.FilterGame.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FilterGame.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.FilterGame.Location = new System.Drawing.Point(610, 23);
            this.FilterGame.Name = "FilterGame";
            this.FilterGame.Size = new System.Drawing.Size(288, 33);
            this.FilterGame.TabIndex = 3;
            this.FilterGame.TabStop = false;
            this.FilterGame.Text = "Filter Library";
            this.FilterGame.Click += new System.EventHandler(this.FilterGame_Click);
            this.FilterGame.TextChanged += new System.EventHandler(this.FilterGame_TextChanged);
            this.FilterGame.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FilterGame_KeyDown);
            // 
            // StoreMenu
            // 
            this.StoreMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.StoreMenu.Controls.Add(this.StoreEmpty);
            this.StoreMenu.Controls.Add(this.StoreAvailableGames);
            this.StoreMenu.Location = new System.Drawing.Point(0, 84);
            this.StoreMenu.Name = "StoreMenu";
            this.StoreMenu.Size = new System.Drawing.Size(990, 624);
            this.StoreMenu.TabIndex = 16;
            this.StoreMenu.Visible = false;
            this.StoreMenu.Click += new System.EventHandler(this.LostFocus_Click);
            // 
            // StoreEmpty
            // 
            this.StoreEmpty.BackColor = System.Drawing.Color.Transparent;
            this.StoreEmpty.Controls.Add(this.StoreEmptyImage);
            this.StoreEmpty.Controls.Add(this.StoreEmptyLabel);
            this.StoreEmpty.Location = new System.Drawing.Point(300, 120);
            this.StoreEmpty.Name = "StoreEmpty";
            this.StoreEmpty.Size = new System.Drawing.Size(390, 390);
            this.StoreEmpty.TabIndex = 1;
            this.StoreEmpty.Visible = false;
            // 
            // StoreEmptyImage
            // 
            this.StoreEmptyImage.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.empty_store;
            this.StoreEmptyImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.StoreEmptyImage.Location = new System.Drawing.Point(0, 0);
            this.StoreEmptyImage.Name = "StoreEmptyImage";
            this.StoreEmptyImage.Size = new System.Drawing.Size(390, 280);
            this.StoreEmptyImage.TabIndex = 0;
            // 
            // StoreEmptyLabel
            // 
            this.StoreEmptyLabel.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StoreEmptyLabel.ForeColor = System.Drawing.Color.Black;
            this.StoreEmptyLabel.Location = new System.Drawing.Point(0, 281);
            this.StoreEmptyLabel.Name = "StoreEmptyLabel";
            this.StoreEmptyLabel.Size = new System.Drawing.Size(390, 109);
            this.StoreEmptyLabel.TabIndex = 1;
            this.StoreEmptyLabel.Text = "No se han encontrado juegos nuevos";
            this.StoreEmptyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // StoreAvailableGames
            // 
            this.StoreAvailableGames.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.StoreAvailableGames.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.StoreAvailableGames.Cursor = System.Windows.Forms.Cursors.Hand;
            this.StoreAvailableGames.HideSelection = false;
            this.StoreAvailableGames.Location = new System.Drawing.Point(92, 10);
            this.StoreAvailableGames.MultiSelect = false;
            this.StoreAvailableGames.Name = "StoreAvailableGames";
            this.StoreAvailableGames.Size = new System.Drawing.Size(806, 595);
            this.StoreAvailableGames.TabIndex = 0;
            this.StoreAvailableGames.UseCompatibleStateImageBehavior = false;
            this.StoreAvailableGames.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GamesListView_MouseClick);
            // 
            // GameLibraryMenu
            // 
            this.GameLibraryMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.GameLibraryMenu.Controls.Add(this.LibraryEmpty);
            this.GameLibraryMenu.Controls.Add(this.GameLibraryAvailableGames);
            this.GameLibraryMenu.Location = new System.Drawing.Point(0, 84);
            this.GameLibraryMenu.Name = "GameLibraryMenu";
            this.GameLibraryMenu.Size = new System.Drawing.Size(990, 624);
            this.GameLibraryMenu.TabIndex = 17;
            this.GameLibraryMenu.Visible = false;
            this.GameLibraryMenu.Click += new System.EventHandler(this.LostFocus_Click);
            // 
            // LibraryEmpty
            // 
            this.LibraryEmpty.BackColor = System.Drawing.Color.Transparent;
            this.LibraryEmpty.Controls.Add(this.LibraryEmptyImage);
            this.LibraryEmpty.Controls.Add(this.LibraryEmptyLabel);
            this.LibraryEmpty.Location = new System.Drawing.Point(300, 120);
            this.LibraryEmpty.Name = "LibraryEmpty";
            this.LibraryEmpty.Size = new System.Drawing.Size(390, 390);
            this.LibraryEmpty.TabIndex = 2;
            this.LibraryEmpty.Visible = false;
            // 
            // LibraryEmptyImage
            // 
            this.LibraryEmptyImage.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.game_library_empty;
            this.LibraryEmptyImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.LibraryEmptyImage.Location = new System.Drawing.Point(0, 0);
            this.LibraryEmptyImage.Name = "LibraryEmptyImage";
            this.LibraryEmptyImage.Size = new System.Drawing.Size(390, 280);
            this.LibraryEmptyImage.TabIndex = 0;
            // 
            // LibraryEmptyLabel
            // 
            this.LibraryEmptyLabel.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LibraryEmptyLabel.ForeColor = System.Drawing.Color.Black;
            this.LibraryEmptyLabel.Location = new System.Drawing.Point(0, 281);
            this.LibraryEmptyLabel.Name = "LibraryEmptyLabel";
            this.LibraryEmptyLabel.Size = new System.Drawing.Size(390, 109);
            this.LibraryEmptyLabel.TabIndex = 1;
            this.LibraryEmptyLabel.Text = "No se han encontrado juegos en la librería";
            this.LibraryEmptyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GameLibraryAvailableGames
            // 
            this.GameLibraryAvailableGames.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.GameLibraryAvailableGames.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.GameLibraryAvailableGames.Cursor = System.Windows.Forms.Cursors.Hand;
            this.GameLibraryAvailableGames.HideSelection = false;
            this.GameLibraryAvailableGames.Location = new System.Drawing.Point(92, 10);
            this.GameLibraryAvailableGames.MultiSelect = false;
            this.GameLibraryAvailableGames.Name = "GameLibraryAvailableGames";
            this.GameLibraryAvailableGames.Size = new System.Drawing.Size(806, 595);
            this.GameLibraryAvailableGames.TabIndex = 1;
            this.GameLibraryAvailableGames.UseCompatibleStateImageBehavior = false;
            this.GameLibraryAvailableGames.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GamesListView_MouseClick);
            // 
            // GameImages
            // 
            this.GameImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.GameImages.ImageSize = new System.Drawing.Size(16, 16);
            this.GameImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // ContextMenuStrip
            // 
            this.ContextMenuStrip.BackColor = System.Drawing.Color.White;
            this.ContextMenuStrip.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ContextMenuStrip.Name = "contextMenuStrip";
            this.ContextMenuStrip.ShowImageMargin = false;
            this.ContextMenuStrip.Size = new System.Drawing.Size(36, 4);
            this.ContextMenuStrip.MouseLeave += new System.EventHandler(this.ContextMenuStrip_MouseLeave);
            // 
            // ImagePanel
            // 
            this.ImagePanel.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.Pantalla_launcher;
            this.ImagePanel.Controls.Add(this.ImageGradient);
            this.ImagePanel.Location = new System.Drawing.Point(-1, 0);
            this.ImagePanel.Name = "ImagePanel";
            this.ImagePanel.Size = new System.Drawing.Size(1366, 709);
            this.ImagePanel.TabIndex = 1;
            // 
            // ImageGradient
            // 
            this.ImageGradient.BackColor = System.Drawing.Color.Transparent;
            this.ImageGradient.Controls.Add(this.ConfigurationPanel);
            this.ImageGradient.Controls.Add(this.EventsPanel);
            this.ImageGradient.Location = new System.Drawing.Point(0, 0);
            this.ImageGradient.Name = "ImageGradient";
            this.ImageGradient.Size = new System.Drawing.Size(1366, 709);
            this.ImageGradient.TabIndex = 0;
            // 
            // ConfigurationPanel
            // 
            this.ConfigurationPanel.BackColor = System.Drawing.Color.Black;
            this.ConfigurationPanel.Controls.Add(this.ConfigurationHeader);
            this.ConfigurationPanel.Controls.Add(this.ClientLanguageHeader);
            this.ConfigurationPanel.Controls.Add(this.LanguageSelected);
            this.ConfigurationPanel.Controls.Add(this.DefaultScreenHeader);
            this.ConfigurationPanel.Controls.Add(this.DefaultScreen);
            this.ConfigurationPanel.Controls.Add(this.DeleteAccount);
            this.ConfigurationPanel.Controls.Add(this.ConfigurationExitButton);
            this.ConfigurationPanel.Location = new System.Drawing.Point(291, 110);
            this.ConfigurationPanel.Name = "ConfigurationPanel";
            this.ConfigurationPanel.Size = new System.Drawing.Size(800, 500);
            this.ConfigurationPanel.TabIndex = 1;
            this.ConfigurationPanel.Visible = false;
            // 
            // ConfigurationHeader
            // 
            this.ConfigurationHeader.AutoSize = true;
            this.ConfigurationHeader.BackColor = System.Drawing.Color.Transparent;
            this.ConfigurationHeader.Font = new System.Drawing.Font("Oxygen", 14F, System.Drawing.FontStyle.Bold);
            this.ConfigurationHeader.ForeColor = System.Drawing.Color.White;
            this.ConfigurationHeader.Location = new System.Drawing.Point(338, 60);
            this.ConfigurationHeader.Name = "ConfigurationHeader";
            this.ConfigurationHeader.Size = new System.Drawing.Size(110, 31);
            this.ConfigurationHeader.TabIndex = 0;
            this.ConfigurationHeader.Text = "AJUSTES";
            this.ConfigurationHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ClientLanguageHeader
            // 
            this.ClientLanguageHeader.AutoSize = true;
            this.ClientLanguageHeader.BackColor = System.Drawing.Color.Transparent;
            this.ClientLanguageHeader.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientLanguageHeader.ForeColor = System.Drawing.Color.White;
            this.ClientLanguageHeader.Location = new System.Drawing.Point(100, 160);
            this.ClientLanguageHeader.Name = "ClientLanguageHeader";
            this.ClientLanguageHeader.Size = new System.Drawing.Size(181, 26);
            this.ClientLanguageHeader.TabIndex = 1;
            this.ClientLanguageHeader.Text = "Idioma del cliente";
            this.ClientLanguageHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LanguageSelected
            // 
            this.LanguageSelected.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LanguageSelected.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.LanguageSelected.DropDownHeight = 160;
            this.LanguageSelected.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LanguageSelected.Font = new System.Drawing.Font("Times New Roman", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LanguageSelected.FormattingEnabled = true;
            this.LanguageSelected.IntegralHeight = false;
            this.LanguageSelected.ItemHeight = 40;
            this.LanguageSelected.Items.AddRange(new object[] {
            "ESPAÑOL - ESPAÑA (es-ES)",
            "INGLÉS - ESTADOS UNIDOS (en-US)",
            "INGLÉS - REINO UNIDO (en-GB)",
            "FRANCÉS (fr)",
            "ALEMÁN (de)",
            "RUSO (ru)",
            "CHINO (zh)",
            "JAPONÉS (ja)",
            "NORUEGO (no)"});
            this.LanguageSelected.Location = new System.Drawing.Point(377, 150);
            this.LanguageSelected.Name = "LanguageSelected";
            this.LanguageSelected.Size = new System.Drawing.Size(333, 46);
            this.LanguageSelected.TabIndex = 11;
            this.LanguageSelected.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.Combobox_DrawItem);
            this.LanguageSelected.SelectionChangeCommitted += new System.EventHandler(this.LanguageSelected_SelectionChangeCommitted);
            this.LanguageSelected.DropDownClosed += new System.EventHandler(this.Combobox_DropDownClosed);
            // 
            // DefaultScreenHeader
            // 
            this.DefaultScreenHeader.AutoSize = true;
            this.DefaultScreenHeader.BackColor = System.Drawing.Color.Transparent;
            this.DefaultScreenHeader.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DefaultScreenHeader.ForeColor = System.Drawing.Color.White;
            this.DefaultScreenHeader.Location = new System.Drawing.Point(100, 260);
            this.DefaultScreenHeader.Name = "DefaultScreenHeader";
            this.DefaultScreenHeader.Size = new System.Drawing.Size(173, 26);
            this.DefaultScreenHeader.TabIndex = 12;
            this.DefaultScreenHeader.Text = "Pantalla de inicio";
            this.DefaultScreenHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DefaultScreen
            // 
            this.DefaultScreen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DefaultScreen.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.DefaultScreen.DropDownHeight = 160;
            this.DefaultScreen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DefaultScreen.Font = new System.Drawing.Font("Times New Roman", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DefaultScreen.FormattingEnabled = true;
            this.DefaultScreen.IntegralHeight = false;
            this.DefaultScreen.ItemHeight = 40;
            this.DefaultScreen.Items.AddRange(new object[] {
            "Libreria de juegos",
            "Tienda",
            "Aleatorio"});
            this.DefaultScreen.Location = new System.Drawing.Point(377, 250);
            this.DefaultScreen.Name = "DefaultScreen";
            this.DefaultScreen.Size = new System.Drawing.Size(333, 46);
            this.DefaultScreen.TabIndex = 13;
            this.DefaultScreen.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.Combobox_DrawItem);
            this.DefaultScreen.SelectionChangeCommitted += new System.EventHandler(this.DefaultScreen_SelectionChangeCommitted);
            this.DefaultScreen.DropDownClosed += new System.EventHandler(this.Combobox_DropDownClosed);
            // 
            // DeleteAccount
            // 
            this.DeleteAccount.AutoSize = true;
            this.DeleteAccount.BackColor = System.Drawing.Color.Transparent;
            this.DeleteAccount.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DeleteAccount.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteAccount.ForeColor = System.Drawing.Color.Red;
            this.DeleteAccount.Location = new System.Drawing.Point(310, 360);
            this.DeleteAccount.Name = "DeleteAccount";
            this.DeleteAccount.Size = new System.Drawing.Size(140, 26);
            this.DeleteAccount.TabIndex = 14;
            this.DeleteAccount.Text = "Borrar cuenta";
            this.DeleteAccount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.DeleteAccount.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DeleteAccount_MouseClick);
            this.DeleteAccount.MouseEnter += new System.EventHandler(this.DeleteAccount_MouseEnter);
            this.DeleteAccount.MouseLeave += new System.EventHandler(this.DeleteAccount_MouseLeave);
            // 
            // ConfigurationExitButton
            // 
            this.ConfigurationExitButton.AutoSize = true;
            this.ConfigurationExitButton.BackColor = System.Drawing.Color.Transparent;
            this.ConfigurationExitButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ConfigurationExitButton.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConfigurationExitButton.ForeColor = System.Drawing.Color.White;
            this.ConfigurationExitButton.Location = new System.Drawing.Point(340, 445);
            this.ConfigurationExitButton.Name = "ConfigurationExitButton";
            this.ConfigurationExitButton.Size = new System.Drawing.Size(88, 26);
            this.ConfigurationExitButton.TabIndex = 3;
            this.ConfigurationExitButton.Text = "CERRAR";
            this.ConfigurationExitButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ConfigurationExitButton.Click += new System.EventHandler(this.ConfigurationExitButton_Click);
            this.ConfigurationExitButton.MouseEnter += new System.EventHandler(this.ConfigurationExitButton_MouseEnter);
            this.ConfigurationExitButton.MouseLeave += new System.EventHandler(this.ConfigurationExitButton_MouseLeave);
            // 
            // EventsPanel
            // 
            this.EventsPanel.BackColor = System.Drawing.Color.Black;
            this.EventsPanel.Controls.Add(this.EventText);
            this.EventsPanel.Controls.Add(this.EventCodeError);
            this.EventsPanel.Controls.Add(this.EventCode);
            this.EventsPanel.Controls.Add(this.EventSendButton);
            this.EventsPanel.Controls.Add(this.EventExitButton);
            this.EventsPanel.Location = new System.Drawing.Point(291, 260);
            this.EventsPanel.Name = "EventsPanel";
            this.EventsPanel.Size = new System.Drawing.Size(800, 185);
            this.EventsPanel.TabIndex = 14;
            this.EventsPanel.Visible = false;
            // 
            // EventText
            // 
            this.EventText.Font = new System.Drawing.Font("Oxygen", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EventText.ForeColor = System.Drawing.Color.White;
            this.EventText.Location = new System.Drawing.Point(20, 15);
            this.EventText.Name = "EventText";
            this.EventText.Size = new System.Drawing.Size(746, 70);
            this.EventText.TabIndex = 0;
            this.EventText.Text = "Hemos mandado un código a tu correo electrónico, dirígete a tu correo e introduce" +
    " el código para confirmar tu cuenta";
            this.EventText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EventCodeError
            // 
            this.EventCodeError.AutoSize = true;
            this.EventCodeError.Font = new System.Drawing.Font("Oxygen", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EventCodeError.ForeColor = System.Drawing.Color.Red;
            this.EventCodeError.Location = new System.Drawing.Point(560, 100);
            this.EventCodeError.Name = "EventCodeError";
            this.EventCodeError.Size = new System.Drawing.Size(134, 21);
            this.EventCodeError.TabIndex = 18;
            this.EventCodeError.Text = "Código erroneo";
            this.EventCodeError.Visible = false;
            // 
            // EventCode
            // 
            this.EventCode.Font = new System.Drawing.Font("Oxygen", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EventCode.ForeColor = System.Drawing.Color.Black;
            this.EventCode.Location = new System.Drawing.Point(298, 95);
            this.EventCode.MaxLength = 8;
            this.EventCode.Name = "EventCode";
            this.EventCode.Size = new System.Drawing.Size(203, 29);
            this.EventCode.TabIndex = 15;
            this.EventCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.EventCode.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EventCode_KeyUp);
            // 
            // EventSendButton
            // 
            this.EventSendButton.BackColor = System.Drawing.Color.Transparent;
            this.EventSendButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.EventSendButton.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EventSendButton.ForeColor = System.Drawing.Color.White;
            this.EventSendButton.Location = new System.Drawing.Point(255, 142);
            this.EventSendButton.Name = "EventSendButton";
            this.EventSendButton.Size = new System.Drawing.Size(133, 31);
            this.EventSendButton.TabIndex = 16;
            this.EventSendButton.Text = "ENVIAR";
            this.EventSendButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.EventSendButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.EventSendButton_MouseClick);
            this.EventSendButton.MouseEnter += new System.EventHandler(this.EventSend_ExitButton_MouseEnter);
            this.EventSendButton.MouseLeave += new System.EventHandler(this.EventSend_ExitButton_MouseLeave);
            // 
            // EventExitButton
            // 
            this.EventExitButton.BackColor = System.Drawing.Color.Transparent;
            this.EventExitButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.EventExitButton.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EventExitButton.ForeColor = System.Drawing.Color.White;
            this.EventExitButton.Location = new System.Drawing.Point(417, 142);
            this.EventExitButton.Name = "EventExitButton";
            this.EventExitButton.Size = new System.Drawing.Size(133, 31);
            this.EventExitButton.TabIndex = 14;
            this.EventExitButton.Text = "CERRAR";
            this.EventExitButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.EventExitButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.EventExitButton_MouseClick);
            this.EventExitButton.MouseEnter += new System.EventHandler(this.EventSend_ExitButton_MouseEnter);
            this.EventExitButton.MouseLeave += new System.EventHandler(this.EventSend_ExitButton_MouseLeave);
            // 
            // ClientAplication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1365, 708);
            this.Controls.Add(this.MainMenu);
            this.Controls.Add(this.SideMenu);
            this.Controls.Add(this.ImagePanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MaximizeBox = false;
            this.Name = "ClientAplication";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientAplication_FormClosing);
            this.Load += new System.EventHandler(this.ClientAplication_Load);
            this.SideMenu.ResumeLayout(false);
            this.Store.ResumeLayout(false);
            this.Store.PerformLayout();
            this.GameLibrary.ResumeLayout(false);
            this.GameLibrary.PerformLayout();
            this.DownloadInformation.ResumeLayout(false);
            this.Uninstall_Information.ResumeLayout(false);
            this.UserInformation.ResumeLayout(false);
            this.MainMenu.ResumeLayout(false);
            this.GameInfoMenu.ResumeLayout(false);
            this.GameInfoGradient.ResumeLayout(false);
            this.CloseGameInfoBackground.ResumeLayout(false);
            this.GameSettingsBackground.ResumeLayout(false);
            this.MoneyPanel.ResumeLayout(false);
            this.MoneyPanel.PerformLayout();
            this.TileSize_GameFilter.ResumeLayout(false);
            this.TileSize_GameFilter.PerformLayout();
            this.StoreMenu.ResumeLayout(false);
            this.StoreEmpty.ResumeLayout(false);
            this.GameLibraryMenu.ResumeLayout(false);
            this.LibraryEmpty.ResumeLayout(false);
            this.ImagePanel.ResumeLayout(false);
            this.ImageGradient.ResumeLayout(false);
            this.ConfigurationPanel.ResumeLayout(false);
            this.ConfigurationPanel.PerformLayout();
            this.EventsPanel.ResumeLayout(false);
            this.EventsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ImagePanel;
        private System.Windows.Forms.Panel ConfigurationPanel;
        private System.Windows.Forms.Label ConfigurationHeader;
        private System.Windows.Forms.Label ClientLanguageHeader;
        private System.Windows.Forms.ComboBox LanguageSelected;
        private System.Windows.Forms.Label ConfigurationExitButton;
        private System.Windows.Forms.Panel EventsPanel;
        private System.Windows.Forms.Label EventText;
        private System.Windows.Forms.Label EventCodeError;
        private System.Windows.Forms.TextBox EventCode;
        private System.Windows.Forms.Label EventSendButton;
        private System.Windows.Forms.Label EventExitButton;
        private System.Windows.Forms.Panel SideMenu;
        private System.Windows.Forms.Panel UserPhoto;
        private System.Windows.Forms.Panel UserInformation;
        private System.Windows.Forms.Label UserName;
        private System.Windows.Forms.Panel UserConfigurationArrow;
        private System.Windows.Forms.Panel MainMenu;
        private System.Windows.Forms.Panel Store;
        private System.Windows.Forms.Label StoreLabel;
        private System.Windows.Forms.Panel GameLibrary;
        private System.Windows.Forms.Label GameLibraryLabel;
        private System.Windows.Forms.Panel ImageGradient;
        private System.Windows.Forms.Panel StoreMenu;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Panel GameLibraryMenu;
        private System.Windows.Forms.Panel LargeTiles;
        private System.Windows.Forms.Panel MediumTiles;
        private System.Windows.Forms.Panel SmallTiles;
        private System.Windows.Forms.Panel TileSize_GameFilter;
        private System.Windows.Forms.TextBox FilterGame;
        private System.Windows.Forms.ListView StoreAvailableGames;
        private System.Windows.Forms.ImageList GameImages;
        private System.Windows.Forms.ListView GameLibraryAvailableGames;
        private System.Windows.Forms.Panel GameInfoMenu;
        private System.Windows.Forms.Panel CloseGameInfo;
        private System.Windows.Forms.Panel CloseGameInfoBackground;
        private System.Windows.Forms.Panel GameSettingsBackground;
        private System.Windows.Forms.Panel GameSettings;
        private System.Windows.Forms.Button Play_BuyGame;
        private System.Windows.Forms.ContextMenuStrip ContextMenuStrip;
        private System.Windows.Forms.Panel GameInfoGradient;
        private System.Windows.Forms.Label DefaultScreenHeader;
        private System.Windows.Forms.ComboBox DefaultScreen;
        private System.Windows.Forms.Label DeleteAccount;
        private System.Windows.Forms.Panel MoneyPanel;
        private System.Windows.Forms.Label CurrentCurrencyHeader;
        private System.Windows.Forms.Label GamePriceHeader;
        private System.Windows.Forms.Label CurrentCurrency;
        private System.Windows.Forms.Label GamePrice;
        private System.Windows.Forms.Panel StoreEmpty;
        private System.Windows.Forms.Panel StoreEmptyImage;
        private System.Windows.Forms.Label StoreEmptyLabel;
        private System.Windows.Forms.Panel LibraryEmpty;
        private System.Windows.Forms.Panel LibraryEmptyImage;
        private System.Windows.Forms.Label LibraryEmptyLabel;
        private System.Windows.Forms.Panel DownloadInformation;
        private System.Windows.Forms.ProgressBar DownloadProgress;
        private System.Windows.Forms.Label DownloadInformationGameName;
        private System.Windows.Forms.Label DownloadState;
        private System.Windows.Forms.Panel DownloadInformationGameImage;
        private System.Windows.Forms.Panel CloseDownloadInformation;
        private System.Windows.Forms.Panel Uninstall_Information;
        private System.Windows.Forms.Label Uninstall_InformationGameName;
        private System.Windows.Forms.Panel Uninstall_InformationGameImage;
        private System.Windows.Forms.ProgressBar UninstallProgress;
        private System.Windows.Forms.Label UninstallState;
        private System.Windows.Forms.Panel CloseUninstall_Information;
    }
}