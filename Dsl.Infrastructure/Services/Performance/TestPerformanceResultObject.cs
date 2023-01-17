namespace Dsl.Infrastructure.Services.Performance
{
    public class TestPerformanceResultObject<T>
    {
        public TimeSpan ElapsedTime { get; set; }

        public uint RowCount { get; set; }

        public uint QueryCount { get; set; }
    }
}