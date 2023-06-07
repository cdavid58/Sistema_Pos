using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp1.actions;
using WpfApp1.forms;

namespace WpfApp1.modals
{
    public partial class List_Supplier : Window
    {
        public List_Supplier()
        {
            InitializeComponent();
            GetListClient();
        }

        private void GetListClient()
        {
            DataTable table = new DataTable();
            shoppings Shopping = new shoppings();
            Shopping.GetSupplier().Fill(table);
            MyDataGrid.ItemsSource = table.DefaultView;
        }

        private void TxtSearch_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }

        private void ApplyFilter(string filterText)
        {
            DataTable originalTable = ((DataView)MyDataGrid.ItemsSource).Table;
            DataView filteredView = originalTable.DefaultView;
            filteredView.RowFilter = $"CONVERT(name, System.String) LIKE '%{filterText}%'";
            MyDataGrid.ItemsSource = filteredView;
        }



        private void DataGridRow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DataGridRow row = sender as DataGridRow;
                DataRowView dataRowView = row.Item as DataRowView;
                var values = dataRowView.Row.ItemArray;
                shopping.Data_Supplier.Clear();
                shopping.Data_Supplier.Add("supplier_id", values[0].ToString());
                shopping.Data_Supplier.Add("name",values[1].ToString());
                shopping.Data_Supplier.Add("address", values[2].ToString());
                shopping.Data_Supplier.Add("phone", values[3].ToString());
                Hide();
                e.Handled = true;
            }
        }

        private void TxtSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ApplyFilter(txtSearch.Text);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtSearch.Focus();
        }
    }
}
