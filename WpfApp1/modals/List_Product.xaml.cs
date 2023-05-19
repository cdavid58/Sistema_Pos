using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.actions;
using WpfApp1.designs;
using WpfApp1.forms.Modules;

namespace WpfApp1.modals
{
    /// <summary>
    /// Lógica de interacción para List_Product.xaml
    /// </summary>
    public partial class List_Product : Window
    {
        public List_Product()
        {
            InitializeComponent();
            GetListInventory();
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filterText = txtSearch.Text;
            ApplyFilter(filterText);
        }

        private void GetListInventory()
        {
            DataTable table = new DataTable();
            Operations_Inventory oy = new Operations_Inventory();
            oy.adaptador().Fill(table);
            MyDataGrid.ItemsSource = table.DefaultView;
        }

        private void ApplyFilter(string filterText)
        {
            DataTable originalTable = ((DataView)MyDataGrid.ItemsSource).Table;
            DataView filteredView = originalTable.DefaultView;
            filteredView.RowFilter = $"CONVERT(product, System.String) LIKE '%{filterText}%' OR CONVERT(code, System.String) LIKE '%{filterText}%'";
            MyDataGrid.ItemsSource = filteredView;
        }

        private void DataGridRow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DataGridRow row = sender as DataGridRow;
                DataRowView dataRowView = row.Item as DataRowView;
                var values = dataRowView.Row.ItemArray;
                var list_data = form_invoice.product_list;
                list_data.Add("code", values[1].ToString());
                list_data.Add("product", values[2].ToString());
                list_data.Add("price_1", values[4].ToString());
                list_data.Add("price_2", values[5].ToString());
                list_data.Add("price_3", values[6].ToString());
                list_data.Add("price_4", values[7].ToString());
                list_data.Add("price_5", values[8].ToString());
                list_data.Add("price_6", values[9].ToString());
                list_data.Add("quantity", values[10].ToString());
                list_data.Add("tax_value", values[16].ToString());
                list_data.Add("address", values[22].ToString());
                Hide();
                e.Handled = true;
            }
            if (e.Key == Key.Escape)
            {
                Hide();
                e.Handled = true;
            }
        }
    }
}
