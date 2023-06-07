using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfApp1.actions;
using WpfApp1.designs;
using WpfApp1.modals;

namespace WpfApp1.forms
{
    public partial class shopping : Window
    {
        private shoppings Shopping = new shoppings();
        private List<Invoice> data = new List<Invoice>();

        public static Dictionary<string, string> Data_Supplier = new Dictionary<string, string>();
        private int fpago = 1, pmtehod = 10;
        public static string notes = "";
        public static double discount_to_invoice = 0, total_base = 0;

        public shopping()
        {
            InitializeComponent();
            txtNDocument.Text = Shopping.getconsecutive(6);
            txtDateToday.Text = DateTime.Now.ToString("yyyy/MM/dd");
            txtSupplier.Focus();
            block(false);
        }

        public void block(bool value)
        {
            txtCodeProduct.IsEnabled = value;
            txtQuanty.IsEnabled = value;
            txtBase.IsEnabled = value;
            txtIPOC.IsEnabled = value;
            txtTax.IsEnabled = value;
            price_1.IsEnabled = value;
            price_2.IsEnabled = value;
            price_3.IsEnabled = value;
            price_4.IsEnabled = value;
            price_5.IsEnabled = value;
            price_6.IsEnabled = value;
            dgInvoice.IsEnabled = value;
            cmbFP.IsEnabled = value;

        }
        
        private bool ProductExist(int quantity, string code, int price)
        {
            bool exist = false;
            if (dgInvoice.ItemsSource is IEnumerable<Invoice> _data)
            {
                foreach (var item in _data)
                {
                    if (item.code == code && item.cost == price)
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

        private void TxtInvoice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!IsNumeric(txtInvoice.Text))
                {
                    MessageBox.Show("Ingrese solo números en el campo de factura.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtInvoice.Clear();
                    txtInvoice.Focus();
                }
                else if (Shopping.QueryNumberInvoice(long.Parse(txtInvoice.Text)))
                {
                    MessageBox.Show($"Esta factura ya fue ingresada, con el proveedor {txtNameSupplier.Text}.", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtInvoice.Clear();
                    txtInvoice.Focus();
                }
                else
                {
                    block(true);
                    txtCodeProduct.Focus();
                }
                e.Handled = true;
            }
        }

        private bool IsNumeric(string text)
        {
            return long.TryParse(text, out _);
        }

        private void TxtSupplier_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                List_Supplier ls = new List_Supplier();
                ls.ShowDialog();
                txtNameSupplier.Text = Data_Supplier["name"];
                txtAddress.Text = Data_Supplier["address"];
                txtPhoneSupplier.Text = Data_Supplier["phone"];
                e.Handled = true;
                txtInvoice.IsEnabled = true;
                txtInvoice.Focus();

            }
        }

        private void TxtCodeProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if (string.IsNullOrEmpty(txtCodeProduct.Text))
                {
                    List_Product lp = new List_Product();
                    lp.ShowDialog();
                    var data = form_invoice.product_list;
                    txtCodeProduct.Text = data["code"];
                    txtProduct.Text = data["product"];
                    txtBase.Text = data["cost"];
                    txtIPOC.Text = data["ipo"];
                    txtTax.Text = data["tax"];
                    price_1.Text = data["price_1"];
                    price_2.Text = data["price_2"];
                    price_3.Text = data["price_3"];
                    price_4.Text = data["price_4"];
                    price_5.Text = data["price_5"];
                    price_6.Text = data["price_6"];
                    txtExist.Text = data["quantity"];
                    txtQuanty.Focus();
                    data.Clear();
                }
                else
                {

                    if (!txtCodeProduct.Text.Contains("-"))
                    {
                        invoice i = new invoice();
                        var product = i.GetProduct(long.Parse(txtCodeProduct.Text));
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
                            txtQuanty.Focus();
                        }
                        else
                        {
                            MessageBox.Show("El producto que busca no existe", "ALERTA", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        DeleteRowProductCode(txtCodeProduct.Text.Replace("-", ""));
                    }
                }
            }
            else if(e.Key == Key.G)
            {
                Save_Shopping();
                e.Handled = true;
            }


        }

        private void DeleteRowProductCode(string code)
        {
            Invoice invoice = data.FirstOrDefault(data => data.code == code);

            if (invoice != null)
            {
                data.Remove(invoice);
                dgInvoice.Items.Refresh();
                txtCodeProduct.Clear();
                txtQuanty.Clear();
                txtCodeProduct.Focus();
                //CalculateTotal();
                invoice i = new invoice();
                i.ReturnProduct(code);
            }
        }

        private void CalculateTotal()
        {
            int total = 0, subtotal = 0, tax = 0, discount = 0, ipoc = 0;
            if (dgInvoice.ItemsSource is IEnumerable<Invoice> data)
            {
                foreach (var item in data)
                {

                    discount += item.cost * (item.discount / 100) * item.quantity;
                    total += item.subtotal + item.tax_value;
                    subtotal += item.subtotal;
                    tax += (item.tax_value * item.quantity);
                    ipoc += item.ipo * item.quantity;
                }
            }
            txtTotal.Text = total.ToString();
            txtTaxes.Text = tax.ToString();
            txtSubtotal.Text = subtotal.ToString();
            txtDcto.Text = discount.ToString();
            txtIPO.Text = ipoc.ToString();
        }

        private void AddProductToTable()
        {
            invoice i = new invoice();
            var invoice = i.GetQueryInvoice(int.Parse(txtQuanty.Text), long.Parse(txtCodeProduct.Text),1,6);
            total_base += invoice.cost * int.Parse(txtQuanty.Text);
            if (!ProductExist(int.Parse(txtQuanty.Text), invoice.code, invoice.cost))
            {
                data.Add(new Invoice()
                {
                    code = txtCodeProduct.Text,
                    product = txtProduct.Text,
                    cost = int.Parse(txtBase.Text),
                    quantity = int.Parse(txtQuanty.Text),
                    tax_value = int.Parse(txtTax.Text),
                    discount = invoice.discount,
                    ipo = int.Parse(txtIPO.Text),
                    subtotal = int.Parse(txtBase.Text) * int.Parse(txtQuanty.Text)
                });
                txtItems.Text = dgInvoice.Items.Count.ToString();
                dgInvoice.ItemsSource = null;
                dgInvoice.ItemsSource = data;
                dgInvoice.Items.Refresh();
            }
            txtCodeProduct.Clear();
            txtQuanty.Clear();
            txtCodeProduct.Focus();
            CalculateTotal();
            GarbageCollector(invoice);
        }

        private void GarbageCollector(object obj)
        {
            obj = null;
        }

        private void TxtTax_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                AddProductToTable();
            }
        }

        private void GetShopping_MouseDown(object sender, MouseButtonEventArgs e)
        {
            recover(15);
        }

        private void CancelShopping_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Cancelar compra", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Exit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            Hide();
        }

        private void New_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NewShopping();
        }
        
        private void Save_Shopping()
        {
            Shopping_Object so = new Shopping_Object {
                number = int.Parse(txtNDocument.Text),
                number_invoice = int.Parse(txtInvoice.Text),
                terminal = Environment.UserName,
                payment_form = fpago,
                payment_method = pmtehod,
                user_id = Query.pk_user,
                supplier_id = int.Parse(Data_Supplier["supplier_id"])
            };
            shoppings sho = new shoppings();
            sho.Create_Shopping(so);
            bool result = false;

            if (dgInvoice.ItemsSource is IEnumerable<Invoice> _data)
            {
                foreach (var item in _data)
                {
                    Details_Shopping ds = new Details_Shopping {
                        code = item.code,
                        product = item.product,
                        quantity = item.quantity,
                        cost = item.cost,
                        discount = item.discount,
                        tax = item.tax,
                        ipo = item.ipo,
                        shopping_id = shoppings.shopping_id,
                        price_1 = double.Parse(price_1.Text),
                        price_2 = double.Parse(price_2.Text),
                        price_3 = double.Parse(price_3.Text),
                        price_4 = double.Parse(price_4.Text),
                        price_5 = double.Parse(price_5.Text),
                        price_6 = double.Parse(price_6.Text)
                    };
                    result = sho.Create_DetailsShopping(ds);
                }
            }
            if(fpago == 2)
            {
                sho.Create_Wallet_Shopping(int.Parse(txtTotal.Text), int.Parse(txtDaysExpired.Text));
            }
            if (result)
            {
                NewShopping();
                MessageBox.Show("Compra Creada con exito","Existo",MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Hubo un error en la compra", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            



        }

        private void recover(int number_invoice)
        {
            GetDocument gd = new GetDocument();
            var data_shopping = gd.GetShopping(number_invoice)[0];
            var data_details_shoppin = gd.GetDetailsShopping();
            var data_supplier = gd.GetSupplier();
            txtNDocument.Text = data_shopping.number.ToString();
            txtInvoice.Text = data_shopping.number_invoice.ToString();
            dgInvoice.ItemsSource = null;
            dgInvoice.ItemsSource = data_details_shoppin;
            dgInvoice.Items.Refresh();
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i);
                    if (child is T typedChild)
                    {
                        yield return typedChild;
                    }

                    foreach (var nestedChild in FindVisualChildren<T>(child))
                    {
                        yield return nestedChild;
                    }
                }
            }
        }

        private void CmbFP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFP.SelectedItem is ComboBoxItem selectedItem)
            {
                if (selectedItem.Content.ToString() == "Crédito")
                {
                    txtDaysExpired.Clear();
                    txtDaysExpired.IsEnabled = true;
                    txtDaysExpired.Focus();
                }
                else
                {
                    txtDaysExpired.IsEnabled = false;
                    txtDaysExpired.Text = "0";
                }
                switch (selectedItem.Content.ToString())
                {
                    case "Efectivo":
                        fpago = 1;
                        pmtehod = 10;
                        break;
                    case "Crédito":
                        fpago = 2;
                        pmtehod = 30;
                        break;
                    case "Transferencia":
                        fpago = 1;
                        pmtehod = 10;
                        break;
                }
            }
        }

        private void NewShopping()
        {
            foreach (var control in FindVisualChildren<TextBox>(this))
            {
                control.Clear();
            }
            txtExist.Text = "0";
            txtNDocument.Text = Shopping.getconsecutive(6);
            price_1.Text = "0";
            price_2.Text = "0";
            price_3.Text = "0";
            price_4.Text = "0";
            price_5.Text = "0";
            price_6.Text = "0";
            txtIPOC.Text = "0";
            txtSubtotal.Text = "0";
            txtDcto.Text = "0";
            txtTaxes.Text = "0";
            txtIPOC.Text = "0";
            txtTotal.Text = "0";
            txtPercentage.Text = "0";
            txtDateToday.Text = DateTime.Now.ToString("yyyy/MM/dd");
            dgInvoice.ItemsSource = null;
            dgInvoice.Items.Refresh();
            data.Clear();
            block(false);
            txtSupplier.Focus();
        }
    
    }
}
