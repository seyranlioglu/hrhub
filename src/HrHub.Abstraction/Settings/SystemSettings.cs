using HrHub.Abstraction.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Abstraction.Settings
{
    [AppSetting("SystemSettings")]
    public class SystemSettings : ISettingsBase
    {
        public string TrxDateFormat { get; set; }
        public string IssuerTrxDateFormat { get; set; }
        public string CardAcceptorTerminalId { get; set; }
        public string CardAcceptorAcqId { get; set; }
        public string MerchantName { get; set; }
        public string MerchantCityZipCode { get; set; }
        public string MerchantCityName { get; set; }
        public string MerchantStateCode { get; set; }
        public string MerchantCountryCode { get; set; }
        public int CurrencyCode { get; set; }
        public string AcqInsIdCode { get; set; }
        public string LocalTxnDateFormat { get; set; }
        public string LocalTxnTimeFormat { get; set; }
        public int MaxSocketCount { get; set; }

        public string CardAcceptorName
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(MerchantName);
                sb.Append(' ');
                sb.Append(MerchantCityZipCode);
                sb.Append(' ');
                sb.Append(MerchantCityName);
                sb.Append(' ');
                sb.Append(MerchantStateCode);
                return sb.ToString();
            }
        }
    }
}
/*
  	"TrxDateFormat": "yyMMddhhmmss",
    "IssuerTrxDateFormat": "MMDDhhmmss",
    "CardAcceptorTerminalId":"55559",
    "CardAcceptorAcqId":"034200000026047",
    "MerchantName": "",
    "MerchantCityZipCode": "",
    "MerchantCityName": "",
    "MerchantsState": ""
 */