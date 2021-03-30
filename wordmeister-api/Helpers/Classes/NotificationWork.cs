using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wordmeister_api.Dtos.General;
using wordmeister_api.Entity;
using wordmeister_api.Interfaces;
using wordmeister_api.Model;

namespace wordmeister_api.Helpers.Classes
{
    public class NotificationWork : INotificationWork
    {
        WordmeisterContext _wordMeisterDbContext;
        DateTime _time = DateTime.Now;
        private readonly ILogger<NotificationJob> _logger;
        private INotification _mailHelper;

        public NotificationWork(WordmeisterContext wordmeisterContext, ILogger<NotificationJob> logger, INotification mailHelper)
        {
            _wordMeisterDbContext = wordmeisterContext;
            _logger = logger;
            _mailHelper = mailHelper;

        }


        public void Start(DateTime time)
        {
            try
            {
                _time = time;

                var currentNotifyUsers = GetNotificationUsers();

                foreach (var user in currentNotifyUsers)
                {
                    var userWordSentences = GetUserNotNotifiedSentences(user.Id, _time);

                    if (userWordSentences.Count == 0)
                    {
                        continue;
                    }

                    var template = CreateTemplateForUser(userWordSentences, user);

                    _mailHelper.Message(template, "", user.Email);

                    var mailLogId = MailLog(user.Id);

                    UserWordLog(userWordSentences, mailLogId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occured during notification job working at {_time:hh:mm:ss}.");
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
                    .Where(w3 => w3.NotificationHour == _time.Hour
                    && w3.NotificationMinute == _time.Minute)
                    .Any()
            )
                .ToList();
        }

        private List<JobDto.UserSentencesByWord> GetUserNotNotifiedSentences(long userId, DateTime notifyDate)
        {
            return _wordMeisterDbContext.UserWords
                .Where(w => w.UserId == userId && w.CreatedDate.Date == notifyDate.Date)
                .Select(s => new JobDto.UserSentencesByWord
                {
                    UserWordId = s.Id,
                    Word = s.Word,
                    Sentences = s.Word.Sentences.Where(w => w.UserId == null || w.UserId == userId).ToList()
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

        private long MailLog(long userId)
        {
            var mailLog = new MailLog
            {
                CreatedDate = _time,
                UserId = userId,
            };
            _wordMeisterDbContext.MailLogs.Add(mailLog);

            return mailLog.Id;
        }

        private void UserWordLog(List<JobDto.UserSentencesByWord> wordSentences, long mailLogId)
        {
            foreach (var sentence in wordSentences)
            {
                _wordMeisterDbContext.MailUserWords.Add(new Model.MailUserWord
                {
                    MailLogId = mailLogId,
                    UserWordId = sentence.UserWordId,
                });
            }
            _wordMeisterDbContext.SaveChanges();
        }
    }
}
