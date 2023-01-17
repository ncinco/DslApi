using Dsl.Domain.Models;

namespace Dsl.Infrastructure.Services.Performance
{
    public interface ITestPerformanceService
    {
        Task StartAsync(uint count);

        TestPerformanceResultObject<Transaction> GetTestResultAsync();
    }
}