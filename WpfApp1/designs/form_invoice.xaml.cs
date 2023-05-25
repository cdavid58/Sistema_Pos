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
        private List<Invoice> data = new List<Invoice>();
        public static Dictionary<string, string> client_list = new Dictionary<string, string>();
        public static Dictionary<string, string> product_list = new Dictionary<string, string>();
        public static string notes = "";
        public static int DaysExpire = 0, payment_method = 0;
        public static double discount_to_invoice = 0;

        private double total_base = 0;

        private int count_limit = 0;

        public form_invoice()
        {
            InitializeComponent();
            txtCode.Focus();
            invoice i = new invoice();
            txtNumberInvoice.Text = i.GetConsecutive().ToString();
        }
        
        public int Payment_Form()
        {
            int value = 1;
            switch (payment_method)
            {
                case 10:
                    value = 1;
                    break;
                case 30:
                    value = 2;
                    break;
                case 31:
                    value = 1;
                    break;
            }
            return value;
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
                        product_list.Clear();
                    }
                    else
                    {
                        MessageBox.Show("El producto no existe", "ALERTA", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
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
            else if (e.Key == Key.C)
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
                if (dgInvoice.Items.Count > 0)
                {
                    data_invoice di = new data_invoice();
                    di.ShowDialog();
                    e.Handled = true;
                    HeaderInvoice hi = new HeaderInvoice
                    {
                        number = int.Parse(txtNumberInvoice.Text),
                        user = Query.pk_user,
                        terminal = Environment.UserName,
                        payment_form = Payment_Form(),
                        payment_method = payment_method,
                        notes = string.IsNullOrEmpty(notes) ? "No hay" : notes,
                        client_id = int.Parse(client_list["pk_client"])
                    };
                    invoice i = new invoice();
                    i.CreateInvoice(hi);

                    if (dgInvoice.ItemsSource is IEnumerable<Invoice> data)
                    {
                        foreach (var item in data)
                        {
                            DetailsInvoice details_invoice = new DetailsInvoice
                            {
                                code = long.Parse(item.code),
                                product = item.product.ToString(),
                                quantity = int.Parse(item.quantity.ToString()),
                                price = double.Parse(item.cost.ToString()),
                                discount = int.Parse(item.discount.ToString()),
                                tax = int.Parse(item.tax.ToString()),
                                ipo = double.Parse(item.ipo.ToString()),
                                invoice_id = int.Parse(txtNumberInvoice.Text)
                            };
                            i.CreateDetailsInvoice(details_invoice);
                            GarbageCollector(details_invoice);
                        }
                    }
                    GarbageCollector(di);
                    GarbageCollector(hi);
                    GarbageCollector(i);
                    Clean();
                    MessageBox.Show("Factura creada con éxito", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    ticket_pos tp = new ticket_pos();
                    tp.Show();
                }
                else
                {
                    MessageBox.Show("Debe tener productos en la lista", "ALERTA", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void CalculateTotal()
        {
            int total = 0, subtotal = 0, tax = 0, discount = 0;
            if (dgInvoice.ItemsSource is IEnumerable<Invoice> data)
            {
                foreach (var item in data)
                {

                    discount += item.cost * (item.discount / 100) * item.quantity;
                    total += item.subtotal + item.tax_value;
                    subtotal += item.subtotal;
                    tax += (item.tax_value * item.quantity);
                    
                }
            }
            txtTotal.Text = total.ToString();
            txtTaxes.Text = tax.ToString();
            txtSubtotal.Text = subtotal.ToString();
            txtDiscount.Text = discount.ToString();
        }
        

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
            if (exist)
            {
                dgInvoice.ItemsSource = null;
                dgInvoice.ItemsSource = data;
            }
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
                            subtotal = invoice.subtotal,
                            
                        });
                        txtItems.Text = dgInvoice.Items.Count.ToString();
                        dgInvoice.ItemsSource = null;
                        dgInvoice.ItemsSource = data;
                    }

                    txtTotalBase.Text = Math.Round(total_base).ToString();
                    int limit = int.Parse(txtTotalBase.Text);
                    if(limit > 212 && count_limit == 0)
                    {
                        MessageBox.Show("La factura sera enviada a electrónica, ya que ha superado el limite en la base", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        count_limit = 1;
                    }

                    txtCode.Clear();
                    txtQuantity.Clear();
                    txtCode.Focus();
                    CalculateTotal();
                    GarbageCollector(invoice);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show("No puede facturar en ceros", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            } 
        }

        private void BtnUltCopy_Click(object sender, RoutedEventArgs e)
        {
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

        private void GarbageCollector(object obj)
        {
            obj = null;
        }

        private void TxtTypePrice_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                invoice i = new invoice();
                var invoice = i.GetQueryInvoice(int.Parse(txtQuantity.Text), long.Parse(txtCode.Text), int.Parse(txtTypePrice.Text));
                total_base += invoice.cost;
                if (!ProductExist(int.Parse(txtQuantity.Text), invoice.code))
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
                        subtotal = invoice.subtotal,

                    });
                    txtItems.Text = dgInvoice.Items.Count.ToString();
                    dgInvoice.ItemsSource = data;
                }
                txtTotalBase.Text = Math.Round(total_base).ToString();
                int limit = int.Parse(txtTotalBase.Text);
                if (limit > 212 && count_limit == 0)
                {
                    MessageBox.Show("La factura sera enviada a electrónica, ya que ha superado el limite en la base", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    count_limit = 1;
                }

                txtCode.Clear();
                txtQuantity.Clear();
                txtCode.Focus();
                CalculateTotal();
                GarbageCollector(invoice);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("No puede facturar en ceros", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void TxtQuantity_GotFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCode.Text))
            {
                txtCode.Focus();
            }
        }

        private void TxtTypePrice_GotFocus(object sender, RoutedEventArgs e)
        {
            txtTypePrice.Clear();
        }

        private void Clean()
        {
            txtItems.Text = "0";
            txtExist.Text = "0";
            txtCode.Text = "";
            txtQuantity.Text = "";
            txtProduct.Text = "No hay producto seleccionado";
            price_1.Text = "0";
            price_2.Text = "0";
            price_3.Text = "0";
            price_4.Text = "0";
            price_5.Text = "0";
            price_6.Text = "0";
            lbClient.Content = "Consumidor Final";
            txtSubtotal.Text = "0";
            txtTaxes.Text = "0";
            txtDiscount.Text = "0";
            txtTotal.Text = "0";
            txtTotalBase.Text = "0";
            txtCode.Focus();
            data_invoice di = new data_invoice();
            di.txtNotes.Text = "";
            di.txtDaysExpire.Text = "0";
            di.cbFPago.SelectedIndex = 0;
            dgInvoice.ItemsSource = null;
            data.Clear();
            invoice i = new invoice();
            txtNumberInvoice.Text = i.GetConsecutive().ToString();
            GarbageCollector(di);
            GarbageCollector(i);
        }
        
    }
}
