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
            }
        }

        private void TxtDaysExpire_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                txtDate_Expires.Content = date.AddDays(int.Parse(txtDaysExpire.Text)).ToString();
            }
            catch(Exception ex) { Console.WriteLine(ex.Message); }
        }
    }






}
