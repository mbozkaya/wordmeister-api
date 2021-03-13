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
using wordmeister_api.Interfaces;
using wordmeister_api.Model;
using wordmeister_api.Services;

namespace wordmeister_api.Helpers
{
    public class NotificationJob : CronJobService
    {
        private WordmeisterContext _wordMeisterDbContext;
        private DateTime _jobTime;

        private readonly ILogger<NotificationJob> _logger;
        private IScheduleConfig<NotificationJob> _config;

        public NotificationJob(IScheduleConfig<NotificationJob> config, ILogger<NotificationJob> logger, IServiceScopeFactory serviceScopeFactory)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _config = config;
            using (var scope = serviceScopeFactory.CreateScope())
            {
                _wordMeisterDbContext = scope.ServiceProvider.GetService<WordmeisterContext>();
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 1 starts.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            Console.WriteLine($"{DateTime.Now:hh:mm:ss} CronJob 1 is working.");

            _jobTime = DateTime.Now;

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 1 is stopping.");
            return base.StopAsync(cancellationToken);
        }

        public void Start()
        {

            var currentNotifyUsers = GetNotificationUsers();

            foreach (var user in currentNotifyUsers)
            {
                var userWordSentences = GetUserNotNotifiedSentences(user.Id, _jobTime);

                var template = CreateTemplateForUser(userWordSentences, user);
                MailLog(userWordSentences);

                //TODO send mail to user
            }
        }

        private List<User> GetNotificationUsers()
        {
            return _wordMeisterDbContext.Users
                .Where(w =>
                w.UserSettings
                .Where(w2 => w2.UserSettingTypeId == (int)Enums.SettingType.MailNotification
                    && w2.Enable)
                    .Any()
                && w.UserInformations
                    .Where(w3 => w3.NotificationHour == _jobTime.Hour
                    && w3.NotificationMinute == _jobTime.Minute)
                    .Any()
            )
                .ToList();
        }

        private List<JobDto.UserSentencesByWord> GetUserNotNotifiedSentences(long userId, DateTime notifyDate)
        {
            return _wordMeisterDbContext.UserWords
                .Where(w => w.UserId == userId && w.CreatedDate.Date == notifyDate.Date)
                .GroupBy(g => g.Word)
                .Select(s => new JobDto.UserSentencesByWord
                {
                    Word = s.Key,
                    Sentences = s.Key.Sentences.Where(w => w.UserId == null || w.UserId == userId).ToList()
                })
                .AsNoTracking()
                .ToList();
        }

        private string CreateTemplateForUser(List<JobDto.UserSentencesByWord> wordSentences, User user)
        {
            string template = string.Empty;
            foreach (var wordSentence in wordSentences)
            {
                template += wordSentence.Word.Text;
            }

            return template;
        }

        private async void MailLog(List<JobDto.UserSentencesByWord> wordSentences)
        {

        }
    }
}
