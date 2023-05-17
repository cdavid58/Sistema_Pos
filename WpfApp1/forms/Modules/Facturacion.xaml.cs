using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfApp1.forms.Modules
{
    /// <summary>
    /// Lógica de interacción para Facturacion.xaml
    /// </summary>
    public partial class Facturacion : Window
    {
        public Facturacion()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new System.Action(() =>
            {
                CreateNewTab();
            }));
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControl.SelectedItem == null) return;

            TabItem selectedTab = (TabItem)tabControl.SelectedItem;

            if (selectedTab.Header.ToString() == "Nueva Pestaña")
            {
                CreateNewTab();
            }
        }

        private void CreateNewTab()
        {
            TabItem newTab = new TabItem();
            newTab.Header = "Nueva Pestaña";
            WebBrowser newWebBrowser = new WebBrowser();
            newTab.Content = newWebBrowser;
            tabControl.Items.Insert(tabControl.Items.Count - 1, newTab);
            tabControl.SelectedItem = newTab;
        }
    }
}
