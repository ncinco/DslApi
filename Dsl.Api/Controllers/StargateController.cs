using Cassandra.Mapping;
using Dsl.Domain.Models;
using Dsl.Infrastructure.Services;
using Dsl.Infrastructure.Services.Performance;
using Microsoft.AspNetCore.Mvc;

namespace Dsl.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StargateController : ControllerBase
    {
        private readonly ILogger<StargateController> _logger;
        private readonly IStargatePerformanceService _stargatePerformanceService;
        private readonly IRestSharpWrapperService _restSharpWrapperService;

        public StargateController(ILogger<StargateController> logger, IStargatePerformanceService stargatePerformanceService, IRestSharpWrapperService restSharpWrapperService)
        {
            _logger = logger;
            _stargatePerformanceService = stargatePerformanceService;
            _restSharpWrapperService = restSharpWrapperService;
        }

        #region Stargate
        [HttpGet("testconnection")]
        public async Task<RestResponseResult<Transaction>> TestConnection()
        {
            return await _restSharpWrapperService.GetStargateAsync();

        }
        
        [HttpGet("startperformance/{count}")]
        public async Task StartStartgatePerformance(uint count = 1000)
        {
            await _stargatePerformanceService.StartAsync(count);
        }

        [HttpGet("getstatus")]
        public async Task<TestPerformanceResultObject<Transaction>> GetStargateStatus()
        {
            return _stargatePerformanceService.GetTestResultAsync();
        }
        #endregion
    }
}