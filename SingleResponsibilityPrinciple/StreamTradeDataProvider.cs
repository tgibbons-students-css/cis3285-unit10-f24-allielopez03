using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SingleResponsibilityPrinciple.Contracts;

namespace SingleResponsibilityPrinciple
{
    public class StreamTradeDataProvider : ITradeDataProvider
    {
        private readonly Stream _stream;
        private readonly ILogger _logger;

        public StreamTradeDataProvider(Stream stream, ILogger logger)
        {
            _stream = stream;
            _logger = logger;
        }

        public Task<IEnumerable<string>> GetTradeData()
        {
            var tradeData = new List<string>();
            _logger.LogInfo("Reading trades from file stream.");

            using (var reader = new StreamReader(_stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    tradeData.Add(line);
                }
            }

            // Use Task.FromResult to wrap the result in a Task
            return Task.FromResult((IEnumerable<string>)tradeData);
        }
    }
}