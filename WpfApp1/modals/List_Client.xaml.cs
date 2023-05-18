using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp1.actions;
using WpfApp1.designs;

namespace WpfApp1.modals
{
    public partial class List_Client : Window
    {
        public List_Client()
        {
            InitializeComponent();
            txtSearch.Focus();
            GetListClient();
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

        private void GetListClient()
        {
            DataTable table = new DataTable();
            Operations_Inventory oy = new Operations_Inventory();
            Query_Client qc = new Query_Client();
            qc.GetListClient().Fill(table);
            MyDataGrid.ItemsSource = table.DefaultView;
        }

        private void ApplyFilter(string filterText)
        {
            DataTable originalTable = ((DataView)MyDataGrid.ItemsSource).Table;
            DataView filteredView = originalTable.DefaultView;
            filteredView.RowFilter = $"CONVERT(name, System.String) LIKE '%{filterText}%' OR CONVERT(documentI, System.String) LIKE '%{filterText}%'";
            MyDataGrid.ItemsSource = filteredView;
        }

        private void DataGridRow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DataGridRow row = sender as DataGridRow;
                DataRowView dataRowView = row.Item as DataRowView;
                var values = dataRowView.Row.ItemArray;
                var data = form_invoice.client_list;
                data.Add("documentI", values[1].ToString());
                data.Add("name", values[2].ToString());
                data.Add("address", values[3].ToString());
                data.Add("phone", values[5].ToString());
                data.Add("type_client", values[6].ToString());
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
