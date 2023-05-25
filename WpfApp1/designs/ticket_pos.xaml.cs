using System.Collections.Generic;
using System.Windows;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System;
using System.Drawing;

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
            var items = new List<Item>
            {
                new Item { Nombre = "Producto 1", Cantidad = 2, PrecioUnitario = 10, Subtotal = 20 },
                new Item { Nombre = "Producto 2", Cantidad = 1, PrecioUnitario = 15, Subtotal = 15 },
                new Item { Nombre = "Producto 3", Cantidad = 3, PrecioUnitario = 5, Subtotal = 15 },
                new Item { Nombre = "Producto 4", Cantidad = 4, PrecioUnitario = 8, Subtotal = 32 }
            };

            DataContext = new FacturaViewModel { Items = items, Total = 82 };
        }

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
            string printerName = GetDefaultPrinterName(); // Obtiene el nombre de la impresora térmica predeterminada

            // Crea el contenido de la factura
            string content = "Factura POS" + "\n\n";
            foreach (var item in ((FacturaViewModel)DataContext).Items)
            {
                content += item.Nombre + " - " + item.Cantidad + " x $" + item.PrecioUnitario + " = $" + item.Subtotal + "\n";
            }
            content += "\nTotal: $" + ((FacturaViewModel)DataContext).Total;

            // Imprime el contenido en la impresora térmica
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

        //private void PrintContent(string printerName, string content)
        //{
        //    IntPtr printerHandle;
        //    DOCINFOA docInfo = new DOCINFOA();
        //    bool success;

        //    docInfo.pDocName = "Factura POS";
        //    docInfo.pDataType = "RAW";

        //    success = OpenPrinter(printerName.Normalize(), out printerHandle, IntPtr.Zero);
        //    if (!success)
        //    {
        //        MessageBox.Show("No se pudo abrir la impresora.");
        //        return;
        //    }

        //    success = StartDocPrinter(printerHandle, 1, docInfo);
        //    if (!success)
        //    {
        //        MessageBox.Show("No se pudo iniciar el documento de impresión.");
        //        ClosePrinter(printerHandle);
        //        return;
        //    }

        //    success = StartPagePrinter(printerHandle);
        //    if (!success)
        //    {
        //        MessageBox.Show("No se pudo iniciar la página de impresión.");
        //        EndDocPrinter(printerHandle);
        //        ClosePrinter(printerHandle);
        //        return;
        //    }

        //    int dwWritten;
        //    IntPtr ptr = Marshal.StringToCoTaskMemAnsi(content);
        //    success = WritePrinter(printerHandle, ptr, content.Length, out dwWritten);
        //    if (!success)
        //    {
        //        MessageBox.Show("Error al escribir en la impresora.");
        //        EndPagePrinter(printerHandle);
        //        EndDocPrinter(printerHandle);
        //        ClosePrinter(printerHandle);
        //        return;
        //    }

        //    success = EndPagePrinter(printerHandle);
        //    if (!success)
        //    {
        //        MessageBox.Show("No se pudo finalizar la página de impresión.");
        //        EndDocPrinter(printerHandle);
        //        ClosePrinter(printerHandle);
        //        return;
        //    }

        //    success = EndDocPrinter(printerHandle);
        //    if (!success)
        //    {
        //        MessageBox.Show("No se pudo finalizar el documento de impresión.");
        //        ClosePrinter(printerHandle);
        //        return;
        //    }

        //    success = ClosePrinter(printerHandle);
        //    if (!success)
        //    {
        //        MessageBox.Show("No se pudo cerrar la impresora.");
        //        return;
        //    }

        //    Marshal.FreeCoTaskMem(ptr);
        //}
    }

    public class Item
    {
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class FacturaViewModel
    {
        public List<Item> Items { get; set; }
        public decimal Total { get; set; }
    }
}
