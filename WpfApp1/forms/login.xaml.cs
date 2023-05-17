using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using WpfApp1.actions;
using System.Windows.Media;
using System.Windows.Controls;

namespace WpfApp1.forms
{
    public partial class login : Window
    {

        //private Create_Table ct = new Create_Table();
        private Query query = new Query();
        private Create_FileStream cf = new Create_FileStream();
        public static Dictionary<string, int> list_permissions;

        public login()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
            Closing += Window_Closing;
            Constantes c = new Constantes();
            c.GetColors();
            var color = (Color)ColorConverter.ConvertFromString(Constantes.Colors["background_login"]);
            var brush = new SolidColorBrush(color);
            Background = brush;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            psswd.Focus();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Psswd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string value_psswd = psswd.Password.ToString();

                cf.App_StartUp("User");
                cf.FileStreamReader("User");
                if (query.Login(value_psswd))
                {
                    // Limpiar el contenido del PasswordBox antes de cambiar de ventana
                    psswd.Password = "";

                    list_permissions = Query.list_permissions;
                    Menu menu = new Menu();
                    menu.Show();
                    Hide();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
