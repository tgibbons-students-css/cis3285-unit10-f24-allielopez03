using SingleResponsibilityPrinciple.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SingleResponsibilityPrinciple
{
    public class URLTradeDataProvider : ITradeDataProvider
    {
        private readonly string _url;
        private readonly ILogger _logger;

        public URLTradeDataProvider(string url, ILogger logger)
        {
            _url = url;
            _logger = logger;
        }

        public async Task<IEnumerable<string>> GetTradeData()
        {
            var tradeData = new List<string>();
            _logger.LogInfo("Reading trades from URL: " + _url);

            using (var client = new HttpClient())
            {
                HttpResponseMessage response;
                try
                {
                    response = await client.GetAsync(_url);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Failed to retrieve data from URL: {_url}. Exception: {ex.Message}");
                    // Return an empty list directly since it's an async method
                    return new List<string>(); // Simply return an empty list
                }

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Failed to retrieve data. Status code: {response.StatusCode}");
                    // Return an empty list if status code is not successful
                    return new List<string>(); // Simply return an empty list
                }

                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        tradeData.Add(line);
                    }
                }
            }

            // Return the tradeData list directly; no need to wrap it in Task.FromResult
            return tradeData;
        }
    }
}