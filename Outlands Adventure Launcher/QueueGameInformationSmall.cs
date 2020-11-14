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
    public partial class QueueGameInformationSmall : Panel
    {
        private Image gameImage;
        private string gameName;

        public QueueGameInformationSmall(Image gameImage, string gameName)
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

            QueueGameImage.Size = new Size(41, 47);
            QueueGameImage.Location = new Point(5, 5);
            QueueGameImage.BackgroundImage = gameImage;

            QueueGameName.Size = new Size(293, 14);
            QueueGameName.Location = new Point(57, 10);
            QueueGameName.TextAlign = ContentAlignment.MiddleLeft;
            QueueGameName.Text = gameName;

            QueueCancelButton.Size = new Size(12, 12);
            QueueCancelButton.Location = new Point(457, 3);

            QueueRankUp.Size = new Size(12, 12);
            QueueRankUp.Location = new Point(457, 41);

            QueueRankDown.Size = new Size(12, 12);
            QueueRankDown.Location = new Point(436, 41);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            SetControlsProperties();
        }

        private void RankUp_MouseEnter(object sender, EventArgs e)
        {
            MultipleResources.ShowToolTip(QueueRankUp, LanguageResx.ClientLanguage.priorityUp);
        }

        private void RankUp_MouseLeave(object sender, EventArgs e)
        {
            MultipleResources.HideToolTip(QueueRankUp);
        }

        private void QueueRankDown_MouseEnter(object sender, EventArgs e)
        {
            MultipleResources.ShowToolTip(QueueRankDown, LanguageResx.ClientLanguage.priorityDown);
        }

        private void QueueRankDown_MouseLeave(object sender, EventArgs e)
        {
            MultipleResources.HideToolTip(QueueRankDown);
        }

        private void QueueCancelButton_MouseEnter(object sender, EventArgs e)
        {
            MultipleResources.ShowToolTip(QueueCancelButton, LanguageResx.ClientLanguage.download_cancel);
        }

        private void QueueCancelButton_MouseLeave(object sender, EventArgs e)
        {
            MultipleResources.HideToolTip(QueueCancelButton);
        }
    }
}
