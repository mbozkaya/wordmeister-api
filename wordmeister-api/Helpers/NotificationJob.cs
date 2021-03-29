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
        private WordmeisterContext _wordMeisterDbContext;

        private readonly ILogger<NotificationJob> _logger;
        private IScheduleConfig<NotificationJob> _config;
        private Mail mailHelper; 

        public NotificationJob(IScheduleConfig<NotificationJob> config, ILogger<NotificationJob> logger, IServiceScopeFactory serviceScopeFactory)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _config = config;

            using (var scope = serviceScopeFactory.CreateScope())
            {
                _wordMeisterDbContext = scope.ServiceProvider.GetService<WordmeisterContext>();
                mailHelper = scope.ServiceProvider.GetService<Helpers.Classes.Mail>();
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 1 starts.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            Task.Run(() => { new NotificationWork(_wordMeisterDbContext, DateTime.Now, _logger, mailHelper); });

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 1 is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
