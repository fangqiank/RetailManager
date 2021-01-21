using System;
using System.Configuration;

namespace RMWPFUi.Library.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        public decimal GetTaxRate()
        {
            string rateText = ConfigurationManager.AppSettings["taxRate"];

            bool IsValidTaxRate = decimal.TryParse(rateText,out decimal output);

            if (IsValidTaxRate == false)
            {
                throw new Exception("The tax rate is not set up properly");
            }

            return output;
        }
    }
}
