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
    public partial class DownloadingGameInformationSmall : Panel
    {
        private Image gameImage;
        private string gameName;

        public DownloadingGameInformationSmall(Image gameImage, string gameName)
        {
            this.gameImage = gameImage;
            this.gameName = gameName;

            InitializeComponent();
        }

        private void SetControlsProperties()
        {
            this.Size = new Size(476, 59);
            this.Margin = new Padding(20, 8, 8, 4);
            this.BackColor = Color.White;

            DownloadGameImage.Size = new Size(41, 47);
            DownloadGameImage.Location = new Point(5, 5);
            DownloadGameImage.BackgroundImage = gameImage;

            DownloadGameName.Size = new Size(293, 14);
            DownloadGameName.Location = new Point(57, 8);
            DownloadGameName.TextAlign = ContentAlignment.MiddleLeft;
            DownloadGameName.Text = gameName;

            DownloadCancelButton.Size = new Size(12, 12);
            DownloadCancelButton.Location = new Point(457, 3);

            DownloadProgressbar.Size = new Size(364, 12);
            DownloadProgressbar.Location = new Point(60, 25);
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
