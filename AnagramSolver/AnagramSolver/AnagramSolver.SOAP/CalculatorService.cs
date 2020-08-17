using ServiceReference1;
using System.Threading.Tasks;
using static ServiceReference1.CalculatorSoapClient;

namespace AnagramSolver.SOAP
{
    public class CalculatorService
    {
        private readonly CalculatorSoapClient _client;

        public CalculatorService()
        {
            _client = new CalculatorSoapClient(EndpointConfiguration.CalculatorSoap);
        }

        public async Task<int> Add(int value1, int value2)
        {
            await _client.OpenAsync();
            var result = await _client.AddAsync(value1, value2);
            await _client.CloseAsync();
            return result;
        }

        public async Task<int> Substract(int value1, int value2)
        {
            await _client.OpenAsync();
            var result = await _client.SubtractAsync(value1, value2);
            await _client.CloseAsync();
            return result;
        }

        public async Task<int> Divide(int value1, int value2)
        {
            await _client.OpenAsync();
            var result = await _client.DivideAsync(value1, value2);
            await _client.CloseAsync();
            return result;
        }

        public async Task<int> Multiply(int value1, int value2)
        {
            await _client.OpenAsync();
            var result = await _client.MultiplyAsync(value1, value2);
            await _client.CloseAsync();
            return result;
        }
    }
}
