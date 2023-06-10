using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using WpfApp1.modals;

namespace WpfApp1.forms
{

    public partial class CXC : Window
    {



        public CXC()
        {
            InitializeComponent();
            txtSelectClient.Focus();
            txtSelectClient.Clear();
            SolidColorBrush cursorBrush = new SolidColorBrush(Colors.White);
            DropShadowEffect shadowEffect = new DropShadowEffect();
            shadowEffect.Color = Colors.Yellow;
            shadowEffect.BlurRadius = 10;
            border_dg.Effect = shadowEffect;
            txtSelectClient.CaretBrush = cursorBrush;
        }

        private void TxtCustomer_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                List_Client lc = new List_Client();
                lc.ShowDialog();

            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
