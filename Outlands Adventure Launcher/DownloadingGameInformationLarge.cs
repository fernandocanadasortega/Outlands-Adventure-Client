using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Outlands_Adventure_Launcher
{
    public partial class DownloadingGameInformationLarge : Panel
    {
        private Image gameImage;
        private string gameName;

        public DownloadingGameInformationLarge(Image gameImage, string gameName)
        {
            this.gameImage = gameImage;
            this.gameName = gameName;

            InitializeComponent();
        }

        private void SetControlsProperties()
        {
            this.Size = new Size(682, 81);
            this.Margin = new Padding(30, 16, 8, 8);
            this.BackColor = Color.White;

            DownloadGameImage.Size = new Size(60, 65);
            DownloadGameImage.Location = new Point(8, 8);
            DownloadGameImage.BackgroundImage = gameImage;

            DownloadGameName.Size = new Size(420, 19);
            DownloadGameName.Location = new Point(82, 11);
            DownloadGameName.TextAlign = ContentAlignment.MiddleLeft;
            DownloadGameName.Text = gameName;

            DownloadCancelButton.Size = new Size(18, 18);
            DownloadCancelButton.Location = new Point(655, 5);

            DownloadProgressbar.Size = new Size(522, 16);
            DownloadProgressbar.Location = new Point(86, 34);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            SetControlsProperties();
        }

        private void DownloadCancelButton_MouseEnter(object sender, EventArgs e)
        {
            MultipleResources.ShowToolTip(DownloadCancelButton, LanguageResx.ClientLanguage.download_cancel);
        }

        private void DownloadCancelButton_MouseLeave(object sender, EventArgs e)
        {
            MultipleResources.HideToolTip(DownloadCancelButton);
        }
    }
}
