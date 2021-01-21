using System;
using System.Configuration;

namespace RMDataManager.Library
{
    public class ConfigHelper
    {
        public static decimal GetTaxRate()
        {
            //Todo Move this from config to API
            string rateText = ConfigurationManager.AppSettings["taxRate"];

            bool IsValidTaxRate = decimal.TryParse(rateText, out decimal output);

            if (IsValidTaxRate == false)
            {
                throw new Exception("The tax rate is not set up properly");
            }

            return output;
        }
    }
}
