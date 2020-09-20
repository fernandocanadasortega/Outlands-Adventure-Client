namespace Outlands_Adventure_Launcher
{
    partial class CloseClientAplicationPopUp
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
            this.ExitButton = new System.Windows.Forms.Button();
            this.LogoutButton = new System.Windows.Forms.Button();
            this.ExitHeader = new System.Windows.Forms.Label();
            this.ExitLabel = new System.Windows.Forms.Label();
            this.CancelExit = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // ExitButton
            // 
            this.ExitButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(40)))));
            this.ExitButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ExitButton.Font = new System.Drawing.Font("Oxygen", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExitButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.ExitButton.Location = new System.Drawing.Point(32, 111);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(162, 41);
            this.ExitButton.TabIndex = 0;
            this.ExitButton.Text = "Salir";
            this.ExitButton.UseVisualStyleBackColor = false;
            this.ExitButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ExitButton_MouseClick);
            this.ExitButton.MouseEnter += new System.EventHandler(this.ExitButton_MouseEnter);
            this.ExitButton.MouseLeave += new System.EventHandler(this.ExitButton_MouseLeave);
            // 
            // LogoutButton
            // 
            this.LogoutButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(40)))));
            this.LogoutButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LogoutButton.Font = new System.Drawing.Font("Oxygen", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogoutButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.LogoutButton.Location = new System.Drawing.Point(226, 111);
            this.LogoutButton.Name = "LogoutButton";
            this.LogoutButton.Size = new System.Drawing.Size(162, 41);
            this.LogoutButton.TabIndex = 1;
            this.LogoutButton.Text = "Cerrar sesión";
            this.LogoutButton.UseVisualStyleBackColor = false;
            this.LogoutButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LogoutButton_MouseClick);
            this.LogoutButton.MouseEnter += new System.EventHandler(this.LogoutButton_MouseEnter);
            this.LogoutButton.MouseLeave += new System.EventHandler(this.LogoutButton_MouseLeave);
            // 
            // ExitHeader
            // 
            this.ExitHeader.AutoSize = true;
            this.ExitHeader.Font = new System.Drawing.Font("Oxygen", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExitHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.ExitHeader.Location = new System.Drawing.Point(115, 13);
            this.ExitHeader.Name = "ExitHeader";
            this.ExitHeader.Size = new System.Drawing.Size(191, 31);
            this.ExitHeader.TabIndex = 2;
            this.ExitHeader.Text = "¿SALIR AHORA?";
            // 
            // ExitLabel
            // 
            this.ExitLabel.AutoSize = true;
            this.ExitLabel.Font = new System.Drawing.Font("Oxygen", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExitLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.ExitLabel.Location = new System.Drawing.Point(93, 60);
            this.ExitLabel.Name = "ExitLabel";
            this.ExitLabel.Size = new System.Drawing.Size(233, 21);
            this.ExitLabel.TabIndex = 3;
            this.ExitLabel.Text = "¿Quieres salir o cerrar sesión?";
            // 
            // CancelExit
            // 
            this.CancelExit.BackgroundImage = global::Outlands_Adventure_Launcher.Properties.Resources.close;
            this.CancelExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CancelExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CancelExit.Location = new System.Drawing.Point(390, 5);
            this.CancelExit.Name = "CancelExit";
            this.CancelExit.Size = new System.Drawing.Size(25, 25);
            this.CancelExit.TabIndex = 4;
            this.CancelExit.TabStop = true;
            this.CancelExit.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CancelExit_MouseClick);
            this.CancelExit.MouseEnter += new System.EventHandler(this.CancelExit_MouseEnter);
            this.CancelExit.MouseLeave += new System.EventHandler(this.CancelExit_MouseLeave);
            // 
            // CloseClientAplicationPopUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(420, 164);
            this.Controls.Add(this.ExitHeader);
            this.Controls.Add(this.ExitLabel);
            this.Controls.Add(this.CancelExit);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.LogoutButton);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(40)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CloseClientAplicationPopUp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.CloseClientAplicationPopUp_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button LogoutButton;
        private System.Windows.Forms.Label ExitHeader;
        private System.Windows.Forms.Label ExitLabel;
        private System.Windows.Forms.Panel CancelExit;
    }
}