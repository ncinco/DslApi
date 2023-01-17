using Cassandra.Mapping;
using Dsl.Domain.Models;

namespace Dsl.Infrastructure.Services.Performance
{
    public interface ICSharpDriverPerformanceService : ITestPerformanceService
    {
    }

    public class CSharpDriverPerformanceService : ICSharpDriverPerformanceService
    {
        private readonly IMapper _mapper;

        private TestPerformanceResultObject<Transaction> _result;

        public CSharpDriverPerformanceService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task StartAsync(uint count)
        {
            _result = new TestPerformanceResultObject<Transaction>();
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var taskList = new List<Task>();

            for (var i = 1; i <= count; i++)
            {
                var task = Task.Run(async () =>
                {
                    var transactions = await _mapper
                        .FetchAsync<Transaction>("SELECT * FROM asb.transactions_by_account LIMIT 20");

                    return transactions.ToList();
                });

                taskList.Add(task);
            }

            Task.WaitAll(taskList.ToArray());

            stopwatch.Stop();

            foreach (var task in taskList)
            {
                var result = ((Task<List<Transaction>>)task).Result;
                _result.RowCount += (uint)result.Count;
            }

            _result.QueryCount = count;
            _result.ElapsedTime = stopwatch.Elapsed;
        }

        public TestPerformanceResultObject<Transaction> GetTestResultAsync()
        {
            return _result;
        }
    }
}