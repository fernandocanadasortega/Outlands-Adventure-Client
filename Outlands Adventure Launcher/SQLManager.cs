using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using MySql.Data.MySqlClient;

namespace Outlands_Adventure_Launcher
{
    static class SQLManager
    {
        private static string conectionRoute = @"server=localhost;user id=root;database=outlands_adventure_client";

        public static string Insert_ModifyUser(string insertQuery)
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

        // Recupera un dato de la base de datos
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

        // Mira si existe un dato en la base de datos y retorna el número de filas que contienen el dato que buscas
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
    }
}
