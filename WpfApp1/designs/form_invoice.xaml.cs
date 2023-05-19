using System;
using System.Collections.Generic;
using System.Linq;
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
            invoice i = new invoice();
            txtNumberInvoice.Text = i.GetConsecutive().ToString();
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
                        foreach (var i in data)
                        {
                            Console.WriteLine(i);
                        }
                        txtCode.Text = data["code"].ToString();
                        txtExist.Text = data["quantity"].ToString();
                        txtProduct.Text = data["product"].ToString();
                        txtLocalitation.Text = data["address"].ToString();
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
            else if(e.Key == Key.O)
            {
                txtObservation.Focus();
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

        private int GetPrice(int id)
        {
            int[] myNum = { 1, 2, 3, 4, 5, 6 };
            return myNum.FirstOrDefault(n => n == id);
        }

        private void TxtQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int type_price = 2;
                try {
                    if(txtTypeClient.Text != null)
                    {
                        type_price = int.Parse(txtTypeClient.Text);
                    }
                }
                catch (Exception){}
                List<Invoice> data = new List<Invoice>();
                invoice i = new invoice();
                var invoice = i.GetQueryInvoice(int.Parse(txtQuantity.Text), long.Parse(txtCode.Text), type_price);
                data.Add(new Invoice()
                {
                    code = invoice.code.ToString(),
                    product = invoice.product,
                    cost = invoice.cost,
                    quantity = int.Parse(txtQuantity.Text),
                    tax_value = invoice.tax_value,
                    discount = invoice.discount,
                    ipo = invoice.ipo,
                    subtotal = invoice.subtotal
                });
                dgInvoice.ItemsSource = data;
                txtItems.Text = dgInvoice.Items.Count.ToString();
            }
        }
    }
}
