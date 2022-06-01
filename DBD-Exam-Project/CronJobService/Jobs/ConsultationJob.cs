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
        private static bool _running = false;

        public ConsultationJob(IScheduleConfig<ConsultationJob> config, ILogger<ConsultationJob> logger, IConsultationCreationService consultationCreationService)
        : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _consultationCreationService = consultationCreationService;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ConsultationJob starts.");
            await base.StartAsync(cancellationToken);
            await DoWork(cancellationToken);
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            if (_running) return;
            try
            {
                _running = true;
                _logger.LogInformation($"{DateTime.Now:hh:mm:ss} ConsultationJob is working.");

                await Task.Run(_consultationCreationService.CreateNewConsultationOpenings, cancellationToken);
            }
            finally
            {
                _running = false;
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ConsultationJob is stopping.");
            await base.StopAsync(cancellationToken);
        }
    }
}