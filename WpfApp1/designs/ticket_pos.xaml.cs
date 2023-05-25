using System.Collections.Generic;
using System.Windows;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System;
using System.Drawing;
using WpfApp1.actions;

namespace WpfApp1.designs
{
    /// <summary>
    /// Lógica de interacción para ticket_pos.xaml
    /// </summary>
    public partial class ticket_pos : Window
    {
        public ticket_pos()
        {
            InitializeComponent();

            // Llenar datos de ejemplo
            //var items = new List<Item>
            //{
            //    new Item { Nombre = "Producto 1", Cantidad = 2, PrecioUnitario = 10, Subtotal = 20 },
            //    new Item { Nombre = "Producto 2", Cantidad = 1, PrecioUnitario = 15, Subtotal = 15 },
            //    new Item { Nombre = "Producto 3", Cantidad = 3, PrecioUnitario = 5, Subtotal = 15 },
            //    new Item { Nombre = "Producto 4", Cantidad = 4, PrecioUnitario = 8, Subtotal = 32 }
            //};
            //DataContext =  new FacturaViewModel { Items = i.PrintPOS(Environment.UserName.ToString()), Total = 82 };
            invoice i = new invoice();
            var data = i.PrintPOS(Environment.UserName.ToString());
            DataContext = new FacturaViewModel { Items = data, Total = 82 };
            
        }

        #region

        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, int level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, int dwCount, out int dwWritten);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            string printerName = GetDefaultPrinterName();
            string content = "Factura POS" + "\n\n";
            foreach (var item in ((FacturaViewModel)DataContext).Items)
            {
                content += item.product + " - " + item.quantity + " x $" + item.cost + " = $" + item.subtotal + "\n";
            }
            content += "\nTotal: $" + ((FacturaViewModel)DataContext).Total;
            PrintContent(printerName, content);
        }
        private string GetDefaultPrinterName()
        {
            PrinterSettings settings = new PrinterSettings();
            return settings.PrinterName;
        }

        private void PrintContent(string printerName, string content)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrinterSettings.PrinterName = printerName;
            printDocument.PrintPage += (sender, e) =>
            {
                using (Font font = new Font("Courier New", 10))
                {
                    e.Graphics.DrawString(content, font, Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top);
                }
            };
            printDocument.Print();
        }

        #endregion
    }

    public class Item
    {
        public string product { get; set; }
        public int quantity { get; set; }
        public int cost { get; set; }
        public int subtotal { get; set; }
    }

    public class FacturaViewModel
    {
        public List<Item> Items { get; set; }
        public decimal Total { get; set; }
    }
}
