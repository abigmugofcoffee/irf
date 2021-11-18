using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using webszolgaltatasok_elerese.MnbReference;

namespace webszolgaltatasok_elerese
{
    public partial class Form1 : Form
    {
        BindingList<RateData> Rates = new BindingList<RateData>();
        List<string> Currencies = new List<string>();

        public Form1()
        {
            InitializeComponent();
            GetCurrenices();
            RefreshData();

            dataGridView1.DataSource = Rates;
            chartRateData.DataSource = Rates;
        }

        void GetCurrenices()
        {
            var mnbService = new MNBArfolyamServiceSoapClient();
            var requestcurr = new GetCurrenciesRequestBody();
            var responsecurr = mnbService.GetCurrencies(requestcurr);
            var resultcurr = responsecurr.GetCurrenciesResult;
            var xml = new XmlDocument();
            xml.LoadXml(resultcurr);

            foreach (XmlElement element in xml.DocumentElement.ChildNodes[0])
            {
                if (element.ChildNodes[0] == null)
                    continue;
                string currency = element.InnerText;

                Currencies.Add(currency);
            }

            comboBox1.DataSource = Currencies;
        }

        private void RefreshData()
        {
            Rates.Clear();

            var result = Webhivas();
            if (result == null) return;
            Beolvasas(result);
            Diagram();
        }

        private string Webhivas()        
        {
            var mnbservice = new MNBArfolyamServiceSoapClient();

            string startdate = dateTimePicker1.Value.ToString();
            string enddate = dateTimePicker2.Value.ToString();
            string curr;
            if (comboBox1.SelectedItem.ToString() == "HUF")
            { return null; }

            curr = comboBox1.SelectedItem.ToString();
            var request = new GetExchangeRatesRequestBody()
            { currencyNames = curr, startDate = startdate, endDate = enddate };

            var response = mnbservice.GetExchangeRates(request);

            var result = response.GetExchangeRatesResult;
            return result;
        }

        private void Beolvasas(string result)
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
        void Diagram()
        {
            chartRateData.DataSource = Rates;

            var series = chartRateData.Series[0];
            series.ChartType = SeriesChartType.Line;

            series.XValueMember = "date";
            series.YValueMembers = "value";

            var legend = chartRateData.Legends[0];
            var chartArea = chartRateData.ChartAreas[0];

            series.BorderWidth = 2;
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.IsStartedFromZero = false;
            legend.Enabled = false;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
