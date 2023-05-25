using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.designs
{
    public partial class data_invoice : Window
    {
        private DateTime date = DateTime.Now;

        public data_invoice()
        {
            InitializeComponent();
            cbFPago.Focus();
            txtSeller.Text = Query.user_name;
            txtDate_Expires.Content = date.ToString();
        }

        private void CbFPago_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbFPago.SelectedValue != null)
            {
                ComboBoxItem itemSeleccionado = (ComboBoxItem)cbFPago.SelectedItem;
                string valorSeleccionado = itemSeleccionado.Content.ToString();
                if(valorSeleccionado == "Crédito")
                {
                    txtDaysExpire.IsEnabled = true;
                    txtDaysExpire.Text = "";
                    txtDaysExpire.Focus();
                }
                else
                {
                    txtDaysExpire.IsEnabled = false;
                    txtDaysExpire.Text = "0";
                }
                int vs = 10;
                switch (valorSeleccionado)
                {
                    case "Crédito":
                        vs = 30;
                        break;
                    case "Transferecia":
                        vs = 31;
                        break;
                }
                form_invoice.payment_method = vs;
            }
        }

        private void TxtDaysExpire_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                txtDate_Expires.Content = date.AddDays(int.Parse(txtDaysExpire.Text)).ToString();
                form_invoice.DaysExpire = int.Parse(txtDaysExpire.Text);

            }
            catch(Exception ex) { Console.WriteLine(ex.Message); }
        }

        private void BtnOrder_Click(object sender, RoutedEventArgs e)
        {
            form_invoice.notes = txtNotes.Text;
            form_invoice.discount_to_invoice = double.Parse(txtDiscount.Text);
            Close();
        }
    }






}
