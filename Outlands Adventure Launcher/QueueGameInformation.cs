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
    public partial class QueueGameInformation : Panel
    {
        private Image gameImage;
        private string gameName;

        public QueueGameInformation(Image gameImage, string gameName)
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

            QueueGameImage.Size = new Size(60, 65);
            QueueGameImage.Location = new Point(8, 8);
            QueueGameImage.BackgroundImage = gameImage;

            QueueGameName.Size = new Size(420, 23);
            QueueGameName.Location = new Point(82, 21);
            QueueGameName.TextAlign = ContentAlignment.MiddleLeft;
            QueueGameName.Text = gameName;

            QueueCancelButton.Size = new Size(18, 18);
            QueueCancelButton.Location = new Point(655, 5);

            QueueRankUp.Size = new Size(18, 18);
            QueueRankUp.Location = new Point(655, 56);

            QueueRankDown.Size = new Size(18, 18);
            QueueRankDown.Location = new Point(625, 56);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            SetControlsProperties();
        }

        private void RankUp_MouseEnter(object sender, EventArgs e)
        {
            MultipleResources.ShowToolTip(QueueRankUp, ClientLanguage.priorityUp);
        }

        private void RankUp_MouseLeave(object sender, EventArgs e)
        {
            MultipleResources.HideToolTip(QueueRankUp);
        }

        private void QueueRankDown_MouseEnter(object sender, EventArgs e)
        {
            MultipleResources.ShowToolTip(QueueRankDown, ClientLanguage.priorityDown);
        }

        private void QueueRankDown_MouseLeave(object sender, EventArgs e)
        {
            MultipleResources.HideToolTip(QueueRankDown);
        }

        private void QueueCancelButton_MouseEnter(object sender, EventArgs e)
        {
            MultipleResources.ShowToolTip(QueueCancelButton, ClientLanguage.download_cancel);
        }

        private void QueueCancelButton_MouseLeave(object sender, EventArgs e)
        {
            MultipleResources.HideToolTip(QueueCancelButton);
        }
    }
}
