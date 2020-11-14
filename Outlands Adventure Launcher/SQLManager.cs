using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using MySql.Data.MySqlClient;

namespace Outlands_Adventure_Launcher
{
    /// <summary>
    /// Class in charge of manipulate read and write operations of the database
    /// </summary>
    static class SQLManager
    {
        private readonly static string conectionRoute = @"server=localhost;user id=root;database=outlands_adventure_client";

        /// <summary>
        /// Insert data in the database
        /// Inserta datos en las base de datos
        /// </summary>
        /// <param name="insertQuery">String, SQL query</param>
        /// <returns>String, return a string with the error if the query failed</returns>
        public static string Insert_ModifyData(string insertQuery)
        {
            string queryError = "";
            try
            {
                using (MySqlConnection dbConnection = new MySqlConnection(conectionRoute))
                {
                    dbConnection.Open();
                    MySqlCommand insertCommand = new MySqlCommand(insertQuery, dbConnection);

                    if (insertCommand.ExecuteNonQuery() == 1)
                    {
                        return queryError;
                    }

                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Fetch requested data of the database
        /// Recupera datos de la base de datos
        /// </summary>
        /// <param name="readQuery">String, SQL query</param>
        /// <returns>String, return the requested value or return a string with the error if the query failed</returns>
        public static string SearchQueryData(string readQuery)
        {
            string recoveredData = "";
            try
            {
                using (MySqlConnection dbConnection = new MySqlConnection(conectionRoute))
                {
                    dbConnection.Open();
                    MySqlCommand readCommand = new MySqlCommand(readQuery, dbConnection);

                    MySqlDataReader reader = readCommand.ExecuteReader();

                    if (reader.Read())
                    {
                        return reader[0].ToString();
                    }
                    else
                    {
                        return recoveredData;
                    }
                }
            }
            catch (Exception)
            {
                return "error";
            }
        }

        /// <summary>
        /// Check if some data if duplicated in the database and return the number of rows that contain that data
        /// Mira si existe un dato en la base de datos y retorna el número de filas que contienen el dato que buscas
        /// </summary>
        /// <param name="readQuery">String, SQL query</param>
        /// <returns>Int, number of duplicated data, -1 if there is an error</returns>
        public static int CheckDuplicatedData(string readQuery)
        {
            try
            {
                using (MySqlConnection dbConnection = new MySqlConnection(conectionRoute))
                {
                    dbConnection.Open();
                    MySqlCommand readCommand = new MySqlCommand(readQuery, dbConnection);

                    MySqlDataReader reader = readCommand.ExecuteReader();

                    if (reader.Read())
                    {
                        return Convert.ToInt32(reader[0]);
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// Get the information of a game requested in the SQL query
        /// </summary>
        /// <param name="readOwnedGamesQuery">String, SQL query</param>
        /// <returns>List<GameInfo>, list with the information of the games</returns>
        public static List<GameInfo> ReadGameList(string readOwnedGamesQuery)
        {
            List<GameInfo> gameList = new List<GameInfo>();

            try
            {
                using (MySqlConnection dbConnection = new MySqlConnection(conectionRoute))
                {
                    dbConnection.Open();
                    MySqlCommand readGamesInfo = new MySqlCommand(readOwnedGamesQuery, dbConnection);
                    MySqlDataReader gamesInfoReader = readGamesInfo.ExecuteReader();

                    while (gamesInfoReader.Read())
                    {
                        string gameName = gamesInfoReader[0].ToString();
                        Image gameImage = AssembleImage((byte[]) gamesInfoReader[1]);
                        gameImage.Tag = gameName;
                        decimal gamePrice = (decimal)gamesInfoReader[2];
                        string gameDownloadLink = gamesInfoReader[3].ToString();

                        gameList.Add(new GameInfo(gameName, gameImage, gamePrice, gameDownloadLink));
                    }

                    return gameList;
                }
            }
            catch (Exception)
            {
                return gameList;
            }
        }

        /// <summary>
        /// Restore the image from bytes
        /// </summary>
        /// <param name="imageBlob">byte[], byte[] with the image information</param>
        /// <returns>Bitmap, Image restored</returns>
        private static Bitmap AssembleImage(byte[] imageBlob)
        {
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.Write(imageBlob, 0, Convert.ToInt32(imageBlob.Length));
            Bitmap bitmapImage = new Bitmap(memoryStream, false);
            memoryStream.Dispose();
            return bitmapImage;
        }
    }
}
