using System.Windows;
using System.Windows.Input;
using WpfApp1.forms;
using System.Diagnostics;
using System;

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
            try
            {
                string laragonPath = @"C:/laragon/";
                string mysqlCommand = $"/{laragonPath}bin/mysql/mysql-5.7.24-winx64/bin/mysqld.exe/ --defaults-file=/{laragonPath}bin/mysql/mysql-5.7.24-winx64/bin/my.ini/ --standalone";
                ExecuteCommand(mysqlCommand);
            }
            catch (Exception) { }
        }

        static void ExecuteCommand(string command)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;

            Process process = new Process();
            process.StartInfo = processInfo;
            process.Start();
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
