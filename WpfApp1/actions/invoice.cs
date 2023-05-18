using MySql.Data.MySqlClient;
using System;
using System.Reflection;
using System.Windows;

namespace WpfApp1.actions
{
    class invoice
    {
        private conector c = new conector();
        private Product product;
        public bool exists_client = false;

        public Product GetQueryInvoice(int quantity, long code, int type_price)
        {
            product = new Product();

            string query = $"CALL getCharacters({quantity}, {code}, {type_price})";
            MySqlCommand cmd = new MySqlCommand(query, c.Conect());
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                product.subtotal = reader.GetInt32("subtotal");
                product.code = reader.GetInt64("code");
                product.product = reader.GetString("product");
                product.tax = reader.GetInt16("tax");
                product.discount = reader.GetInt16("discount");
                product.cost = reader.GetInt64("cost");
                product.tax_value = reader.GetInt32("tax_value");
            }
            
            foreach (PropertyInfo property in typeof(Product).GetProperties())
            {
                object value = property.GetValue(product);
                Console.WriteLine($"{property.Name}: {value}");
            }

            c.Conect().Close();
            return product;
        }


        public Product GetProduct(long code)
        {
            product = new Product();
            string query = $"select price_1, price_2, price_3,price_4,price_5,price_6,product, quantity from inventory where code = {code}";
            MySqlCommand cmd = new MySqlCommand(query, c.Conect());
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                product.price_1 = reader.GetInt32("price_1");
                product.price_2 = reader.GetInt32("price_2");
                product.price_3 = reader.GetInt32("price_3");
                product.price_4 = reader.GetInt32("price_4");
                product.price_5 = reader.GetInt32("price_5");
                product.price_6 = reader.GetInt32("price_6");
                product.quantity = reader.GetInt32("price_6");
                product.product = reader.GetString("product");
            }
            c.Conect().Close();
            return product;
        }

        public Client GetClient(string documentI)
        {
            Client client = new Client();
            string query = $"select name, address, phone, type_client from client where documentI={documentI}";
            MySqlCommand cmd = new MySqlCommand(query, c.Conect());
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                client.name = reader.GetString("name");
                client.address = reader.GetString("address");
                client.phone = !reader.IsDBNull(reader.GetOrdinal("phone")) ? reader.GetString("phone") : "No tiene";
                client.type_client = reader.GetInt16("type_client");
                exists_client = true;
            }
            else
            {
                MessageBox.Show("El cliente que busca no existe", "ALERTA", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            return client;
        }


    }
}
