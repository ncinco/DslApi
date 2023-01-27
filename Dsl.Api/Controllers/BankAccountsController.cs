using Dsl.Domain.Models;
using Dsl.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dsl.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BankAccountsController : ControllerBase
    {
        private readonly ILogger<StargateController> _logger;
        private readonly IRestSharpWrapperService _restSharpWrapperService;

        public BankAccountsController(ILogger<StargateController> logger, IRestSharpWrapperService restSharpWrapperService)
        {
            _logger = logger;
            _restSharpWrapperService = restSharpWrapperService;
        }

        [HttpGet("bank-accounts/{tokenisedCif}")]
        public async Task<BankAccounts> GetBankAccounts(Guid tokenisedCif)
        {
            return await _restSharpWrapperService.GetBankAccountsAsync(tokenisedCif);
        }
    }
}