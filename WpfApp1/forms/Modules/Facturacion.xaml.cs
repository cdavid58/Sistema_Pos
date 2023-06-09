﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace WpfApp1.forms.Modules
{
    public partial class Facturacion : Window
    {

        private int number_invoice = 0;
        public static string Change_Price = "";

        public Facturacion()
        {
            InitializeComponent();
            Loaded += Facturacion_Loaded;
            Change_Price = Menu.chage_price;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControl.SelectedItem == null) return;

            TabItem selectedTab = (TabItem)tabControl.SelectedItem;

            if (selectedTab.Header.ToString() == "+")
            {
                CreateNewTab();
            }
        }

        private void Facturacion_Loaded(object sender, RoutedEventArgs e)
        {
            CreateNewTab();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void AgregarPestaña_Click(object sender, RoutedEventArgs e)
        {
            CreateNewTab();
        }

        private void CerrarPestana_Click(object sender, RoutedEventArgs e)
        {
            Button closeButton = sender as Button;
            TabItem tabItem = FindParentTabItem(closeButton);

            if (tabItem != null)
            {
                if (number_invoice > 1)
                {
                    tabControl.Items.Remove(tabItem);
                    number_invoice--;
                }
                else
                {
                    MessageBox.Show("No puede cerrar la ultima pestaña abierta", "CUIDADO", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                
            }
        }

        private TabItem FindParentTabItem(UIElement element)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(element);

            while (parent != null && !(parent is TabItem))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as TabItem;
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            Hide();
        }


        private void CreateNewTab()
        {
            TabItem newTab = new TabItem();
            newTab.Header = "Facturacion " + (number_invoice + 1).ToString();
            Frame frame = new Frame();
            designs.form_invoice fi = new designs.form_invoice();
            frame.Content = fi;
            newTab.Content = frame;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                int insertIndex = tabControl.Items.Count;

                tabControl.Items.Insert(insertIndex, newTab);
                tabControl.SelectedItem = newTab;
            }));
            number_invoice++;
        }

        
        private void WebBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            WebBrowser wb = (WebBrowser)sender;
            TabItem ti = (TabItem)wb.Parent;

            dynamic document = wb.Document;

            wb.LoadCompleted -= WebBrowser_LoadCompleted;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (document != null && !string.IsNullOrEmpty(document.Title))
                {
                    string documentTitle = document.Title;
                    ti.Header = documentTitle;
                }
                else
                {
                    // Manejar el caso en que no se pudo obtener el título del documento
                    ti.Header = "Página sin título";
                }
            }));
        }

        private void CerrarVentana_Click(object sender, RoutedEventArgs e)
        {
            if (tabControl.Items.Count > 1)
            {
                TabItem lastTab = (TabItem)tabControl.Items[tabControl.Items.Count - 1];
                tabControl.Items.Remove(lastTab);
                number_invoice--;
            }
            else
            {
                MessageBox.Show("No puede cerrar la ultima pestaña abierta", "CUIDADO", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void Facturacion_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
