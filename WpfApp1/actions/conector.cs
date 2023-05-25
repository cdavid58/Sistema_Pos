using System;
using MySql.Data.MySqlClient;
using System.IO;
using System.Globalization;
using System.Windows;
using System.Collections.Generic;

namespace WpfApp1
{
    class conector
    {
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

    }

    class Create_FileStream
    {
        public void App_StartUp(string name)
        {
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
                string archivoTemporal = Path.Combine(filePath, name + ".txt");

                using (var fileStream = new FileStream(archivoTemporal, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using (var writer = new StreamWriter(fileStream))
                    {

                        //writer.WriteLine($"ID de factura: {factura.Id}");
                        writer.WriteLine("ID de factura: 514");
                        DateTime now = DateTime.Now;
                        writer.WriteLine("Fecha de factura: " + now.ToString("yyyy-MM-ddTHH:mm:ss"));
                        writer.WriteLine("Cliente: David");
                        writer.WriteLine("Total: 50000");
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error al crear el archivo temporal.");
            }
        }

        public void FileStreamReader(string name)
        {
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
                string archivoTemporal = Path.Combine(filePath, name + ".txt");
                using (var fileStream = new FileStream(archivoTemporal, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = new StreamReader(fileStream))
                    {
                        string idLine = reader.ReadLine();
                        string fechaLine = reader.ReadLine();
                        string clienteLine = reader.ReadLine();
                        string totalLine = reader.ReadLine();

                        int id = int.Parse(idLine.Split(':')[1].Trim());
                        DateTime.TryParse(fechaLine.Split(':')[1].Trim(), out DateTime fecha);
                        string fechaStr = fecha.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        string horaStr = fecha.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);
                        string cliente = clienteLine.Split(':')[1].Trim();
                        decimal total = decimal.Parse(totalLine.Split(':')[1].Trim(), CultureInfo.InvariantCulture);

                        Console.WriteLine(id.ToString());
                        Console.WriteLine(fechaStr.ToString());
                        Console.WriteLine(horaStr.ToString());
                        Console.WriteLine(cliente);
                        Console.WriteLine(total.ToString());
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error al leer el archivo temporal.");
            }
        }
    }

    class Query
    {
        private conector c = new conector();
        public static int type_company;
        public static Dictionary<string, int> list_permissions;
        public static string user_name;
        public static int pk_user = 0, user_id = 0;
        public bool Login(string psswd)
        {
            bool result = false;
            try
            {
                using (c.Conect())
                {
                    try
                    {
                        string query = "select * from user where psswd = " + psswd;
                        MySqlCommand command = new MySqlCommand(query, c.Conect());

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Acceder a los valores de las columnas
                                int id = reader.GetInt32("psswd");
                                user_name = reader.GetString("username");
                                pk_user = reader.GetInt32("id");
                                user_id = reader.GetInt32("id");
                                Query_permissions(user_id);
                                result = true;
                            }
                            else
                            {
                                MessageBox.Show("No existe");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al ejecutar la consulta: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            c.Conect().Close();
            return result;
        }

        private void Query_permissions(int pk_user)
        {
            list_permissions = new Dictionary<string, int>();
            try
            {
                using (c.Conect())
                {
                    try
                    {
                        string query = "select p.modules_id as 'pk_modules', m.name as 'name_modules' from permissions as p" +
                                        " inner join modules as m on m.id = p.modules_id " +
                                        " where active = 1 and p.user_id = " + pk_user;
                        MySqlCommand command = new MySqlCommand(query, c.Conect());

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list_permissions.Add(reader.GetString("name_modules"), reader.GetInt32("pk_modules"));
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al ejecutar la consulta: " + ex.Message);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            c.Conect().Close();
        }

        private void Create_Inventory(int type_company)
        {
            string query;
            if (type_company == 1)
            {
                query = "CREATE TABLE IF NOT EXISTS inventory(" +
                    "id INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY COLLATE 'utf8mb4_spanish_ci'," +
                    "code INT UNSIGNED NOT NULL UNIQUE COLLATE 'utf8mb4_spanish_ci'," +
                    "product VARCHAR(200) NOT NULL UNIQUE COLLATE 'utf8mb4_spanish_ci'," +
                    "cost DECIMAL(10, 2) NOT NULL COLLATE 'utf8mb4_spanish_ci'," +
                    "price_1 DECIMAL(10, 2) NOT NULL COLLATE 'utf8mb4_spanish_ci'," +
                    "price_2 DECIMAL(10, 2) NOT NULL COLLATE 'utf8mb4_spanish_ci'," +
                    "price_3 DECIMAL(10, 2) NOT NULL COLLATE 'utf8mb4_spanish_ci'," +
                    "price_4 DECIMAL(10, 2) NOT NULL COLLATE 'utf8mb4_spanish_ci'," +
                    "price_5 DECIMAL(10, 2) NOT NULL COLLATE 'utf8mb4_spanish_ci'," +
                    "price_6 DECIMAL(10, 2) NOT NULL COLLATE 'utf8mb4_spanish_ci'," +
                    "quantity INT UNSIGNED NOT NULL COLLATE 'utf8mb4_spanish_ci'," +
                    "pack INT NOT NULL COLLATE 'utf8mb4_spanish_ci'," +
                    "display INT NOT NULL COLLATE 'utf8mb4_spanish_ci'," +
                    "quantity_static INT UNSIGNED NOT NULL COLLATE 'utf8mb4_spanish_ci'," +
                    "pack_static INT NOT NULL COLLATE 'utf8mb4_spanish_ci'," +
                    "display_static INT NOT NULL COLLATE 'utf8mb4_spanish_ci'," +
                    "tax INT NOT NULL COLLATE 'utf8mb4_spanish_ci'," +
                    "ipo DECIMAL(10, 2) NOT NULL COLLATE 'utf8mb4_spanish_ci'," +
                    "discount INT NOT NULL COLLATE 'utf8mb4_spanish_ci'," +
                    "brand VARCHAR(50) NULL COLLATE 'utf8mb4_spanish_ci'," +
                    "stock INT(10) NULL COLLATE 'utf8mb4_spanish_ci', " +
                    "address VARCHAR(50) NULL COLLATE 'utf8mb4_spanish_ci'," +
                    "reserva INT NULL COLLATE 'utf8mb4_spanish_ci'," +
                    "subcategory_id INT UNSIGNED COLLATE 'utf8mb4_spanish_ci'," +
                    "FOREIGN KEY (subcategory_id) REFERENCES subcategory(id)" +
                ");";

            }
            else
            {
                query = "CREATE TABLE IF NOT EXISTS inventory (" +
                    "id INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY," +
                    "code INT UNSIGNED NOT NULL UNIQUE," +
                    "product VARCHAR(200) NOT NULL UNIQUE," +
                    "cost DECIMAL(10, 2) NOT NULL," +
                    "price_1 DECIMAL(10, 2) NOT NULL," +
                    "price_2 DECIMAL(10, 2) NOT NULL," +
                    "price_3 DECIMAL(10, 2) NOT NULL," +
                    "price_4 DECIMAL(10, 2) NOT NULL," +
                    "price_5 DECIMAL(10, 2) NOT NULL," +
                    "price_6 DECIMAL(10, 2) NOT NULL," +
                    "quantity INT UNSIGNED NOT NULL," +
                    "pack INT NOT NULL," +
                    "display INT NOT NULL," +
                    "tax INT NOT NULL," +
                    "ipo DECIMAL(10, 2) NOT NULL," +
                    "discount INT NOT NULL," +
                    "brand VARCHAR(50) NULL," +
                    "stock INT(10) NULL, " +
                    "address VARCHAR(50) NULL," +
                    "reserva INT NULL," +
                    "subcategory_id INT UNSIGNED," +
                    "FOREIGN KEY (subcategory_id) REFERENCES subcategory(id)" +
                ");";
            }
            MySqlCommand cmd = new MySqlCommand(query, c.Conect());
            cmd.ExecuteNonQuery();
            c.Conect().Close();
        }



        public bool Query_Block_Company(int nit)
        {
            bool result = true;
            bool inventory = true;
            try
            {
                string query = "select id, block, type_company,inventory from company where nit = " + nit;
                MySqlCommand command = new MySqlCommand(query, c.Conect());
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader.GetInt32("block") == 0)
                        {
                            result = false;
                            pk_user = reader.GetInt32("id");
                            type_company = reader.GetInt32("type_company");

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            c.Conect().Close();

            if (!inventory)
            {
                MessageBox.Show("Creare el inventario");
                Create_Inventory(type_company);
            }
            return result;
        }
    }


    class QueryInventory
    {
        private conector c = new conector();

        public MySqlDataAdapter GetCategory()
        {
            string query = "SELECT id, name_category FROM category";
            MySqlCommand cmd = new MySqlCommand(query, c.Conect());
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            c.Conect().Close();
            return adapter;
        }


        public MySqlDataAdapter GetSubcategory(int id)
        {
            string query = "SELECT id, name_subcategory FROM subcategory where category_id = " + id;
            MySqlCommand cmd = new MySqlCommand(query, c.Conect());
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            c.Conect().Close();
            return adapter;
        }
    }

}



