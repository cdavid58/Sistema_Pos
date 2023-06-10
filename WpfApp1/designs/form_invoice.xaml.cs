using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using WpfApp1.actions;
using WpfApp1.modals;
using Image = iText.Layout.Element.Image;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WpfApp1.designs
{

    public partial class form_invoice : Page
    {
        private List<Invoice> data = new List<Invoice>();
        public static Dictionary<string, string> client_list = new Dictionary<string, string>();
        public static Dictionary<string, string> product_list = new Dictionary<string, string>();
        public static string notes = "";
        public static int DaysExpire = 0, payment_method = 0, count_limit = 0, type_document = 1;
        public static double discount_to_invoice = 0;
        private DispatcherTimer timer;
        private double total_base = 0;

        public form_invoice()
        {
            InitializeComponent();
            txtCode.Focus();
            invoice i = new invoice();
            txtNumberInvoice.Text = i.GetConsecutive().ToString();
            dgInvoice.AllowDrop = false;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            Loaded += form_invoice_Loaded;
        }

        private async void form_invoice_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(async () =>
            {
                await Send_Invoice_Electronic();
            });
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            txtDateTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public int Payment_Form()
        {
            int value = 1;
            switch (payment_method)
            {
                case 10:
                    value = 1;
                    break;
                case 30:
                    value = 2;
                    break;
                case 31:
                    value = 1;
                    break;
            }
            return value;
        }

        private void TxtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (string.IsNullOrEmpty(txtCode.Text)) {
                    List_Product lp = new List_Product();
                    lp.ShowDialog();
                    e.Handled = true;
                    var data = product_list;
                    if (product_list.Count > 0)
                    {
                        txtCode.Text = data["code"];
                        txtExist.Text = data["quantity"];
                        txtProduct.Text = data["product"];
                        price_1.Text = data["price_1"];
                        price_2.Text = data["price_2"];
                        price_3.Text = data["price_3"];
                        price_4.Text = data["price_4"];
                        price_5.Text = data["price_5"];
                        price_6.Text = data["price_6"];
                        txtQuantity.Focus();
                        product_list.Clear();
                    }
                    else
                    {
                        MessageBox.Show("El producto no existe", "ALERTA", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    
                }
                else
                {
                    
                    if (!txtCode.Text.Contains("-"))
                    {
                        invoice i = new invoice();
                        var product = i.GetProduct(long.Parse(txtCode.Text));
                        if (product != null)
                        {
                            txtExist.Text = product.quantity.ToString();
                            price_1.Text = product.price_1.ToString();
                            price_2.Text = product.price_2.ToString();
                            price_3.Text = product.price_3.ToString();
                            price_4.Text = product.price_4.ToString();
                            price_5.Text = product.price_5.ToString();
                            price_6.Text = product.price_6.ToString();
                            txtProduct.Text = product.product;
                            txtQuantity.Focus();
                        }
                        else
                        {
                            MessageBox.Show("El producto que busca no existe", "ALERTA", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        DeleteRowProductCode(txtCode.Text.Replace("-",""));
                    }
                }
            }
            else if (e.Key == Key.C)
            {
                try
                {
                    List_Client lc = new List_Client();
                    lc.ShowDialog();
                    var data = client_list;
                    lbClient.Content = data["name"].ToString();
                    txtCode.Focus();
                    e.Handled = true;
                }
                catch (Exception) { }
            }
            else if (e.Key == Key.F)
            {
                if (dgInvoice.Items.Count > 0)
                {
                    data_invoice di = new data_invoice();
                    di.ShowDialog();
                    e.Handled = true;
                    HeaderInvoice hi = new HeaderInvoice
                    {
                        number = int.Parse(txtNumberInvoice.Text),
                        user = Query.pk_user,
                        terminal = Environment.UserName,
                        payment_form = Payment_Form(),
                        payment_method = payment_method,
                        notes = string.IsNullOrEmpty(notes) ? "No hay" : notes,
                        client_id = int.Parse(client_list["pk_client"]),
                        type_document = type_document
                    };
                    invoice i = new invoice();
                    i.CreateInvoice(hi);

                    if (dgInvoice.ItemsSource is IEnumerable<Invoice> data)
                    {
                        foreach (var item in data)
                        {
                            DetailsInvoice details_invoice = new DetailsInvoice
                            {
                                code = long.Parse(item.code),
                                product = item.product.ToString(),
                                quantity = int.Parse(item.quantity.ToString()),
                                price = double.Parse(item.cost.ToString()),
                                discount = int.Parse(item.discount.ToString()),
                                tax = int.Parse(item.tax.ToString()),
                                ipo = double.Parse(item.ipo.ToString()),
                                invoice_id = int.Parse(txtNumberInvoice.Text)
                            };
                            i.CreateDetailsInvoice(details_invoice);
                            GarbageCollector(details_invoice);
                        }
                    }
                    i.Create_Wallet(int.Parse(txtTotal.Text), DaysExpire, int.Parse(txtNumberInvoice.Text));
                    GarbageCollector(di);
                    GarbageCollector(hi);
                    GarbageCollector(i);
                    Clean();
                    MessageBox.Show("Factura creada con éxito", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    ticket_pos tp = new ticket_pos();
                    tp.Show();
                }
                else
                {
                    MessageBox.Show("Debe tener productos en la lista", "ALERTA", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void CalculateTotal()
        {
            int total = 0, subtotal = 0, tax = 0, discount = 0;
            if (dgInvoice.ItemsSource is IEnumerable<Invoice> data)
            {
                foreach (var item in data)
                {

                    discount += item.cost * (item.discount / 100) * item.quantity;
                    total += item.subtotal + item.tax_value;
                    subtotal += item.subtotal;
                    tax += (item.tax_value * item.quantity);
                    
                }
            }
            txtTotal.Text = total.ToString();
            txtTaxes.Text = tax.ToString();
            txtSubtotal.Text = subtotal.ToString();
            txtDiscount.Text = discount.ToString();
        }
        
        private bool ProductExist(int quantity, string code,int price)
        {
            bool exist = false;
            if (dgInvoice.ItemsSource is IEnumerable<Invoice> _data)
            {
                foreach (var item in _data)
                {
                    if (item.code == code && item.cost == price)
                    {
                        item.quantity += quantity;
                        item.subtotal += (quantity * item.cost);
                        exist = true;
                        break;
                    }
                }
            }
            if (exist)
            {
                dgInvoice.ItemsSource = null;
                dgInvoice.ItemsSource = data;
            }
            return exist;
        }

        private void TxtQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    if (int.Parse(txtQuantity.Text) > 0)
                    {
                        if (int.Parse(txtExist.Text) > 0)
                        {
                            invoice i = new invoice();
                            var invoice = i.GetQueryInvoice(int.Parse(txtQuantity.Text), long.Parse(txtCode.Text), int.Parse(txtTypePrice.Text),1);
                            total_base += invoice.cost * int.Parse(txtQuantity.Text);
                            if (!ProductExist(int.Parse(txtQuantity.Text), invoice.code, invoice.cost))
                            {
                                data.Add(new Invoice()
                                {
                                    code = invoice.code.ToString(),
                                    product = invoice.product,
                                    cost = invoice.cost,
                                    quantity = int.Parse(txtQuantity.Text),
                                    tax_value = invoice.tax_value,
                                    discount = invoice.discount,
                                    ipo = invoice.ipo,
                                    subtotal = invoice.subtotal,

                                });
                                txtItems.Text = dgInvoice.Items.Count.ToString();
                                dgInvoice.ItemsSource = null;
                                dgInvoice.ItemsSource = data;
                                dgInvoice.Items.Refresh();
                            }

                            txtTotalBase.Text = Math.Round(total_base).ToString();
                            int limit = int.Parse(txtTotalBase.Text);
                            if (limit > 212 && count_limit == 0)
                            {
                                MessageBox.Show("La factura sera enviada a electrónica, ya que ha superado el limite en la base", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                                count_limit = 1;
                                type_document = 2;
                            }

                            txtCode.Clear();
                            txtQuantity.Clear();
                            txtCode.Focus();
                            CalculateTotal();
                            GarbageCollector(invoice);
                        }
                        else
                        {
                            MessageBox.Show("Producto Agotado", "ALERTA", MessageBoxButton.OK, MessageBoxImage.Information);
                            txtCode.Clear();
                            txtCode.Focus();
                            txtQuantity.Clear();
                        }
                    }
                    else{
                        MessageBox.Show("No puede facturar en ceros", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show("No puede facturar en ceros", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            } 
        }

        private void BtnUltCopy_Click(object sender, RoutedEventArgs e)
        {
            ticket_pos tp = new ticket_pos();
            tp.Show();
        }

        private void BtnClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List_Client lc = new List_Client();
                lc.ShowDialog();
                var data = client_list;
                lbClient.Content = data["name"].ToString();
                txtCode.Focus();
            }
            catch (Exception) { }
        }

        private void GarbageCollector(object obj)
        {
            obj = null;
        }

        private void TxtTypePrice_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (int.Parse(txtExist.Text) > 0)
                    {
                        invoice i = new invoice();
                        var invoice = i.GetQueryInvoice(int.Parse(txtQuantity.Text), long.Parse(txtCode.Text), int.Parse(txtTypePrice.Text),1);
                        total_base += invoice.cost;
                        if (!ProductExist(int.Parse(txtQuantity.Text), invoice.code,invoice.cost))
                        {
                            data.Add(new Invoice()
                            {
                                code = invoice.code.ToString(),
                                product = invoice.product,
                                cost = invoice.cost,
                                quantity = int.Parse(txtQuantity.Text),
                                tax_value = invoice.tax_value,
                                discount = invoice.discount,
                                ipo = invoice.ipo,
                                subtotal = invoice.subtotal,

                            });
                            txtItems.Text = dgInvoice.Items.Count.ToString();
                            dgInvoice.ItemsSource = data;
                            dgInvoice.Items.Refresh();
                        }
                        txtTotalBase.Text = Math.Round(total_base).ToString();
                        int limit = int.Parse(txtTotalBase.Text);
                        if (limit > 212 && count_limit == 0)
                        {
                            MessageBox.Show("La factura sera enviada a electrónica, ya que ha superado el limite en la base", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                            count_limit = 1;
                        }

                        txtCode.Clear();
                        txtQuantity.Clear();
                        txtCode.Focus();
                        CalculateTotal();
                        txtTypePrice.Text = "1";
                        GarbageCollector(invoice);
                    }
                    else
                    {
                        MessageBox.Show("Producto Agotado", "ALERTA", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtCode.Clear();
                        txtCode.Focus();
                        txtQuantity.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("No puede facturar en ceros", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void TxtQuantity_GotFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCode.Text))
            {
                txtCode.Focus();
            }
        }

        private void TxtTypePrice_GotFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtQuantity.Text))
            {
                MessageBox.Show("No puede continuar si la cantidad esta en vacio", "ALERTA", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtQuantity.Focus();
            }
            else
            {
                txtTypePrice.Clear();
            }
        }

        private void Clean()
        {
            txtItems.Text = "0";
            txtExist.Text = "0";
            txtCode.Text = "";
            txtQuantity.Text = "";
            txtProduct.Text = "No hay producto seleccionado";
            price_1.Text = "0";
            price_2.Text = "0";
            price_3.Text = "0";
            price_4.Text = "0";
            price_5.Text = "0";
            price_6.Text = "0";
            lbClient.Content = "Consumidor Final";
            txtSubtotal.Text = "0";
            txtTaxes.Text = "0";
            txtDiscount.Text = "0";
            txtTotal.Text = "0";
            txtTotalBase.Text = "0";
            txtCode.Focus();
            data_invoice di = new data_invoice();
            di.txtNotes.Text = "";
            di.txtDaysExpire.Text = "0";
            di.cbFPago.SelectedIndex = 0;
            dgInvoice.ItemsSource = null;
            data.Clear();
            invoice i = new invoice();
            txtNumberInvoice.Text = i.GetConsecutive().ToString();
            GarbageCollector(di);
            GarbageCollector(i);
        }

        private void BtnPrice_Click(object sender, RoutedEventArgs e)
        {
            if (lbClient.Content.ToString() != "Consumidor Final")
            {
                invoice order = new invoice();
                var data_client = client_list;
                string projectDirectory = Directory.GetCurrentDirectory();
                var logoPath = System.IO.Path.Combine(projectDirectory, "image", "logo.png");
                var logo = new Image(ImageDataFactory.Create(logoPath));

                float mediaCartaWidth = 800;
                float mediaCartaHeight = 396;
                var pageSize = new PageSize(mediaCartaWidth, mediaCartaHeight);
                pageSize.Rotate();
                var filePath = @"C:\Users\Desarrollo2\Desktop\Cotizaciones\factura.pdf";
                var writer = new PdfWriter(filePath);
                var pdf = new PdfDocument(writer);
                pdf.SetDefaultPageSize(pageSize);
                var document = new Document(pdf, pageSize);

                var headerTable = new Table(new float[] { 2, 1, 1 });
                headerTable.SetWidth(UnitValue.CreatePercentValue(100));

                logo.ScaleToFit(100, 100);
                var logoCell = new Cell().Add(logo);
                headerTable.AddCell(logoCell);

                var razonSocial = new Paragraph("Theriosoft s.a.s")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

                var nit = new Paragraph("NIT: 900541566")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

                var phone = new Paragraph("Teléfono: 3145080696")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

                var email = new Paragraph("Email: theriosoft@gmail.com")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

                var resolucion = new Paragraph("Resolución: 18764009127371")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

                var client_name = new Paragraph($"Nombre: {data_client["name"].ToString()}")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

                var client_phone = new Paragraph($"Teléfono: {data_client["phone"].ToString()}")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

                var client_nit = new Paragraph($"Documento I: {data_client["documentI"].ToString()}")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

                var client_address = new Paragraph($"Dirección: : {data_client["address"].ToString()}")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

                headerTable.AddCell(new Cell().Add(razonSocial).Add(nit).Add(phone).Add(email).Add(resolucion));
                headerTable.AddCell(new Cell().Add(client_nit).Add(client_name).Add(client_phone).Add(client_address));

                document.Add(headerTable);

                var detalleFactura = new Paragraph($"Detalle de la cotización - N° {order.GetConsecutivePrice()} -  Fecha:{DateTime.Now.ToString("dd/MM/yyyy")}");
                detalleFactura.SetBold();
                detalleFactura.SetFontSize(16);
                document.Add(detalleFactura);

                var tablaDetalle = new Table(UnitValue.CreatePercentArray(new float[] { 25, 25, 25, 25 }));
                tablaDetalle.SetWidth(UnitValue.CreatePercentValue(100));
                tablaDetalle.AddCell("Producto");
                tablaDetalle.AddCell("Cantidad");
                tablaDetalle.AddCell("IVA");
                tablaDetalle.AddCell("Subtotal");
                foreach (var item in data)
                {
                    Console.WriteLine(item.code);
                    var quantity_cell = new Cell().SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).Add(new Paragraph(item.quantity.ToString()));
                    var tax_value_cell = new Cell().SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).Add(new Paragraph(item.tax_value.ToString()));
                    var subtotal_cell = new Cell().SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).Add(new Paragraph(item.subtotal.ToString()));

                    tablaDetalle.AddCell(item.product);
                    tablaDetalle.AddCell(quantity_cell);
                    tablaDetalle.AddCell(tax_value_cell);
                    tablaDetalle.AddCell(subtotal_cell);
                }

                document.Add(tablaDetalle);

                var piePagina = new Paragraph("Este documento no es una factura válida, solo es una cotización.");
                piePagina.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                document.Add(piePagina);

                document.Close();

                Console.WriteLine("La factura se ha generado exitosamente.");

                System.Diagnostics.Process.Start(filePath);
            }
            else
            {
                MessageBox.Show("Debe elegir un cliente","Advertencia", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void DeleteRowProductCode(string code)
        {
            Invoice invoice = data.FirstOrDefault(data => data.code == code);

            if (invoice != null)
            {
                data.Remove(invoice);
                dgInvoice.Items.Refresh();
                total_base -= invoice.cost * invoice.quantity;
                txtTotalBase.Text = Math.Round(total_base).ToString();
                int limit = int.Parse(txtTotalBase.Text);
                if (limit < 212 && count_limit == 1)
                {
                    MessageBox.Show("La factura sera enviada a POS, ya que no ha superado el limite en la base", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    count_limit = 0;
                    type_document = 1;
                }
                txtCode.Clear();
                txtQuantity.Clear();
                txtCode.Focus();
                CalculateTotal();
                invoice i = new invoice();
                i.ReturnProduct(code);
            }
        }
        

        private void BtntDelete_Click(object sender, RoutedEventArgs e)
        {
            Button eliminarButton = (Button)sender;
            DataGridRow dataGridRow = FindVisualParent<DataGridRow>(eliminarButton);
            Invoice invoice = (Invoice)dataGridRow.Item;
            data.Remove(invoice);
            dgInvoice.Items.Refresh();
            total_base -= invoice.cost * invoice.quantity;
            txtTotalBase.Text = Math.Round(total_base).ToString();
            int limit = int.Parse(txtTotalBase.Text);
            if (limit > 212 && count_limit == 0)
            {
                MessageBox.Show("La factura sera enviada a electrónica, ya que ha superado el limite en la base", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                count_limit = 1;
            }
            txtCode.Clear();
            txtQuantity.Clear();
            txtCode.Focus();
            CalculateTotal();
            GarbageCollector(dataGridRow);
            invoice i = new invoice();
            i.ReturnProduct(invoice.code);
        }

        private static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
                return null;

            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindVisualParent<T>(parentObject);
        }

        static async Task Send_Invoice_Electronic()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://theriosoft.com:8080/api/ubl2.1/invoice");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", "Bearer fbcaff08718a4625e4885e76ac190d6cade6c001480ef6977994ece1829496f7");
            #region
            var payload = @"
                {
                    ""number"": 869,
                    ""type_document_id"": 1,
                    ""date"": ""2023-06-01"",
                    ""time"": ""04:08:12"",
                    ""resolution_number"": ""18760000001"",
                    ""prefix"": ""SETP"",
                    ""notes"": """",
                    ""disable_confirmation_text"": true,
                    ""establishment_name"": ""TORRE SOFTWARE"",
                    ""establishment_address"": ""BRR LIMONAR MZ 6 CS 3 ET 1 PISO 2"",
                    ""establishment_phone"": ""3226563672"",
                    ""establishment_municipality"": 600,
                    ""seze"": ""2021-2017"",
                    ""foot_note"": """",
                    ""customer"": {
                        ""identification_number"": 800135582,
                        ""dv"": 7,
                        ""name"": ""FUNDACION ALEJANDRO LONDOÑO"",
                        ""phone"": ""3105193539"",
                        ""address"": ""CLL 4 NRO 33-90"",
                        ""email"": ""sistemas@disuniversal.com"",
                        ""merchant_registration"": ""0000000-00"",
                        ""type_document_identification_id"": 6,
                        ""type_organization_id"": 1,
                        ""type_liability_id"": 117,
                        ""municipality_id"": 822,
                        ""type_regime_id"": 1
                    },
                    ""payment_form"": {
                        ""payment_form_id"": 1,
                        ""payment_method_id"": 10,
                        ""payment_due_date"": ""2023-06-01"",
                        ""duration_measure"": ""0""
                    },
                    ""legal_monetary_totals"": {
                        ""line_extension_amount"": ""100"",
                        ""tax_exclusive_amount"": ""100"",
                        ""tax_inclusive_amount"": ""100"",
                        ""payable_amount"": ""100""
                    },
                    ""tax_totals"": [
                        {
                            ""tax_id"": 1,
                            ""tax_amount"": ""0"",
                            ""percent"": ""0"",
                            ""taxable_amount"": ""100""
                        }
                    ],
                    ""invoice_lines"": [
                        {
                            ""ipo"": ""0"",
                            ""unit_measure_id"": 70,
                            ""invoiced_quantity"": ""1"",
                            ""line_extension_amount"": ""100"",
                            ""free_of_charge_indicator"": false,
                            ""tax_totals"": [
                                {
                                    ""tax_id"": 1,
                                    ""tax_amount"": ""0"",
                                    ""taxable_amount"": ""100"",
                                    ""percent"": ""0""
                                }
                            ],
                            ""description"": ""COMISION POR SERVICIOS"",
                            ""notes"": ""ESTA ES UNA PRUEBA DE NOTA DE DETALLE DE LINEA"",
                            ""code"": ""COMISION"",
                            ""type_item_identification_id"": 4,
                            ""price_amount"": ""100"",
                            ""base_quantity"": ""1""
                        }
                    ]
                }";
            #endregion
            request.Content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var respuesta = await response.Content.ReadAsStringAsync();
            Dictionary<string, object> diccionario = JsonConvert.DeserializeObject<Dictionary<string, object>>(respuesta);
            string message = diccionario["message"].ToString();
            if (diccionario.ContainsKey("message"))
            {
                Console.WriteLine(message);
            }
            else
            {
                Console.WriteLine(respuesta);
                foreach(var i in diccionario)
                {
                    Console.WriteLine(i.Key + " " + i.Value);
                }
            }
        }




    }
}
