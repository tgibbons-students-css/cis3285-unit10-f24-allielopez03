using SingleResponsibilityPrinciple.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleResponsibilityPrinciple
{
    public class AdjustTradeDataProvider : ITradeDataProvider
    {
        string url;
        ILogger logger;
        URLTradeDataProvider objectThing;

        public AdjustTradeDataProvider(string url, ILogger logger, URLTradeDataProvider objectThing)
        {
            this.url = url;
            this.logger = logger;
            this.objectThing = objectThing;
        }

        public IEnumerable<string> GetTradeData()
        {
            // Fetch the trade data from UrlTradeDataProvider
            IEnumerable<string> tradeData = objectThing.GetTradeData();

            logger.LogInfo("Converting 'GBP' to 'EUR' in trade data.");

            // Convert each instance of "GBP" to "EUR" in the data
            List<string> adjustedTradeData = new List<string>();
            foreach (var line in tradeData)
            {
                string adjustedLine = line.Replace("GBP", "EUR");
                adjustedTradeData.Add(adjustedLine);
            }

            return adjustedTradeData;
        }
    }
}
