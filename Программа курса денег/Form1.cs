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
    public partial class MainForm : Form
    {
        public MainForm()
        {
           InitializeComponent();
           Parse();
            ParseOLD();
            Colors();
           

        }
        
        public void Colors()
        { 
        
        
        }

        public void Parse() // загрузка данных из ЦБ на сегоднящний день
        {

            string timebank = DateTime.Today.ToString("dd/MM/yyyy");

            string url = $"http://www.cbr.ru/scripts/XML_daily.asp?date_req={timebank}";

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
                    Price = Convert.ToDecimal(node["Value"].InnerText), // нужно переделать под decimal
                   };

                Currencies.Add(currency);
            }
      
        }

        public void ParseOLD() // загрузка данных из ЦБ на вчерашний день
        {

            string OldTimeBank = DateTime.Today.AddDays(-1).ToString("dd/MM/yyyy");

            string url = $"http://www.cbr.ru/scripts/XML_daily.asp?date_req={OldTimeBank}";

            WebClient wc = new WebClient();
            wc.Encoding = System.Text.Encoding.UTF8;

            string BankString = wc.DownloadString(url);

            XmlDocument urlbank = new XmlDocument();
            urlbank.LoadXml(BankString);

            XmlNodeList XmlBankList = urlbank.GetElementsByTagName("Valute");
            foreach (XmlNode node in XmlBankList)
            {
                Currency currency1 = new Currency()
                {
                    Name = node["Name"].InnerText,
                    CharCode = node["CharCode"].InnerText,
                    Price = Convert.ToDecimal(node["Value"].InnerText), // нужно переделать под decimal
                };

                CurrenciesOLD.Add(currency1);
            }

        }

        private void btn_search_Click(object sender, EventArgs e) //кнопка поиска
        {
            string sName = comboBox1.Text;

            var CharCodePrice = Currencies.Where(x => x.CharCode == sName).FirstOrDefault();

            var OldCharCodePrice = CurrenciesOLD.Where(x => x.CharCode == sName).FirstOrDefault();

            if (string.IsNullOrEmpty(this.comboBox1.Text))
            {
                textBox1.Clear();
                textBox2.Clear();
                MessageBox.Show("Ошибка. Выберите валюту ", "Сообщение");
            }
            else
            {
                textBox1.Text = CharCodePrice.Price.ToString("G");

                textBox2.Text = OldCharCodePrice.Price.ToString("G");
            }

        }

        private void btn_clear_Click(object senderr, EventArgs e) // Кнопка очистки
        {
            textBox1.Clear();
            textBox2.Clear();
            comboBox1.SelectedIndex = -1;
        }

        public List<Currency> Currencies { get; } = new List<Currency>();

        public List<Currency> CurrenciesOLD { get; } = new List<Currency>();
        public class Currency
        { 
            public string CharCode { get; set; }

            public string Name { get; set; }

            public decimal Price { get; set; }
        }

      
    }
}
