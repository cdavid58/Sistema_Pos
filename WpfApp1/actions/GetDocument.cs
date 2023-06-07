using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows;
using WpfApp1.forms;

namespace WpfApp1.actions
{
    class GetDocument
    {
        private conector c = new conector();
        private int id_document = 0, id_supplier = 0;

        public List<Shopping_Object> GetShopping(int number)
        {
            List<Shopping_Object> data = new List<Shopping_Object>();
            string query = $"CALL GetCopyDocument({number},6)";
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, c.Conect());
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Shopping_Object so = new Shopping_Object
                    {
                        number = reader.GetInt32("number"),
                        number_invoice = reader.GetInt32("number_invoice"),
                        terminal = reader.GetString("terminal"),
                        payment_form = reader.GetInt32("payment_form_id"),
                        payment_method = reader.GetInt32("payment_method_id")
                    };
                    id_document = reader.GetInt32("id");
                    id_supplier = reader.GetInt32("supplier_id");
                    data.Add(so);
                }
            }
            catch(Exception ex){
                MessageBox.Show(ex.Message);
            }
            c.Conect().Close();
            return data;
        }

        public List<Details_Shopping> GetDetailsShopping()
        {
            List<Details_Shopping> data = new List<Details_Shopping>();
            string query = $"CALL GetDetailsCopyDocument({id_document},6)";
            try
            {
                Details_Shopping ds;
                MySqlCommand cmd = new MySqlCommand(query, c.Conect());
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ds = new Details_Shopping {
                        code = reader.GetString("code"),
                        product = reader.GetString("product"),
                        quantity = reader.GetInt32("quantity"),
                        cost = reader.GetInt32("cost"),
                        discount = reader.GetInt32("cost"),
                        tax = reader.GetInt32("discount"),
                        ipo = reader.GetInt32("ipo"),
                        price_1 = reader.GetInt32("price_1"),
                        price_2 = reader.GetInt32("price_2"),
                        price_3 = reader.GetInt32("price_3"),
                        price_4 = reader.GetInt32("price_4"),
                        price_5 = reader.GetInt32("price_5"),
                        price_6 = reader.GetInt32("price_6")
                    };
                    data.Add(ds);
                }
            }
            catch (Exception) { }
            c.Conect().Close();
            return data;
        }

        public Dictionary<string,string> GetSupplier()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            string query = $"CALL GetSupplierDocument({id_supplier})";
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, c.Conect());
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    data.Add("name", reader.GetString("name"));
                    data.Add("address", reader.GetString("address"));
                    data.Add("phone", reader.GetString("phone"));
                }
            }
            catch (Exception) { }
            return data;
        }

    }
}
