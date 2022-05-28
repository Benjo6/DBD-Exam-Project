using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CronJobService.Services
{
    public class ConsultationJob : CronJobBase
    {
        private ILogger<ConsultationJob> _logger;
        private IConsultationCreationService _consultationCreationService;

        public ConsultationJob(IScheduleConfig<ConsultationJob> config, ILogger<ConsultationJob> logger, IConsultationCreationService consultationCreationService)
        : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _consultationCreationService = consultationCreationService;

        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ConsultationJob starts.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} ConsultationJob is working.");

            // TODO implement consultation creation

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ConsultationJob is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}