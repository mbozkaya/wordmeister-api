using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wordmeister_api.Dtos.Dashboard;
using static wordmeister_api.Helpers.Enums;

namespace wordmeister_api.Interfaces
{
    public interface IDashboardService
    {
        DashboardResponse.StandartDashboardCard GetTotalWordsCard(int userId, DateRange dateRange = DateRange.LastDay);
        DashboardResponse.StandartDashboardCard GetLearnedWordsCard(int userId, DateRange dateRange = DateRange.LastDay);
        DashboardResponse.StandartDashboardCard GetSentencesCard(int userId, DateRange dateRange = DateRange.LastDay);
        decimal GetProgressRate(int userId);
        DashboardResponse.Chart GetChartCard(int userId, DateRange dateRange = DateRange.LastDay);
        DashboardResponse.AllCards GetDashboard(int userId, DashboardRequest.AllCards model);
    }
}
