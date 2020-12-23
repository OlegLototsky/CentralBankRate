using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Net;
using System.Xml.Linq;


namespace Программа_курса_денег
{
    public partial class Form1 : Form
    {
        public Form1()
        {
           InitializeComponent();
           Parse();
           
           

        }
        private void btn_search_Click(object sender, EventArgs e)
        {
            string s_name = comboBox1.Text;

            var dollar = Currencies.Where(x => x.CharCode == s_name).FirstOrDefault();

            textBox1.Text =  dollar.Price;

        }

        public void Parse()
        {
         
        DateTime timebank = new DateTime(2020, 12, 24);

            string url = $"http://www.cbr.ru/scripts/XML_daily.asp?date_req={timebank.ToString("dd/MM/yyyy")}";

            WebClient wc = new WebClient();
            wc.Encoding = System.Text.Encoding.UTF8;

            string BankString = wc.DownloadString(url);

            XmlDocument urlbank = new XmlDocument();
            urlbank.LoadXml(BankString);

            XmlNodeList XmlBankList = urlbank.GetElementsByTagName("Valute");
            foreach (XmlNode node in XmlBankList)
            {
                Currency currency = new Currency()
                {
                    Name = node["Name"].InnerText,
                    CharCode = node["CharCode"].InnerText,
                    Price = node["Value"].InnerText, // нужно переделать под decimal
                   };

                Currencies.Add(currency);
            }
      
        }

        public List<Currency> Currencies { get; } = new List<Currency>();
        public class Currency
        { 
            public string CharCode { get; set; }

            public string Name { get; set; }

            private decimal  price;

            public string Price
            {
                get { return price.ToString("c"); }
                set { price = Convert.ToDecimal(value); }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
