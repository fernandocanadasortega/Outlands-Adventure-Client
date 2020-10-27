namespace Outlands_Adventure_Launcher
{
    partial class DownloadingGameInformation
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.DownloadGameName = new System.Windows.Forms.Label();
            this.DownloadProgressbar = new System.Windows.Forms.ProgressBar();
            this.DownloadGameImage = new System.Windows.Forms.Panel();
            this.DownloadCancelButton = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // DownloadGameName
            // 
            this.DownloadGameName.AutoSize = true;
            this.DownloadGameName.Font = new System.Drawing.Font("Oxygen", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DownloadGameName.Location = new System.Drawing.Point(0, 0);
            this.DownloadGameName.Name = "DownloadGameName";
            this.DownloadGameName.Size = new System.Drawing.Size(0, 21);
            this.DownloadGameName.TabIndex = 0;
            this.DownloadGameName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DownloadProgressbar
            // 
            this.DownloadProgressbar.Location = new System.Drawing.Point(0, 0);
            this.DownloadProgressbar.Name = "DownloadProgressbar";
            this.DownloadProgressbar.Size = new System.Drawing.Size(100, 23);
            this.DownloadProgressbar.TabIndex = 0;
            // 
            // DownloadGameImage
            // 
            this.DownloadGameImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.DownloadGameImage.Location = new System.Drawing.Point(8, 8);
            this.DownloadGameImage.Name = "DownloadGameImage";
            this.DownloadGameImage.Size = new System.Drawing.Size(200, 100);
            this.DownloadGameImage.TabIndex = 1;
            // 
            // DownloadCancelButton
            // 
            this.DownloadCancelButton.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.Dark_Close;
            this.DownloadCancelButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.DownloadCancelButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DownloadCancelButton.Location = new System.Drawing.Point(8, 8);
            this.DownloadCancelButton.Name = "DownloadCancelButton";
            this.DownloadCancelButton.Size = new System.Drawing.Size(200, 100);
            this.DownloadCancelButton.TabIndex = 1;
            this.DownloadCancelButton.MouseEnter += new System.EventHandler(this.DownloadCancelButton_MouseEnter);
            this.DownloadCancelButton.MouseLeave += new System.EventHandler(this.DownloadCancelButton_MouseLeave);
            // 
            // DownloadingGameInformation
            // 
            this.Controls.Add(this.DownloadGameName);
            this.Controls.Add(this.DownloadProgressbar);
            this.Controls.Add(this.DownloadGameImage);
            this.Controls.Add(this.DownloadCancelButton);
            this.Margin = new System.Windows.Forms.Padding(40, 20, 10, 10);
            this.Size = new System.Drawing.Size(910, 100);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label DownloadGameName;
        private System.Windows.Forms.ProgressBar DownloadProgressbar;
        private System.Windows.Forms.Panel DownloadGameImage;
        private System.Windows.Forms.Panel DownloadCancelButton;
    }
}
