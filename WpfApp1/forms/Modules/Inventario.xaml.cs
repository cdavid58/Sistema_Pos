using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Hardcodet.Wpf.TaskbarNotification;
using WpfApp1.actions;

namespace WpfApp1.forms.Modules
{
    public partial class Inventario : Window
    {

        private TaskbarIcon notifyIcon;
        private int id_subcategory = 0;
        private QueryInventory qi = new QueryInventory();
        private int action = 1;

        public static Dictionary<string, string> list_data = new Dictionary<string, string>();

        public Inventario()
        {
            InitializeComponent();
            Closing += Window_Closing;
            LoadComboBoxData();
            txtCode.Focus();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
        
        private void LoadComboBoxData()
        {
            DataTable dataTable = new DataTable();
            qi.GetCategory().Fill(dataTable);
            Category.ItemsSource = dataTable.DefaultView;
            Category.DisplayMemberPath = "name_category";
            Category.SelectedValuePath = "id";
        }

        private void Category_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            DataRowView selectedItem = (DataRowView)Category.SelectedItem;
            Subcategory.IsEnabled = true;
            LoadSubCategory(Convert.ToInt32(selectedItem["id"]));
        }

        private void LoadSubCategory(int id)
        {
            DataTable dataTable = new DataTable();
            qi.GetSubcategory(id).Fill(dataTable);
            Subcategory.ItemsSource = dataTable.DefaultView;
            Subcategory.DisplayMemberPath = "name_subcategory";
            Subcategory.SelectedValuePath = "id";
        }

        private void Caltulate_Utili(TextBox price, TextBox cost, TextBox utili)
        {
            if (!string.IsNullOrEmpty(price.Text))
            {
                
                float result = float.Parse(price.Text) - float.Parse(cost.Text);
                if (result > 0)
                {
                    utili.Text = "$" + result.ToString("N2");
                }
                else
                {
                    utili.Text = "$0";
                }
            }
            else
            {
                utili.Text = "$0";
            }
            

        }

        private void TxtPrice1_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            Caltulate_Utili(txtPrice1, txtCost, txtUtili1);
        }

        private void TxtPrice2_TextChanged(object sender, TextChangedEventArgs e)
        {
            Caltulate_Utili(txtPrice2, txtCost, txtUtili2);
        }

        private void TxtPrice3_TextChanged(object sender, TextChangedEventArgs e)
        {
            Caltulate_Utili(txtPrice3, txtCost, txtUtili3);
        }

        private void TxtPrice4_TextChanged(object sender, TextChangedEventArgs e)
        {
            Caltulate_Utili(txtPrice4, txtCost, txtUtili4);
        }

        private void TxtPrice5_TextChanged(object sender, TextChangedEventArgs e)
        {
            Caltulate_Utili(txtPrice5, txtCost, txtUtili5);
        }

        private void TxtPrice6_TextChanged(object sender, TextChangedEventArgs e)
        {
            Caltulate_Utili(txtPrice6, txtCost, txtUtili6);
        }

        private void TxtQuanty_TextChanged(object sender, TextChangedEventArgs e)
        {
            var quanty = txtQuanty.Text;
            var cost = txtCost.Text;
            if ( !string.IsNullOrEmpty(quanty) && !string.IsNullOrEmpty(cost))
            {
                txtTotal.Text = (float.Parse(quanty) * float.Parse(cost)).ToString();
            }
            else
            {
                txtTotal.Text = "$0";
            }

        }

        private void TxtCost_TextChanged(object sender, TextChangedEventArgs e)
        {
            var quanty = txtQuanty.Text;
            var cost = txtCost.Text;
            if (!string.IsNullOrEmpty(quanty) && !string.IsNullOrEmpty(cost))
            {
                txtTotal.Text = (float.Parse(quanty) * float.Parse(cost)).ToString();
            }
            else
            {
                txtTotal.Text = "$0";
            }
        }

        private void Convert_To_Miles(TextBox txt)
        {
            try
            {
                txt.Text = "$" + float.Parse(txt.Text).ToString("N2");
            }
            catch (Exception)
            {}
            
        }

        private void TxtPrice6_LostFocus(object sender, RoutedEventArgs e)
        {
            Convert_To_Miles(txtCost);
            Convert_To_Miles(txtTotal);
            Convert_To_Miles(txtPrice1);
            Convert_To_Miles(txtPrice2);
            Convert_To_Miles(txtPrice3);
            Convert_To_Miles(txtPrice4);
            Convert_To_Miles(txtPrice5);
            Convert_To_Miles(txtPrice6);
        }

        private void TxtMarca_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Enter)
            {
                Save_Product sp = new Save_Product();
                notifyIcon = new TaskbarIcon();
                notifyIcon.ToolTipText = "Theriosoft";
                bool result = false;
                string action_result = "";
                if (action == 1)
                {
                    result = sp.Create_Product(Load_Data());
                    action_result = "creado";
                }else if(action == 2)
                {
                    result = sp.Update_Product(Load_Data());
                    action_result = "actualizado";
                }

                if (result)
                {
                    notifyIcon.ShowBalloonTip("Error", $"El producto fué {action_result} con éxito!.", BalloonIcon.Info);
                    ClearTextBoxes();
                }
                else
                {
                    notifyIcon.ShowBalloonTip("Error", $"El producto no fué {action_result}!.", BalloonIcon.Info);
                }
                


            }
        }

        private void Subcategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView selectedItem = (DataRowView)Subcategory.SelectedItem;
            id_subcategory = Convert.ToInt32(selectedItem["id"]);
            

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            modals.edit_product ep = new modals.edit_product();
            ep.Owner = this;
            ep.ShowDialog();
            LoadData();
            e.Handled = true;
        }

        private void TxtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.E)
            {
                modals.edit_product ep = new modals.edit_product();
                ep.Owner = this;
                ep.ShowDialog();
                LoadData();
                e.Handled = true;
            }
        }

        private Product Load_Data()
        {
            Product p = new Product
            {
                code = long.Parse(txtCode.Text),
                product = txtProduct.Text,
                cost = float.Parse(txtCost.Text.Replace("$", "")),
                price_1 = float.Parse(txtPrice1.Text.Replace("$", "")),
                price_2 = float.Parse(txtPrice2.Text.Replace("$", "")),
                price_3 = float.Parse(txtPrice3.Text.Replace("$", "")),
                price_4 = float.Parse(txtPrice4.Text.Replace("$", "")),
                price_5 = float.Parse(txtPrice5.Text.Replace("$", "")),
                price_6 = float.Parse(txtPrice6.Text.Replace("$", "")),
                quantity = int.Parse(txtQuanty.Text),
                pack = int.Parse(txtQuanty.Text),
                display = int.Parse(txtQuanty.Text),
                quantity_static = Query.type_company == 1 ? int.Parse(txtQuanty.Text) : 0,
                pack_static = Query.type_company == 1 ? int.Parse(txtQuanty.Text) : 0,
                display_static = Query.type_company == 1 ? int.Parse(txtQuanty.Text) : 0,
                tax = int.Parse(txtTax.Text),
                ipo = 0,
                discount = 0,
                subcategory = id_subcategory,
                address = txtAddress.Text,
                brand = txtBrand.Text,
                stock = int.Parse(txtStock.Text)
            };
            return p;
        }

        private void LoadData()
        {
            if (list_data.Count > 0)
            {
                Enabled_TextBox(true);
                txtCode.Text = list_data["code"];
                txtCodeInt.Text = list_data["code"];
                txtProduct.Text = list_data["product"];
                txtCost.Text = list_data["cost"];
                txtPrice1.Text = list_data["price_1"];
                txtPrice2.Text = list_data["price_2"];
                txtPrice3.Text = list_data["price_3"];
                txtPrice4.Text = list_data["price_4"];
                txtPrice5.Text = list_data["price_5"];
                txtPrice6.Text = list_data["price_6"];
                txtQuanty.Text = list_data["quantity"];
                txtBrand.Text = list_data["brand"];
                txtAddress.Text = list_data["address"];
                txtStock.Text = list_data["stock"];
                txtInvIni.Text = list_data["invini"];
                txtTax.Text = list_data["tax"];
                DataTable dataTable = new DataTable();
                qi.GetCategory().Fill(dataTable);
                Category.ItemsSource = dataTable.DefaultView;
                Category.DisplayMemberPath = "name_category";
                Category.SelectedValuePath = "id";
                SeleccionarOpcionPorId(int.Parse(list_data["id_category"]), int.Parse(list_data["subcategory_id"]));
                action = 2;
            }
        }

        private void SeleccionarOpcionPorId(int id_category, int id_subcategory)
        {
            if (id_category != -1 && id_subcategory != -1)
            {
                int find_id_category = id_category - 1;
                int find_id_subcategory = id_subcategory - 1;
                if (find_id_category >= 0)
                {
                    Category.SelectedIndex = find_id_category;
                }
                if (find_id_subcategory >= 0)
                {
                    Subcategory.SelectedIndex = find_id_subcategory;
                }
            }
        }




        private IEnumerable<Control> GetControlsTextBoxes(DependencyObject parent)
        {
            var count = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is TextBox)
                {
                    yield return child as TextBox;
                }
                else
                {
                    foreach (var descendant in GetControlsTextBoxes(child))
                    {
                        yield return descendant;
                    }
                }
            }
        }

        private void ClearTextBoxes()
        {
            foreach (var control in GetControlsTextBoxes(this))
            {
                if (control is TextBox textBox)
                {
                    textBox.Text = string.Empty;
                }
            }
            txtCode.Focus();
            txtUtili1.Text = "$0";
            txtUtili2.Text = "$0";
            txtUtili3.Text = "$0";
            txtUtili4.Text = "$0";
            txtUtili5.Text = "$0";
            txtUtili6.Text = "$0";
            txtInvIni.Text = "0";
        }

        private void Add_product_Click(object sender, RoutedEventArgs e)
        {
            ClearTextBoxes();
            action = 1;
            Enabled_TextBox(true);
            txtCode.Focus();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            ModalPopup.IsOpen = true;
        }

        private void CerrarModal_Click(object sender, RoutedEventArgs e)
        {
            ModalPopup.IsOpen = false;
        }

        private void Delete_Product_Click(object sender, RoutedEventArgs e)
        {
            Save_Product sp = new Save_Product();
            if (sp.DeleteProduct(long.Parse(txtCode.Text)))
            {
                notifyIcon = new TaskbarIcon();
                notifyIcon.ToolTipText = "Theriosoft";
                notifyIcon.ShowBalloonTip("Existo", $"El producto fué eliminado con éxito!.", BalloonIcon.Info);
                ClearTextBoxes();
                ModalPopup.IsOpen = false;
                Enabled_TextBox(true);
            }
        }

        private void BtnSearchProduct_Click(object sender, RoutedEventArgs e)
        {
            modals.edit_product ep = new modals.edit_product();
            ep.Owner = this;
            ep.ShowDialog();
            LoadData();
            e.Handled = true;
            Enabled_TextBox(false);
        }

        private void Enabled_TextBox(bool value)
        {
            foreach (var control in GetControlsTextBoxesSearch(this))
            {
                if (control is TextBox textBox)
                {
                    textBox.IsEnabled = value;
                }
            }
        }


        private IEnumerable<Control> GetControlsTextBoxesSearch(DependencyObject parent)
        {
            var count = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is TextBox)
                {
                    yield return child as TextBox;
                }
                else
                {
                    foreach (var descendant in GetControlsTextBoxesSearch(child))
                    {
                        yield return descendant;
                    }
                }
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Menu m = new Menu();
            m.Show();
            Hide();
        }
    }
}
