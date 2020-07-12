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

        public static string InsertNewUser(string insertQuery)
        {
            // Poner pop up pantalla de carga
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

        public static int CheckDuplicatedData(string readQuery)
        {
            // Poner pop up pantalla de carga
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
                        MessageBox.Show("No se encuentran coincidencias");
                        return 0;
                    }
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static void ModifyUserData()
        {
            // Poner pop up pantalla de carga
            string updateQuery = "UPDATE user_information SET user_password = 'nueva nueva contraseña'" +
                " WHERE user_email LIKE 'email De napo'";
            try
            {
                using (MySqlConnection dbConnection = new MySqlConnection(conectionRoute))
                {
                    dbConnection.Open();
                    MySqlCommand updateCommand = new MySqlCommand(updateQuery, dbConnection);

                    if (updateCommand.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Usuario modificado :D");
                    }
                    else
                    {
                        MessageBox.Show("Ha ocurrido un error D:");
                        throw new Exception();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
