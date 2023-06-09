﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows;

namespace WpfApp1.actions
{
    class Constantes
    {
        public static Dictionary<string, string> Colors;

        public MySqlConnection Conect()
        {
            string connectionString;
            try
            { connectionString = "server=25.35.176.44;database=pos;uid=usuario;Integrated Security=false;SslMode=None;"; }
            catch (Exception) { }
            finally
            { connectionString = "server=192.168.1.62;database=pos;uid=usuario;Integrated Security=false;SslMode=None;"; }
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("No hay conexión con la base de datos", "Alerta", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public void GetColors()
        {
            try
            {
                Colors = new Dictionary<string, string>();
                string query = "select * from color";
                MySqlCommand cmd = new MySqlCommand(query, Conect());
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Colors.Add(reader.GetString("name_color"), "#"+reader.GetString("hexadecimal"));
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Error GETCOLORS {ex.Message}");
            }
            Conect().Close();
        }

    }
}
