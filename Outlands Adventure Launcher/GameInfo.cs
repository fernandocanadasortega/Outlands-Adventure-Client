using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outlands_Adventure_Launcher
{
    class GameInfo
    {
        private string gameName;
        private Image gameImage;
        private decimal gamePrice;
        private string gameDownloadLink;

        public GameInfo(string gameName, Image gameImage, decimal gamePrice, string gameDownloadLink)
        {
            this.gameName = gameName;
            this.gameImage = gameImage;
            this.gamePrice = gamePrice;
            this.gameDownloadLink = gameDownloadLink;
        }

        public string GameName { get => gameName; set => gameName = value; }
        public Image GameImage { get => gameImage; set => gameImage = value; }
        public decimal GamePrice { get => gamePrice; set => gamePrice = value; }
        public string GameDownloadLink { get => gameDownloadLink; set => gameDownloadLink = value; }
    }
}
