using CronJobService.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CronJobService.Service
{
    public class RenewalJob : CronJobBase
    {
        private ILogger<RenewalJob> _logger;
        private IRenewalService _renewalService;

        public RenewalJob(IScheduleConfig<RenewalJob> config, ILogger<RenewalJob> logger, IRenewalService renewalService)
        : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _renewalService = renewalService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob starts.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} CronJob is working.");

            _renewalService.NotifyRenewals();

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}