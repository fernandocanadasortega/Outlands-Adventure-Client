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
        // Cual es el server al que me conecto?
        // Si no he pagado un dominio pero si he pagado el hosting puedo usar la base de datos?
        private readonly static string server = "sql364.main-hosting.eu";
        private readonly static string database = "u273393391_Outlands_ADV";
        private readonly static string uid = "u273393391_Napo";
        private readonly static string password = "Outlands_Client_Password2";
        private readonly static string conectionRoute = "Server=" + server + "; Database=" + database + "; Uid=" + uid + "; Pwd=" + password;
        //private readonly static string conectionRoute = "server=127.0.0.1:3306; uid=u273393391_Napo; pwd=Outlands_Client_Password2; database=u273393391_Outlands_ADV";

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
        /// Write in the database the game progress of that user
        /// </summary>
        /// <param name="gameProgress">byte[], json file game progress</param>
        /// <param name="userEmail">String, user email attached to the game progress</param>
        /// <param name="gameName">String, game name attached to the game progress</param>
        /// <returns></returns>
        public static string WriteGameProgress(byte[] gameProgress, string userEmail, string gameName)
        {
            try
            {
                using (MySqlConnection dbConnection = new MySqlConnection(conectionRoute))
                {
                    dbConnection.Open();
                    //MySqlCommand updateCommand = new MySqlCommand(updateQuery, dbConnection);

                    //UPDATE games_owned SET gameProgression = value WHERE user_email LIKE '' AND gameName LIKE ''

                    using (MySqlCommand updateCommand = new MySqlCommand("UPDATE games_owned SET gameProgression = @gameProgress " +
                        "WHERE user_email LIKE '" + userEmail + "' AND gameName LIKE '" + gameName + "'",
                      dbConnection))
                    {
                        updateCommand.Parameters.Add("@gameProgress", MySqlDbType.Blob).Value = gameProgress;

                        if (updateCommand.ExecuteNonQuery() == 1)
                        {
                            return "";
                        }
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
        /// Get the user's game progress
        /// </summary>
        /// <param name="gameProgressQuery">String, SQL query</param>
        /// <returns>byte[], json file game progress</returns>
        public static byte[] ReadGameProgress(string gameProgressQuery)
        {
            bool conectionOk = false; // Se usa para saber si el catch es de error de conexión o por que no hay archivos de guardado en la nube
            try
            {
                using (MySqlConnection dbConnection = new MySqlConnection(conectionRoute))
                {
                    dbConnection.Open();
                    MySqlCommand readGameProgress = new MySqlCommand(gameProgressQuery, dbConnection);
                    MySqlDataReader gamesInfoReader = readGameProgress.ExecuteReader();

                    if (gamesInfoReader.Read())
                    {
                        conectionOk = true;
                        return (byte[])gamesInfoReader[0];
                    }

                    throw new Exception();
                }
            }
            catch (Exception)
            {
                if (conectionOk) return new byte[0];
                else return null;
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
