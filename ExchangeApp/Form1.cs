using ExchangeApp.Services;

namespace ExchangeApp
{
    public partial class Form1 : Form
    {
        private readonly ExchangeService exchangeService; 
        private decimal? SellingExchangeRate = null;
        private decimal? PurchaseExchangeRate = null;
        public Form1()
        {
            exchangeService = new ExchangeService();
            InitializeComponent();

            configSellingTextBoxOnChange();
            configPurchaseTextBoxOnChange();
        }
        private void configPurchaseTextBoxOnChange()
        {
            textBox3.Text = "USD$ ";

            textBox3.TextChanged += (Object sender, EventArgs args) =>
            {

                if (!label10.Visible)
                    label10.Visible = true;


                if (!textBox3.Text.StartsWith("USD$ "))
                {
                    textBox3.Text = "USD$ ";
                    textBox3.SelectionStart = textBox3.Text.Length;
                }


                if (!SellingExchangeRate.HasValue)
                {
                    label10.Text = "La tasa no ha cargado, favor intente mas tarde.";
                    return;
                }


                string textVal = textBox3.Text.Replace("USD$ ", string.Empty);

                bool isValidNumber = decimal.TryParse(textVal, out decimal validNum);

                if (!isValidNumber)
                {
                    label10.Text = "Ingrese un número valido.";
                    return;
                }


                decimal totalCompra = validNum * PurchaseExchangeRate.Value;
                decimal totalVenta = validNum * SellingExchangeRate.Value;
                label10.Text = $"Total Compra: RD{totalCompra.ToString("C")}\nTotal venta: RD{totalVenta.ToString("C")}";
            };
        }
        private void configSellingTextBoxOnChange()
        {
            textBox2.Text = "RD$ ";

            textBox2.TextChanged += (Object sender, EventArgs args) =>
            {

                if(!label7.Visible)
                    label7.Visible = true;


                if (!textBox2.Text.StartsWith("RD$ "))
                {
                    textBox2.Text = "RD$ ";
                    textBox2.SelectionStart = textBox2.Text.Length;
                }


                if (!SellingExchangeRate.HasValue)
                {
                    label7.Text = "La tasa no ha cargado, favor intente mas tarde.";
                    return;
                }


                string textVal = textBox2.Text.Replace("RD$ ", string.Empty);

                bool isValidNumber = decimal.TryParse(textVal, out decimal validNum);

                if (!isValidNumber)
                {
                    label7.Text = "Ingrese un número valido.";
                    return;
                }


                decimal totalVenta = validNum / SellingExchangeRate.Value ;
                decimal totalCompra = validNum / PurchaseExchangeRate.Value;


                label7.Text = $"Total venta: USD{totalVenta.ToString("C")}\nTotal compra: USD{totalCompra.ToString("C")}";
            };
        }
        private void disableArrows()
        {
            foreach (Control control in this.Controls)
            {
                control.PreviewKeyDown += new PreviewKeyDownEventHandler(control_PreviewKeyDown);
            }
        }
        private void control_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                e.IsInputKey = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            var task = Task.Run(async () => await exchangeService.GetTodayExchange());

            var result = task.GetAwaiter().GetResult();


            SellingExchangeRate = result.actualSellingValue;
            PurchaseExchangeRate = result.actualPurchaseValue;


            label11.Text = $"Fecha: {result.date.ToString("dd/MMM/yyyy")}";

            label6.Visible = true;
            label11.Visible = true;

            if (!SellingExchangeRate.HasValue)
                label6.Text = "Error al cargar tasa del banco central";
            else
            {
                string sellingInfo = $"Tasa de venta: RD{SellingExchangeRate.Value.ToString("C")} \nTasa de compra: RD{PurchaseExchangeRate.Value.ToString("C")}";
                label6.Text = sellingInfo;

               
            }



        }

        
    }
}