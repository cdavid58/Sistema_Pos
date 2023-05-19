using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp1.forms;

namespace WpfApp1
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Query query = new Query();
        public MainWindow()
        {
            InitializeComponent();
            //Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(async () =>
            {
                await Program.RunOpenAI();
            });
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            if (!query.Query_Block_Company(900541566))
            {
                login Login = new login();
                Login.Show();
                Hide();
            }
            else
            {
                MessageBox.Show("Por favor comunicarse con su proveedor de servicio");
            }
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (e.Key == Key.Enter)
                {
                    if (!query.Query_Block_Company(900541566))
                    {
                        login Login = new login();
                        Login.Show();
                        Hide();
                    }
                    else
                    {
                        MessageBox.Show("Por favor comunicarse con su proveedor de servicio");
                    }
                }
            }
        }
    }
}
