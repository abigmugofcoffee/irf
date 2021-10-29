using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using webszolgaltatasok_elerese.MnbReference;

namespace webszolgaltatasok_elerese
{
    public partial class Form1 : Form
    {
        List<MNBArfolyamServiceSoap> mnbservic = new List<MNBArfolyamServiceSoap>();
        GetExchangeRatesRequestBody reques = new GetExchangeRatesRequestBody();
        

        public Form1()
        {
            InitializeComponent();
            Webhivas();
        }

        void Webhivas()
        {
            var mnbservice = new MNBArfolyamServiceSoapClient();

            var request = new GetExchangeRatesRequestBody()
            { currencyNames = "EUR", startDate = "2020-01-01", endDate = "2020-06-30" };

            var response =mnbservice.GetExchangeRates(request);

            var result = response.GetExchangeRatesResult; ;
            
        }

    }
}
