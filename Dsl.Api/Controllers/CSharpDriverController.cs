using Cassandra.Mapping;
using Dsl.Domain.Models;
using Dsl.Infrastructure.Services;
using Dsl.Infrastructure.Services.Performance;
using Microsoft.AspNetCore.Mvc;

namespace Dsl.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CSharpDriverController : ControllerBase
    {
        private readonly ILogger<CSharpDriverController> _logger;
        private readonly ICSharpDriverPerformanceService _csharpDriverPerformanceService;
        private readonly IMapper _mapper;

        public CSharpDriverController(ILogger<CSharpDriverController> logger, IMapper mapper, ICSharpDriverPerformanceService csharpDriverPerformanceService)
        {
            _logger = logger;
            _mapper = mapper;
            _csharpDriverPerformanceService = csharpDriverPerformanceService;
        }

        #region CSharpDriver
        [HttpGet("testconnection")]
        public List<Transaction> GetBigData()
        {
            var transactions = _mapper
                .Fetch<Transaction>("SELECT * FROM asb.transactions_by_account LIMIT 10")
                .ToList();

            return transactions;
        }

        [HttpPost("fileupload")]
        public async Task<string> FileUpload([FromForm] FileUploadRequest fileUploadRequest)
        {
            try
            {
                byte[] binaryImage;

                using (var stream = new MemoryStream())
                {
                    fileUploadRequest.FileDetails.CopyTo(stream);
                    binaryImage = stream.ToArray();
                }

                var fileUpload = new FileUpload
                {
                    Id = Guid.NewGuid(),
                    FileName = fileUploadRequest.FileDetails.FileName,
                    Image = binaryImage
                };

                await _mapper.InsertAsync(fileUpload);

                return fileUpload.FileName;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        [HttpGet("startperformance/{count}")]
        public async Task StartCSharpDriverPerformance(uint count = 1000)
        {
            await _csharpDriverPerformanceService.StartAsync(count);
        }

        [HttpGet("getstatus")]
        public async Task<TestPerformanceResultObject<Transaction>> GetCSharpDriverStatus()
        {
            return _csharpDriverPerformanceService.GetTestResultAsync();
        }
        #endregion
    }
}