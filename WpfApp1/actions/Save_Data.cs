using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Windows;

namespace WpfApp1.actions
{
    class Operations_Inventory
    {
        private conector c = new conector();

        public MySqlDataAdapter adaptador()
        {
            string query = "SELECT i.*, c.id FROM inventory AS i " +
                "INNER JOIN subcategory AS sb ON sb.id = i.subcategory_id " +
                "INNER JOIN category AS c ON c.id = sb.category_id";
            MySqlCommand cmd = new MySqlCommand(query, c.Conect());
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            c.Conect().Close();
            return adapter;
        }
    }
    
    class Save_Product
    {
        private conector c = new conector();


        //UPDATE PRODUCT
        #region
        public bool Update_Product(Product product)
        {
            bool result = false;
            string query = "";
            if(Query.type_company == 1)
            {
                query = "update inventory set code=@code, product = @product, cost = @cost, price_1 = @price_1, price_2 = @price_2," +
                    " price_3 = @price_3, price_4 = @price_4, price_5 = @price_5, price_6 = @price_6," +
                    " quantity = @quantity, tax = @tax, discount = @discount, ipo = @ipo, brand = @marca, stock = @stock, address = @ubicacion where code = "+product.code;
            }
            MySqlTransaction transaccion = c.Conect().BeginTransaction();
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, c.Conect(), transaccion);
                cmd.Parameters.AddWithValue("@code", product.code);
                cmd.Parameters.AddWithValue("@product", product.product);
                cmd.Parameters.AddWithValue("@cost", product.cost);
                cmd.Parameters.AddWithValue("@price_1", product.price_1);
                cmd.Parameters.AddWithValue("@price_2", product.price_2);
                cmd.Parameters.AddWithValue("@price_3", product.price_3);
                cmd.Parameters.AddWithValue("@price_4", product.price_4);
                cmd.Parameters.AddWithValue("@price_5", product.price_5);
                cmd.Parameters.AddWithValue("@price_6", product.price_6);
                cmd.Parameters.AddWithValue("@quantity", product.quantity);
                cmd.Parameters.AddWithValue("@tax", product.tax);
                cmd.Parameters.AddWithValue("@ipo", product.ipo);
                cmd.Parameters.AddWithValue("@discount", product.discount);
                cmd.Parameters.AddWithValue("@marca", product.brand);
                cmd.Parameters.AddWithValue("@stock", product.stock);
                cmd.Parameters.AddWithValue("@ubicacion", product.address);
                cmd.ExecuteNonQuery();
                transaccion.Commit();
                result = true;
            }
            catch (Exception ex)
            {
                transaccion.Rollback();
                MessageBox.Show($"Error {ex.Message}");
            }
            c.Conect().Close();
            return result;
        }
        #endregion

        //CREATE PRODUCT
        #region
        public bool Create_Product(Product product)
        {
            bool result = false;
            string query = "";
            if (Query.type_company == 1)
            {
                query = "insert into inventory(code,product,cost,price_1,price_2,price_3,price_4,price_5,price_6," +
                    "quantity,pack,display,quantity_static,pack_static,display_static,tax,ipo,discount,subcategory_id,brand,stock,address,quantity_inital)" +
                    "values(@code,@product,@cost,@price_1,@price_2,@price_3,@price_4,@price_5,@price_6," +
                    "@quantity,@pack,@display,@quantity_static,@pack_static,@display_static,@tax,@discount,@ipo,@subcategory,@marca,@stock,@ubicacion,@quantity_inital)";
            }
            else
            {
                query = "insert into inventory(code,product,cost,price_1,price_2,price_3,price_4,price_5,price_6," +
                    "quantity,pack,display,quantity_static,pack_static,display_static,tax,ipo,discount,subcategory_id)" +
                    "values(@code,@product,@cost,@price_1,@price_2,@price_3,@price_4,@price_5,@price_6," +
                    "@quantity,@pack,@display,@display_static,@tax,@discount,@ipo,@subcategory)";
            }

            MySqlTransaction transaccion = c.Conect().BeginTransaction();
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, c.Conect(), transaccion);
                cmd.Parameters.AddWithValue("@code", product.code);
                cmd.Parameters.AddWithValue("@product", product.product);
                cmd.Parameters.AddWithValue("@cost", product.cost);
                cmd.Parameters.AddWithValue("@price_1", product.price_1);
                cmd.Parameters.AddWithValue("@price_2", product.price_2);
                cmd.Parameters.AddWithValue("@price_3", product.price_3);
                cmd.Parameters.AddWithValue("@price_4", product.price_4);
                cmd.Parameters.AddWithValue("@price_5", product.price_5);
                cmd.Parameters.AddWithValue("@price_6", product.price_6);
                cmd.Parameters.AddWithValue("@quantity", product.quantity);
                cmd.Parameters.AddWithValue("@quantity_inital", product.quantity);
                cmd.Parameters.AddWithValue("@pack", product.pack);
                cmd.Parameters.AddWithValue("@display", product.display);
                cmd.Parameters.AddWithValue("@quantity_static", product.quantity_static);
                cmd.Parameters.AddWithValue("@pack_static", product.pack_static);
                cmd.Parameters.AddWithValue("@display_static", product.display_static);
                cmd.Parameters.AddWithValue("@tax", product.tax);
                cmd.Parameters.AddWithValue("@ipo", product.ipo);
                cmd.Parameters.AddWithValue("@discount", product.discount);
                cmd.Parameters.AddWithValue("@subcategory", product.subcategory);
                cmd.Parameters.AddWithValue("@marca", product.brand);
                cmd.Parameters.AddWithValue("@stock", product.stock);
                cmd.Parameters.AddWithValue("@ubicacion", product.address);

                cmd.ExecuteNonQuery();
                transaccion.Commit();
                result = true;
            }
            catch (Exception ex)
            {
                transaccion.Rollback();
                MessageBox.Show($"Error {ex.Message}");
            }
            c.Conect().Close();
            return result;

        }
        #endregion

        //DELETE PRODUCT
        #region

        public bool DeleteProduct(long id)
        {
            bool result = false;
            string query = "";
            query = "delete from inventory where code = " + id;
            MySqlTransaction transaccion = c.Conect().BeginTransaction();
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, c.Conect(), transaccion);
                cmd.ExecuteNonQuery();
                transaccion.Commit();
                result = true;
            }
            catch (Exception ex)
            {
                transaccion.Rollback();
                MessageBox.Show($"Error {ex.Message}");
            }
            c.Conect().Close();
            return result;
        }

        #endregion

        //CREATE CATEGORY
        #region

        public bool CreateCategory(string name)
        {
            bool result = false;
            string query = $"insert into category(name_category)values({name})";
            MySqlTransaction transaccion = c.Conect().BeginTransaction();
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, c.Conect(), transaccion);
                cmd.ExecuteNonQuery();
                result = true;
                transaccion.Commit();
            }
            catch (Exception e)
            {
                transaccion.Rollback();
                MessageBox.Show($"Error {e.Message}");
            }
            c.Conect().Close();
            return result;
        }
        #endregion

        //CREATE SUBCATEGORY
        #region
        public bool CreateSubcategory(int id,string name)
        {
            bool result = false;
            string query = $"insert into subcategory(category_id, name_subcategory)values({id},{name})";
            MySqlTransaction transaccion = c.Conect().BeginTransaction();
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, c.Conect(), transaccion);
                cmd.ExecuteNonQuery();
                result = true;
                transaccion.Commit();
            }
            catch (Exception e)
            {
                transaccion.Rollback();
                MessageBox.Show($"Error {e.Message}");
            }
            c.Conect().Close();
            return result;
        }


        #endregion

    }

    class Product: IEnumerable
    {
        public long code { get; set; }
        public string product { get; set; }
        public float cost { get; set; }
        public float price_1 { get; set; }
        public float price_2 { get; set; }
        public float price_3 { get; set; }
        public float price_4 { get; set; }
        public float price_5 { get; set; }
        public float price_6 { get; set; }
        public int quantity { get; set; }
        public int pack { get; set; }
        public int display { get; set; }
        public int quantity_static { get; set; }
        public int pack_static { get; set; }
        public int display_static { get; set; }
        public int tax { get; set; }
        public float ipo { get; set; }
        public int discount { get; set; }
        public int subcategory { get; set; }
        public string address { get; set; }
        public string brand { get; set; }
        public int stock { get; set; }
        public int tax_value { get; set; }
        
        public int subtotal { get; set; }

        public IEnumerator GetEnumerator()
        {
            yield return code;
            yield return product;
            yield return cost;
            yield return price_1;
            yield return price_2;
            yield return price_3;
            yield return price_4;
            yield return price_5;
            yield return price_6;
            yield return quantity;
            yield return pack;
            yield return display;
            yield return quantity_static;
            yield return pack_static;
            yield return display_static;
            yield return tax;
            yield return ipo;
            yield return discount;
            yield return subcategory;
            yield return stock;
            yield return address;
            yield return brand;
        }

    }





}
