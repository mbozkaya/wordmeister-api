using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using wordmeister_api.Dtos.General;
using wordmeister_api.Entity;
using wordmeister_api.Helpers.Classes;
using wordmeister_api.Interfaces;
using wordmeister_api.Model;
using wordmeister_api.Services;

namespace wordmeister_api.Helpers
{
    public class NotificationJob : CronJobService
    {

        private readonly ILogger<NotificationJob> _logger;
        private IScheduleConfig<NotificationJob> _config;
        IServiceScopeFactory _serviceScopeFactory;

        public NotificationJob(IScheduleConfig<NotificationJob> config, ILogger<NotificationJob> logger, IServiceScopeFactory serviceScopeFactory)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _config = config;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 1 starts.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var notificationWork = scope.ServiceProvider.GetService<INotificationWork>();
                notificationWork.Start(DateTime.Now);
            }

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 1 is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
