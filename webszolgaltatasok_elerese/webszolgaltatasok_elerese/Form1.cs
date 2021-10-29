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
using webszolgaltatasok_elerese.MnbReference;

namespace webszolgaltatasok_elerese
{
    public partial class Form1 : Form
    {
        BindingList<RateData> Rates = new BindingList<RateData>();

        public Form1()
        {
            InitializeComponent();

            var result = Webhivas();
            Beolvasas(result);

            dataGridView1.DataSource = Rates;
        }

        private string Webhivas()
        {
            var mnbservice = new MNBArfolyamServiceSoapClient();

            var request = new GetExchangeRatesRequestBody()
            { currencyNames = "EUR", startDate = "2020-01-01", endDate = "2020-06-30" };

            var response = mnbservice.GetExchangeRates(request);

            var result = response.GetExchangeRatesResult;
            return result;
            
        }

        void Beolvasas(string result)
        {
            var xml = new XmlDocument();
            xml.LoadXml(result);

            foreach (XmlElement element in xml.DocumentElement)
            {
                RateData rate = new RateData();
                Rates.Add(rate);


                rate.Date = Convert.ToDateTime(element.GetAttribute("date"));
                
                var childElement = (XmlElement)element.ChildNodes[0];
                rate.Currency = childElement.GetAttribute("curr");

                var unit = Convert.ToDecimal(childElement.GetAttribute("unit"));

                if (unit != 0)
                {
                    rate.Value = Convert.ToDecimal(childElement.InnerText) / unit;
                }
                else rate.Value = 0;
            }

        }

    }
}
