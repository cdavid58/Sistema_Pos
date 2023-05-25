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
        private Invoice order;
        public bool exists_client = false;
        


        public bool CreateInvoice(HeaderInvoice hi)
        {
            bool result = false;
            string query = $"call Create_Invoice({hi.number},{hi.user},'{hi.terminal}',{hi.payment_form},{hi.payment_method},'{hi.notes}',{hi.client_id})";
            MySqlCommand cmd = new MySqlCommand(query, c.Conect());
            cmd.ExecuteNonQuery();
            result = true;
            return result;
        }

        public void CreateDetailsInvoice(DetailsInvoice di)
        {
            string query = $"call Details_Invoice({di.code},'{di.product}',{di.quantity},{di.price},{di.discount},{di.tax},{di.ipo},{di.invoice_id})";
            MySqlCommand cmd = new MySqlCommand(query, c.Conect());
            cmd.ExecuteNonQuery();
        }

        public int GetConsecutive()
        {
            int number = 0;
            string query = $"select number from consecutive";
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, c.Conect());
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    number = reader.GetInt32("number");
                }
            }catch(Exception){}
            c.Conect().Close();
            return number;
        }

        public Invoice GetQueryInvoice(int quantity, long code, int type_price)
        {
            order = new Invoice();

            string query = $"CALL getCharacters({quantity}, {code}, {type_price})";
            MySqlCommand cmd = new MySqlCommand(query, c.Conect());
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                order.subtotal = reader.GetInt32("subtotal");
                order.code = reader.GetString("code");
                order.product = reader.GetString("product");
                order.tax = reader.GetInt16("tax");
                order.discount = reader.GetInt16("discount");
                order.cost = reader.GetInt32("cost");
                order.tax_value = reader.GetInt32("tax_value");
                order.booking = reader.GetInt32("booking");
            }
            c.Conect().Close();
            return order;
        }


        public Product GetProduct(long code)
        {
            product = new Product();
            string query = $"select price_1, price_2, price_3,price_4,price_5,price_6,product, quantity, address from inventory where code = {code}";
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
                product.address = reader.GetString("address");
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
            c.Conect().Close();
            return client;
        }


    }

    class HeaderInvoice
    {
        public int number { get; set; }
        public int user { get; set; }
        public string terminal { get; set; }
        public int payment_form { get; set; }
        public int payment_method { get; set; }
        public string notes { get; set; }
        public int client_id { get; set; }
    }

    class DetailsInvoice
    {
        public long code { get; set; }
        public string product { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
        public int discount { get; set; }
        public int tax { get; set; }
        public double ipo { get; set; }
        public int invoice_id { get; set; }
    }


    class Invoice
    {
        public string code { get; set; }
        public string product { get; set; }
        public int quantity { get; set; }
        public int cost { get; set; }
        public int tax { get; set; }
        public int tax_value { get; set; }
        public int discount { get; set; }
        public int ipo { get; set; }
        public int subtotal {get;set;}
        public int booking { get; set; }
    }
}
