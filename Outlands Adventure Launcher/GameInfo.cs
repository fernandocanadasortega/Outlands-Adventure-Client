using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outlands_Adventure_Launcher
{
    /// <summary>
    /// GameInfo class in charge of saving the data of every game in the database
    /// </summary>
    class GameInfo
    {
        private string gameName;
        private Image gameImage;
        private decimal gamePrice;
        private string gameDownloadLink;

        /// <summary>
        /// Constructor of the GameInfo class
        /// </summary>
        /// <param name="gameName">String, game name</param>
        /// <param name="gameImage">Image, game image</param>
        /// <param name="gamePrice">Decimal, game price</param>
        /// <param name="gameDownloadLink">String, game download link</param>
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
