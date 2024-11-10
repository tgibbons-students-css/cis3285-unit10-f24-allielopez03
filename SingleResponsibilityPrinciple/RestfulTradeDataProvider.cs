using SingleResponsibilityPrinciple.Contracts;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SingleResponsibilityPrinciple
{
    public class RestfulTradeDataProvider : ITradeDataProvider
    {
        private readonly string _url;
        private readonly ILogger _logger;
        private readonly HttpClient _client = new HttpClient();

        public RestfulTradeDataProvider(string url, ILogger logger)
        {
            _url = url;
            _logger = logger;
        }

        // Asynchronous method to fetch trade data
        private async Task<List<string>> GetTradeAsync()
        {
            _logger.LogInfo("Connecting to the Restful server using HTTP");

            HttpResponseMessage response = await _client.GetAsync(_url);
            if (response.IsSuccessStatusCode)
            {
                // Read the content as a string and deserialize it into a List<string>
                string content = await response.Content.ReadAsStringAsync();
                var tradesString = JsonSerializer.Deserialize<List<string>>(content);
                _logger.LogInfo("Received trade strings of length = " + (tradesString?.Count ?? 0));
                return tradesString;
            }

            _logger.LogWarning("Failed to retrieve data. Status code: " + response.StatusCode);
            return null;
        }

        // Updated GetTradeData method
        public async Task<IEnumerable<string>> GetTradeData()
        {
            var trades = await GetTradeAsync();
            return trades ?? Task.FromResult((IEnumerable<string>)new List<string>()).Result;
        }
    }
}
