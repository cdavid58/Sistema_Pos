using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp1.actions;
using WpfApp1.modals;

namespace WpfApp1.designs
{

    public partial class form_invoice : Page
    {
        public static Dictionary<string, string> client_list = new Dictionary<string, string>();
        public static Dictionary<string, string> product_list = new Dictionary<string, string>();
        private double total_base = 0;

        private int count_limit = 0;

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
                        txtCode.Text = data["code"];
                        txtExist.Text = data["quantity"];
                        txtProduct.Text = data["product"];
                        price_1.Text = data["price_1"];
                        price_2.Text = data["price_2"];
                        price_3.Text = data["price_3"];
                        price_4.Text = data["price_4"];
                        price_5.Text = data["price_5"];
                        price_6.Text = data["price_6"];
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
            }else if (e.Key == Key.C)
            {
                try
                {
                    List_Client lc = new List_Client();
                    lc.ShowDialog();
                    var data = client_list;
                    lbClient.Content = data["name"].ToString();
                    txtCode.Focus();
                    e.Handled = true;
                }
                catch (Exception) { }
            }
            else if (e.Key == Key.F)
            {
                data_invoice di = new data_invoice();
                di.ShowDialog();
                e.Handled = true;
            }
        }

        private void CalculateTotal()
        {
            int total = 0, subtotal = 0, tax = 0;
            if (dgInvoice.ItemsSource is IEnumerable<Invoice> data)
            {
                foreach (var item in data)
                {
                    total += item.subtotal + item.tax_value;
                    subtotal += item.subtotal;
                    tax += (item.tax_value * item.quantity);
                }
            }
            txtTotal.Text = total.ToString();
            txtTaxes.Text = tax.ToString();
            txtSubtotal.Text = subtotal.ToString();
        }
        private List<Invoice> data = new List<Invoice>();

        private bool ProductExist(int quantity, string code)
        {
            bool exist = false;
            if (dgInvoice.ItemsSource is IEnumerable<Invoice> _data)
            {
                foreach (var item in _data)
                {
                    if (item.code == code)
                    {
                        item.quantity += quantity;
                        item.subtotal += (quantity * item.cost);
                        exist = true;
                        break;
                    }
                }
            }
            
            dgInvoice.ItemsSource = null;
            dgInvoice.ItemsSource = data;
            return exist;
        }

        

        private void TxtQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    invoice i = new invoice();
                    var invoice = i.GetQueryInvoice(int.Parse(txtQuantity.Text), long.Parse(txtCode.Text), int.Parse(txtTypePrice.Text));
                    total_base += invoice.cost;
                    if (!ProductExist(int.Parse(txtQuantity.Text),invoice.code))
                    {
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
                    }
                    txtItems.Text = dgInvoice.Items.Count.ToString();
                    txtTotalBase.Text = Math.Round(total_base).ToString();
                    int limit = int.Parse(txtTotalBase.Text);
                    if(limit > 212 && count_limit == 0)
                    {
                        MessageBox.Show("La factura sera enviada a electrónica, ya que ha superado el limite en la base", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        count_limit = 1;
                    }

                    
                    CalculateTotal();
                }
                catch (Exception)
                {
                    MessageBox.Show("No puede facturar en ceros", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            } 
        }

        private void BtnClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List_Client lc = new List_Client();
                lc.ShowDialog();
                var data = client_list;
                lbClient.Content = data["name"].ToString();
                txtCode.Focus();
            }
            catch (Exception) { }
        }


        
    }
}
