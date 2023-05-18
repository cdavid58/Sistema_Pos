using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp1.actions;
using WpfApp1.forms.Modules;
using WpfApp1.modals;

namespace WpfApp1.designs
{

    public partial class form_invoice : Page
    {
        public static Dictionary<string, string> client_list = new Dictionary<string, string>();
        public static Dictionary<string, string> product_list = new Dictionary<string, string>();

        public form_invoice()
        {
            InitializeComponent();
            txtCode.Focus();
            List<Product> data = new List<Product>();
            data.Add(new Product(){ code = 1, product = "Producto 1",cost = 10, quantity = 10, tax = 19, discount = 500, ipo = 500, subtotal = 100 });
            dgInvoice.ItemsSource = data;
            invoice i = new invoice();
            i.GetQueryInvoice(1, 123456789123456, 1);
            if(Facturacion.Change_Price == "Change_Price")
            {
                txtTypePrice.Visibility = Visibility.Visible;
            }
        }



        private void TxtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (string.IsNullOrEmpty(txtCode.Text)) {
                    List_Product lp = new List_Product();
                    lp.ShowDialog();
                    e.Handled = true;
                    var data = product_list;
                    if (product_list.Count > 0)
                    {
                        txtCode.Text = data["code"].ToString();
                        txtExist.Text = data["quantity"].ToString();
                        txtProduct.Text = data["product"].ToString();
                        txtAddress.Text = data["address"].ToString();
                        price_1.Text = data["price_1"].ToString();
                        price_2.Text = data["price_2"].ToString();
                        price_3.Text = data["price_3"].ToString();
                        price_4.Text = data["price_4"].ToString();
                        price_5.Text = data["price_5"].ToString();
                        price_6.Text = data["price_6"].ToString();
                        txtQuantity.Focus();
                    }
                    else
                    {
                        MessageBox.Show("El producto no existe", "ALERTA", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                } else
                {
                    invoice i = new invoice();
                    var product = i.GetProduct(long.Parse(txtCode.Text));
                    if (product != null)
                    {
                        txtExist.Text = product.quantity.ToString();
                        price_1.Text = product.price_1.ToString();
                        price_2.Text = product.price_2.ToString();
                        price_3.Text = product.price_3.ToString();
                        price_4.Text = product.price_4.ToString();
                        price_5.Text = product.price_5.ToString();
                        price_6.Text = product.price_6.ToString();
                        txtProduct.Text = product.product;
                        txtQuantity.Focus();
                    }
                    else
                    {
                        MessageBox.Show("El producto que busca no existe", "ALERTA", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else if(e.Key == Key.C)
            {
                txtClient.Focus();
                e.Handled = true;
            }
        }

        private void form_invoice_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void TxtClient_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if (string.IsNullOrEmpty(txtClient.Text))
                {
                    List_Client list_client = new List_Client();
                    list_client.ShowDialog();
                    e.Handled = true;
                    if (client_list.Count > 0)
                    {
                        txtClient.Text = client_list["documentI"].ToString();
                        txtName.Text = client_list["name"].ToString();
                        txtAddress.Text = client_list["address"].ToString();
                        txtPhone.Text = client_list["phone"].ToString();
                        txtTypeClient.Text = client_list["type_client"].ToString();
                        txtCode.Focus();
                    }
                }
                else
                {
                    invoice i = new invoice();
                    var data_client = i.GetClient(txtClient.Text);
                    txtName.Text = data_client.name;
                    txtAddress.Text = data_client.address;
                    txtPhone.Text = data_client.phone;
                    txtTypeClient.Text = data_client.type_client.ToString();
                    if (i.exists_client)
                    {
                        txtCode.Focus();
                    }else{
                        txtClient.Text = "";
                    }
                }
            }
            else if(e.Key == Key.C)
            {
                txtCode.Focus();
                e.Handled = true;
            }
            
        }
    }
}
