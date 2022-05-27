using CronJobService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CronJobService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RenewalController : ControllerBase
    {
        private readonly ILogger<RenewalController> _logger;
        private readonly IRenewalService _renewalService;

        public RenewalController(ILogger<RenewalController> logger,IRenewalService renewalService)
        {
            _logger = logger;
            _renewalService = renewalService;

        }

        [HttpGet(Name ="TriggerRenewals")]
        public Task Get()
        {
            _renewalService.NotifyRenewals();
            return Task.CompletedTask;
        }
    }
}
