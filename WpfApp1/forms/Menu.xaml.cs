using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using WpfApp1.actions;
using WpfApp1.forms.Modules;

namespace WpfApp1.forms
{
    /// <summary>
    /// Lógica de interacción para Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        private Dictionary<string, int> permissions;
        public static string chage_price = "";

        public Menu()
        {
            InitializeComponent();
            Closing += Window_Closing;
            permissions = login.list_permissions;
            Enabled_Modules();
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            user_name.Content = Query.user_name;
            //COLOR
            Constantes c = new Constantes();
            c.GetColors();
            var color = (Color)ColorConverter.ConvertFromString(Constantes.Colors["background_menu"]);
            var brush = new SolidColorBrush(color);
            Background = brush;

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            txtReloj.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }

        private void Enabled_Modules()
        {
            
            foreach (KeyValuePair<string, int> item in permissions)
            {
                try
                {
                    var element = FindName(item.Key.ToString()) as UIElement;
                    var label = FindName(item.Key.ToString() + "_Label") as UIElement;
                    element.Visibility = Visibility.Visible;
                    label.Visibility = Visibility.Visible;
                }catch(Exception e)
                {
                    chage_price = item.Key.ToString();
                }
            }
        }

        private void Select_Module(int id)
        {
            switch(id){
                case 1:
                    Facturacion f = new Facturacion();
                    f.Show();
                    Hide();
                    break;
                case 2:
                    Compras shop = new Compras();
                    shop.Show();
                    Hide();
                    break;
                case 3:
                    Inventario i = new Inventario();
                    i.Show();
                    Hide();
                    break;
                case 4:
                    Informes inf = new Informes();
                    inf.Show();
                    Hide();
                    break;
                case 5:
                    Clientes client = new Clientes();
                    client.Show();
                    Hide();
                    break;
                case 6:
                    Proveedores provee = new Proveedores();
                    provee.Show();
                    Hide();
                    break;
                case 7:
                    Usuarios users = new Usuarios();
                    users.Show();
                    Hide();
                    break;
            }
        }

        private void Menu_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.F1)
            {
                Facturacion f = new Facturacion();
                f.Show();
                Hide();
            }
            else if(e.Key == Key.F2)
            {
                Compras shop = new Compras();
                shop.Show();
                Hide();
            }
            else if (e.Key == Key.F3)
            {
                Inventario i = new Inventario();
                i.Show();
                Hide();
            }
            else if (e.Key == Key.F4)
            {
                Informes inf = new Informes();
                inf.Show();
                Hide();
            }
            else if (e.Key == Key.F5)
            {
                Clientes client = new Clientes();
                client.Show();
                Hide();
            }
            else if (e.Key == Key.F6)
            {
                Proveedores provee = new Proveedores();
                provee.Show();
                Hide();
            }
            else if (e.Key == Key.F7)
            {
                Usuarios users = new Usuarios();
                users.Show();
                Hide();
            }
            else if(e.Key == Key.E)
            {
                login Login = new login();
                Login.Show();
                Hide();
            }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Facturacion_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (sender is Image clickedImage)
                {
                    var id_module = Convert.ToInt32(clickedImage.Tag);
                    Select_Module(id_module);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }
    }
}
