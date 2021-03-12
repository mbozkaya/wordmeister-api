using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wordmeister_api.Dtos.Dashboard;
using wordmeister_api.Entity;
using wordmeister_api.Helpers;
using wordmeister_api.Interfaces;
using wordmeister_api.Model;
using static wordmeister_api.Helpers.Enums;

namespace wordmeister_api.Services
{
    public class DashboardService : IDashboardService
    {
        WordmeisterContext _dbContext;
        public DashboardService(WordmeisterContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DashboardResponse.StandartDashboardCard GetTotalWordsCard(int userId, DateRange dateRange = DateRange.LastDay)
        {
            var addedWordsByDate = _dbContext.UserWords
                .Where(w => w.UserId == userId && w.CreatedDate >= dateRange.GetDate())
                .Count();

            var compareDate = dateRange.GetCompareDate();

            var comparedWords = _dbContext.UserWords
                .Where(w => w.UserId == userId && w.CreatedDate > compareDate.end && w.CreatedDate <= compareDate.begin)
                .Count();

            return new DashboardResponse.StandartDashboardCard
            {
                DateRange = (int)dateRange,
                Rate = dateRange == DateRange.AllTime ? decimal.Zero : GetIncreasingRate(addedWordsByDate, comparedWords),
                WordCount = addedWordsByDate
            };
        }

        public DashboardResponse.StandartDashboardCard GetLearnedWordsCard(int userId, DateRange dateRange = DateRange.LastDay)
        {
            var learnedWords = _dbContext.UserWords
                .Where(w => w.UserId == userId && w.IsLearned && w.LearnedDate >= dateRange.GetDate())
                .Count();

            var compareDate = dateRange.GetCompareDate();

            var comparedWords = _dbContext.UserWords
                .Where(w => w.UserId == userId && w.IsLearned && w.LearnedDate > compareDate.end && w.LearnedDate <= compareDate.begin)
                .Count();

            return new DashboardResponse.StandartDashboardCard
            {
                DateRange = (int)dateRange,
                Rate = GetIncreasingRate(learnedWords, comparedWords),
                WordCount = learnedWords
            };
        }

        public DashboardResponse.StandartDashboardCard GetSentencesCard(int userId, DateRange dateRange = DateRange.LastDay)
        {
            var userSentences = _dbContext.UserWords
                .Where(w => w.UserId == userId && w.CreatedDate >= dateRange.GetDate() && w.MailUserWord != null)
                .Select(s => s.Word.Sentences.Where(w => w.UserId == null || w.UserId == userId).Count())
                .FirstOrDefault();

            var compareDate = dateRange.GetCompareDate();

            var compareSentences = _dbContext.UserWords
                .Where(w => w.UserId == userId && w.CreatedDate > compareDate.end && w.CreatedDate <= compareDate.begin && w.MailUserWord != null)
                .Select(s => s.Word.Sentences.Where(w => w.UserId == null || w.UserId == userId).Count())
                .FirstOrDefault();

            return new DashboardResponse.StandartDashboardCard
            {
                DateRange = (int)dateRange,
                Rate = GetIncreasingRate(userSentences, compareSentences),
                WordCount = userSentences
            };
        }

        public decimal GetProgressRate(int userId)
        {
            int userWordCount = _dbContext.UserWords.Where(w => w.UserId == userId).Count();
            int userLearnedWordCount = _dbContext.UserWords.Where(w => w.UserId == userId && w.IsLearned).Count();

            return userWordCount == 0 ? decimal.Zero : (decimal)userLearnedWordCount / userWordCount;
        }

        public DashboardResponse.AllCards GetDashboard(int userId, DashboardRequest.AllCards model)
        {
            DashboardResponse.AllCards response = new DashboardResponse.AllCards();

            response.ChartData = GetChartCard(userId, model.ChartData);
            response.LearnedWords = GetLearnedWordsCard(userId, model.LearnedWords);
            response.ProgressRate = GetProgressRate(userId);
            response.TotalSentences = GetSentencesCard(userId, model.TotalSentences);
            response.TotalWords = GetTotalWordsCard(userId, model.TotalWords);
            response.LatestWords = GetLatestWords(userId);

            return response;
        }

        public DashboardResponse.Chart GetChartCard(int userId, DateRange dateRange = DateRange.LastDay)
        {
            var endDate = dateRange.GetDate();

            var wholeUserWords = _dbContext.UserWords
                .Where(w => w.UserId == userId && w.CreatedDate >= endDate)
                .OrderBy(o => o.CreatedDate)
                .ToList();

            var firstCreatedDate = wholeUserWords.Count > 0 ? wholeUserWords.FirstOrDefault().CreatedDate : DateTime.Today;

            DashboardResponse.Chart chartResponse = new DashboardResponse.Chart();

            switch (dateRange)
            {
                case DateRange.LastDay:
                    break;
                case DateRange.LastWeek:
                    chartResponse = GetLastWeekOrMonthChartData(wholeUserWords, endDate);
                    break;
                case DateRange.LastMonth:
                    chartResponse = GetLastWeekOrMonthChartData(wholeUserWords, endDate);
                    break;
                case DateRange.LastSixMonth:
                    chartResponse = GetLastSixMonthsCharData(wholeUserWords, endDate);
                    break;
                case DateRange.AllTime:
                    chartResponse = GetAllTimeChartData(wholeUserWords, firstCreatedDate);
                    break;
                default:
                    break;
            }
            chartResponse.Datasets[0].Label = "Added Words";
            chartResponse.Datasets[1].Label = "Learned Words";

            chartResponse.DateRange = (int)dateRange;
            return chartResponse;
        }

        public List<DashboardResponse.LatestWords> GetLatestWords(int userId)
        {
            return _dbContext.UserWords
                .Where(w => w.UserId == userId)
                .Include(i => i.MailUserWord)
                .Select(s => new DashboardResponse.LatestWords
                {
                    CreatedDate = s.CreatedDate,
                    Description = s.Description,
                    Id = s.Id,
                    Word = s.Word.Text,
                    Status = s.MailUserWord != null
                })
                .OrderByDescending(o => o.CreatedDate)
            .Take(6)
            .ToList();
        }

        private DashboardResponse.Chart GetLastWeekOrMonthChartData(List<UserWord> userWords, DateTime endDate)
        {
            DashboardResponse.Chart chart = new DashboardResponse.Chart();
            List<string> labels = new List<string>();

            DashboardResponse.Dataset createdWords = new DashboardResponse.Dataset();
            DashboardResponse.Dataset learnedWords = new DashboardResponse.Dataset();
            DateTime date = DateTime.Today;
            int dateEject = (int)(date - endDate).TotalDays;
            createdWords.Data = new int[dateEject + 1];
            learnedWords.Data = new int[dateEject + 1];

            for (int i = 0; i <= dateEject; i++)
            {
                createdWords.Data[i] = userWords.Where(w => w.CreatedDate.Date == endDate.Date).Count();
                learnedWords.Data[i] = userWords.Where(w => w.IsLearned && w.LearnedDate != null && w.LearnedDate.Value.Date == endDate.Date).Count();
                labels.Add(endDate.ToString("m"));

                endDate = endDate.AddDays(1);
            }
            chart.Datasets.Add(createdWords);
            chart.Datasets.Add(learnedWords);
            chart.Labels = labels;


            return chart;
        }

        private DashboardResponse.Chart GetLastSixMonthsCharData(List<UserWord> userWords, DateTime endDate)
        {
            DashboardResponse.Chart chart = new DashboardResponse.Chart();
            List<string> labels = new List<string>();

            DashboardResponse.Dataset createdWords = new DashboardResponse.Dataset();
            DashboardResponse.Dataset learnedWords = new DashboardResponse.Dataset();
            int sixMonth = 6;
            createdWords.Data = new int[sixMonth];
            learnedWords.Data = new int[sixMonth];
            var dateCount = (DateTime.Today.Date - endDate.Date).TotalDays;
            var format = string.Empty;
            if (dateCount / 30 >= 5)
            {
                format = "MMM";
            }
            else
            {
                format = "m";
            }


            for (int i = 0; i < sixMonth; i++)
            {
                DateTime currentMonth = endDate.AddMonths(i);
                DateTime nextMonth = currentMonth.AddMonths(1);

                createdWords.Data[i] = userWords.Where(w => w.CreatedDate.Date > currentMonth.Date && w.CreatedDate.Date <= nextMonth.Date).Count();
                learnedWords.Data[i] = userWords.Where(w => w.IsLearned && w.LearnedDate != null && w.LearnedDate.Value.Date >= currentMonth.Date && w.LearnedDate < nextMonth.Date).Count();

                labels.Add(currentMonth.ToString(format));

            }

            chart.Datasets.Add(createdWords);
            chart.Datasets.Add(learnedWords);
            chart.Labels = labels;
            return chart;
        }

        private DashboardResponse.Chart GetAllTimeChartData(List<UserWord> userWords, DateTime endDate)
        {
            DashboardResponse.Chart chart = new DashboardResponse.Chart();
            List<string> labels = new List<string>();

            DashboardResponse.Dataset createdWords = new DashboardResponse.Dataset();
            DashboardResponse.Dataset learnedWords = new DashboardResponse.Dataset();
            DateTime today = DateTime.Today;

            var dateCount = (today.Date - endDate.Date).TotalDays;
            if (dateCount < 6)
            {
                return GetLastWeekOrMonthChartData(userWords, endDate);
            }
            createdWords.Data = new int[6];
            learnedWords.Data = new int[6];
            var format = string.Empty;
            if (dateCount / 365 > 3)
            {
                format = "Y";
            }
            else if (dateCount / 30 >= 6)
            {
                format = "MMM";
            }
            else
            {
                format = "m";
            }

            for (int i = 0; i < 6; i++)
            {
                int addedDays = (int)dateCount / 6;

                if (i == 6)
                {
                    addedDays += (int)dateCount % 6;
                }

                DateTime currentDate = endDate.AddDays(addedDays * i);
                DateTime nextDate = currentDate.AddDays(addedDays);

                createdWords.Data[i] = userWords.Where(w => w.CreatedDate.Date >= currentDate.Date && w.CreatedDate.Date < nextDate.Date).Count();
                learnedWords.Data[i] = userWords.Where(w => w.IsLearned && w.LearnedDate != null && w.LearnedDate.Value.Date >= currentDate.Date && w.LearnedDate < nextDate.Date).Count();

                labels.Add(currentDate.ToString(format));
            }

            chart.Datasets.Add(createdWords);
            chart.Datasets.Add(learnedWords);
            chart.Labels = labels;

            return chart;
        }

        private decimal GetIncreasingRate(int currentValue, int pastValue)
        {
            if (pastValue == 0)
            {
                return decimal.Zero;
            }

            return (decimal)currentValue / pastValue - 1;
        }
    }
}
