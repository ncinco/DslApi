using Dsl.Domain.Models;

namespace Dsl.Infrastructure.Services.Performance
{
    public interface IStargatePerformanceService : ITestPerformanceService
    {
    }

    public class StargatePerformanceService : IStargatePerformanceService
    {
        private readonly IRestSharpWrapperService _restSharpWrapper;

        private TestPerformanceResultObject<Transaction> _result;

        public StargatePerformanceService(IRestSharpWrapperService restSharpWrapper)
        {
            _restSharpWrapper = restSharpWrapper;
        }

        public async Task StartAsync(uint count)
        {
            _result = new TestPerformanceResultObject<Transaction>();
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var taskList = new List<Task>();

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Cassandra-Token", "AstraCS:AZjhNqTMgAilAZrCDBGejmtg:32372b11a3dd57f6e99092c8219939460084adfb5e1bd640309d14168a5bfd79");

            var queryParameters = new Dictionary<string, string>();
            queryParameters.Add("page-size", "20");
            var dictFormUrlEncoded = new FormUrlEncodedContent(queryParameters);
            var queryString = await dictFormUrlEncoded.ReadAsStringAsync();

            for (var i = 1; i <= count; i++)
            {
                var task = Task.Run(async () => {
                    var response = await client.GetAsync($"https://0b743e1f-1d8d-476d-a769-a3f511e7edd3-australiaeast.apps.astra.datastax.com/api/rest/v2/keyspaces/asb/transactions_by_account/rows?{queryString}");

                    var jsonString = await response.Content.ReadAsStringAsync();

                    var transactions = Newtonsoft.Json.JsonConvert.DeserializeObject<RestResponseResult<Transaction>>(jsonString);

                    return transactions;
                });

                taskList.Add(task);
            }

            Task.WaitAll(taskList.ToArray());

            stopwatch.Stop();

            foreach (var task in taskList)
            {
                var result = ((Task<RestResponseResult<Transaction>>)task).Result;
                _result.RowCount += (uint)result.Data.Count;
            }

            _result.ElapsedTime = stopwatch.Elapsed;
        }

        public TestPerformanceResultObject<Transaction> GetTestResultAsync()
        {
            return _result;
        }
    }
}