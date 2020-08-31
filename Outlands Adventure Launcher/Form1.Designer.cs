namespace Outlands_Adventure_Launcher
{
    partial class Form1
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
            this.listView = new System.Windows.Forms.ListView();
            this.Rellenar = new System.Windows.Forms.Button();
            this.Limpiar = new System.Windows.Forms.Button();
            this.IconImages = new System.Windows.Forms.Button();
            this.LargeImages = new System.Windows.Forms.Button();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView.BackColor = System.Drawing.SystemColors.Control;
            this.listView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(190, 12);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.listView.Size = new System.Drawing.Size(739, 520);
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.Click += new System.EventHandler(this.ListView_Click);
            // 
            // Rellenar
            // 
            this.Rellenar.Location = new System.Drawing.Point(982, 57);
            this.Rellenar.Name = "Rellenar";
            this.Rellenar.Size = new System.Drawing.Size(122, 41);
            this.Rellenar.TabIndex = 1;
            this.Rellenar.Text = "Rellenar";
            this.Rellenar.UseVisualStyleBackColor = true;
            this.Rellenar.Click += new System.EventHandler(this.Rellenar_Click);
            // 
            // Limpiar
            // 
            this.Limpiar.Location = new System.Drawing.Point(982, 356);
            this.Limpiar.Name = "Limpiar";
            this.Limpiar.Size = new System.Drawing.Size(122, 41);
            this.Limpiar.TabIndex = 2;
            this.Limpiar.Text = "Limpiar";
            this.Limpiar.UseVisualStyleBackColor = true;
            this.Limpiar.Click += new System.EventHandler(this.Limpiar_Click);
            // 
            // IconImages
            // 
            this.IconImages.Location = new System.Drawing.Point(982, 247);
            this.IconImages.Name = "IconImages";
            this.IconImages.Size = new System.Drawing.Size(122, 41);
            this.IconImages.TabIndex = 4;
            this.IconImages.Text = "Pequeño";
            this.IconImages.UseVisualStyleBackColor = true;
            this.IconImages.Click += new System.EventHandler(this.SmallImages_Click);
            // 
            // LargeImages
            // 
            this.LargeImages.Location = new System.Drawing.Point(982, 149);
            this.LargeImages.Name = "LargeImages";
            this.LargeImages.Size = new System.Drawing.Size(122, 41);
            this.LargeImages.TabIndex = 5;
            this.LargeImages.Text = "Grande";
            this.LargeImages.UseVisualStyleBackColor = true;
            this.LargeImages.Click += new System.EventHandler(this.LargeImages_Click);
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1116, 555);
            this.Controls.Add(this.LargeImages);
            this.Controls.Add(this.IconImages);
            this.Controls.Add(this.Limpiar);
            this.Controls.Add(this.Rellenar);
            this.Controls.Add(this.listView);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.Button Rellenar;
        private System.Windows.Forms.Button Limpiar;
        private System.Windows.Forms.Button IconImages;
        private System.Windows.Forms.Button LargeImages;
        private System.Windows.Forms.ImageList imageList;
    }
}