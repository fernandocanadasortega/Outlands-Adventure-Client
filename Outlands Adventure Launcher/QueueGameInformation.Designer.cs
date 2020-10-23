namespace Outlands_Adventure_Launcher
{
    partial class QueueGameInformation
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
            this.QueueGameName = new System.Windows.Forms.Label();
            this.QueueState = new System.Windows.Forms.Label();
            this.QueueGameImage = new System.Windows.Forms.Panel();
            this.QueueCancelButton = new System.Windows.Forms.Panel();
            this.QueueRankUp = new System.Windows.Forms.Panel();
            this.QueueRankDown = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // QueueGameName
            // 
            this.QueueGameName.AutoSize = true;
            this.QueueGameName.Font = new System.Drawing.Font("Oxygen", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QueueGameName.Location = new System.Drawing.Point(0, 0);
            this.QueueGameName.Name = "QueueGameName";
            this.QueueGameName.Size = new System.Drawing.Size(0, 26);
            this.QueueGameName.TabIndex = 0;
            this.QueueGameName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // QueueState
            // 
            this.QueueState.AutoSize = true;
            this.QueueState.Font = new System.Drawing.Font("Oxygen", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QueueState.Location = new System.Drawing.Point(0, 0);
            this.QueueState.Name = "QueueState";
            this.QueueState.Size = new System.Drawing.Size(0, 21);
            this.QueueState.TabIndex = 0;
            this.QueueState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // QueueGameImage
            // 
            this.QueueGameImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.QueueGameImage.Location = new System.Drawing.Point(8, 8);
            this.QueueGameImage.Name = "QueueGameImage";
            this.QueueGameImage.Size = new System.Drawing.Size(200, 100);
            this.QueueGameImage.TabIndex = 1;
            // 
            // QueueCancelButton
            // 
            this.QueueCancelButton.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.Dark_Close;
            this.QueueCancelButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.QueueCancelButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.QueueCancelButton.Location = new System.Drawing.Point(8, 8);
            this.QueueCancelButton.Name = "QueueCancelButton";
            this.QueueCancelButton.Size = new System.Drawing.Size(200, 100);
            this.QueueCancelButton.TabIndex = 1;
            this.QueueCancelButton.MouseEnter += new System.EventHandler(this.QueueCancelButton_MouseEnter);
            this.QueueCancelButton.MouseLeave += new System.EventHandler(this.QueueCancelButton_MouseLeave);
            // 
            // QueueRankUp
            // 
            this.QueueRankUp.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.RankGameUp;
            this.QueueRankUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.QueueRankUp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.QueueRankUp.Location = new System.Drawing.Point(8, 8);
            this.QueueRankUp.Name = "QueueRankUp";
            this.QueueRankUp.Size = new System.Drawing.Size(200, 100);
            this.QueueRankUp.TabIndex = 1;
            this.QueueRankUp.MouseEnter += new System.EventHandler(this.RankUp_MouseEnter);
            this.QueueRankUp.MouseLeave += new System.EventHandler(this.RankUp_MouseLeave);
            // 
            // QueueRankDown
            // 
            this.QueueRankDown.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.RankGameDown;
            this.QueueRankDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.QueueRankDown.Cursor = System.Windows.Forms.Cursors.Hand;
            this.QueueRankDown.Location = new System.Drawing.Point(8, 8);
            this.QueueRankDown.Name = "QueueRankDown";
            this.QueueRankDown.Size = new System.Drawing.Size(200, 100);
            this.QueueRankDown.TabIndex = 1;
            this.QueueRankDown.MouseEnter += new System.EventHandler(this.QueueRankDown_MouseEnter);
            this.QueueRankDown.MouseLeave += new System.EventHandler(this.QueueRankDown_MouseLeave);
            // 
            // QueueGameInformation
            // 
            this.Controls.Add(this.QueueGameName);
            this.Controls.Add(this.QueueState);
            this.Controls.Add(this.QueueGameImage);
            this.Controls.Add(this.QueueCancelButton);
            this.Controls.Add(this.QueueRankUp);
            this.Controls.Add(this.QueueRankDown);
            this.Margin = new System.Windows.Forms.Padding(40, 20, 10, 10);
            this.Size = new System.Drawing.Size(910, 100);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label QueueGameName;
        private System.Windows.Forms.Label QueueState;
        private System.Windows.Forms.Panel QueueGameImage;
        private System.Windows.Forms.Panel QueueCancelButton;
        private System.Windows.Forms.Panel QueueRankUp;
        private System.Windows.Forms.Panel QueueRankDown;
    }
}
