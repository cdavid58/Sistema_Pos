using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Windows;
using WpfApp1.forms;

namespace WpfApp1.actions
{
    public class shoppings
    {
        private conector c = new conector();
        public static int shopping_id;

        public void Create_Wallet_Shopping(int amount, int duration)
        {
            string query = $"CALL Create_Wallet_Shopping({amount},{duration},{shopping_id})";
            MySqlTransaction transaction = c.Conect().BeginTransaction();
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, c.Conect(), transaction);
                cmd.ExecuteNonQuery();
                transaction.Commit();

            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                transaction.Rollback();
            }
            c.Conect().Close();
        }

        public void Create_Shopping(Shopping_Object so)
        {
            string query = $"SELECT create_shopping({so.number},{so.number_invoice},'{so.terminal}',{so.payment_form},{so.payment_method},{so.user_id},{so.supplier_id})";
            MySqlTransaction transaction = c.Conect().BeginTransaction();
            try
            {

                MySqlCommand cmd = new MySqlCommand(query, c.Conect(), transaction);
                shopping_id = Convert.ToInt32(cmd.ExecuteScalar());
                transaction.Commit();
            }
            catch (Exception ex) {
                transaction.Rollback();
                Console.WriteLine("Error en Create_Shopping " + ex.Message);
            }
            c.Conect().Close();
        }

        public bool Create_DetailsShopping(Details_Shopping ds)
        {
            bool result = false;
            string query = $"insert into details_shopping(code,product,quantity,cost,discount,tax,ipo,shopping_id," +
                $"price_1,price_2,price_3,price_4,price_5,price_6)" +
                $"values(" +
                $"'{ds.code}','{ds.product}',{ds.quantity},{ds.cost},{ds.discount},{ds.tax},{ds.ipo},{ds.shopping_id}," +
                $"{ds.price_1},{ds.price_2},{ds.price_3},{ds.price_4},{ds.price_5},{ds.price_6}" +
                $")";
            MySqlTransaction transaction = c.Conect().BeginTransaction();
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, c.Conect(), transaction);
                cmd.ExecuteNonQuery();
                transaction.Commit();
                result = true;
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine("Error en Create_DetailsShopping " + ex.Message+ $" {query}");
                result = false;
            }
            c.Conect().Close();
            return result;
        }


        public Product GetProductShopping(int quantity, string codeproduct)
        {
            Product p = new Product();
            string query = $"call inventory_shopping({quantity},{codeproduct})";
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, c.Conect());
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    p.tax_value = reader.GetInt32("tax_value");
                    p.code = long.Parse(reader.GetString("code"));
                    p.discount = reader.GetInt32("discount");
                }
            }
            catch (Exception) { }
            return p;
        }

        public MySqlDataAdapter GetSupplier()
        {
            string query = "CALL GetSupplier()";
            MySqlCommand cmd = new MySqlCommand(query,c.Conect());
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            c.Conect().Close();
            return adapter;
        }

        public bool QueryNumberInvoice(long number)
        {
            bool result = false;
            int supplier = int.Parse(shopping.Data_Supplier["supplier_id"]);
            string query = $"select checkout_number_invoice({number},{supplier});";
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, c.Conect());
                result = (bool)cmd.ExecuteScalar();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        public string getconsecutive(int id)
        {
            string number = null;
            string query = $"call GetConsecutive({id})";
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, c.Conect());
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    number = reader.GetString("number");
                }
                else
                {
                    number = "0";
                }
                c.Conect().Close();
            }
            catch (Exception) { }
            return number;
        }
    }

    public class Details_Shopping : IEnumerable
    {
        public string code { get; set; }
        public string product { get; set; }
        public int quantity { get; set; }
        public double cost { get; set; }
        public int discount { get; set; }
        public int tax { get; set; }
        public int ipo { get; set; }
        public int shopping_id { get; set; }
        public double price_1 { get; set; }
        public double price_2 { get; set; }
        public double price_3 { get; set; }
        public double price_4 { get; set; }
        public double price_5 { get; set; }
        public double price_6 { get; set; }

        public IEnumerator GetEnumerator()
        {
            yield return code;
            yield return product;
            yield return quantity;
            yield return cost;
            yield return discount;
            yield return tax;
            yield return ipo;
            yield return shopping_id;
            yield return price_1;
            yield return price_2;
            yield return price_3;
            yield return price_4;
            yield return price_5;
            yield return price_6;
        }

    }

    public class Shopping_Object
    {
        public int number { get; set; }
        public int number_invoice { get; set; }
        public string terminal { get; set; }
        public int payment_form { get; set; }
        public int payment_method { get; set; }
        public int user_id { get; set; }
        public int supplier_id { get; set; }
    }
    
}
