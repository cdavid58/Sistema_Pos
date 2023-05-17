using System;
using System.Collections.Generic;
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

namespace WpfApp1.modals
{
    /// <summary>
    /// Lógica de interacción para delete_product.xaml
    /// </summary>
    public partial class delete_product : Window
    {
        public delete_product()
        {
            InitializeComponent();
        }
        private void AbrirModal_Click(object sender, RoutedEventArgs e)
        {
            MostrarModal();
        }

        private void CerrarModal_Click(object sender, RoutedEventArgs e)
        {
            OcultarModal();
        }

        private void MostrarModal()
        {
            ModalPopup.IsOpen = true;
        }

        private void OcultarModal()
        {
            ModalPopup.IsOpen = false;
        }
    }
}
