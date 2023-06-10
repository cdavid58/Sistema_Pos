using System;
using System.Windows;
using System.Windows.Input;
using WpfApp1.modals;

namespace WpfApp1.forms.Modules
{
    public partial class CXP : Window
    {
        public CXP()
        {
            InitializeComponent();
        }

        private void TxtCustomer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                List_Client lc = new List_Client();
                lc.ShowDialog();
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
