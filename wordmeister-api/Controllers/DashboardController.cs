using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wordmeister_api.Dtos.Dashboard;
using wordmeister_api.Dtos.General;
using wordmeister_api.Helpers;
using wordmeister_api.Interfaces;
using static wordmeister_api.Helpers.Enums;

namespace wordmeister_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : Controller
    {
        private IDashboardService _dashboardService;

        ILogger<AccountController> _logger;
        public DashboardController(IDashboardService dashboardService, ILogger<AccountController> logger)
        {
            _logger = logger;
            _dashboardService = dashboardService;
        }

        [HttpPost("AllCards")]
        public IActionResult AllCards(DashboardRequest.AllCards model)
        {
            var data = _dashboardService.GetDashboard(User.GetUserId(), model);

            return Ok(new General.ResponseResult { Data = data });
        }

        [HttpGet("TotalWordsCard/{dateRange}")]
        public IActionResult TotalWordsCard(int dateRange)
        {
            var data = _dashboardService.GetTotalWordsCard(User.GetUserId(), (DateRange)dateRange);
            return Ok(new General.ResponseResult { Data = data });
        }

        [HttpGet("LearnedWordsCard/{dateRange}")]
        public IActionResult LearnedWordsCard(int dateRange)
        {
            var data = _dashboardService.GetLearnedWordsCard(User.GetUserId(), (DateRange)dateRange);
            return Ok(new General.ResponseResult { Data = data });
        }

        [HttpGet("TotalSentencesCard/{dateRange}")]
        public IActionResult TotalSentencesCard(int dateRange)
        {
            var data = _dashboardService.GetSentencesCard(User.GetUserId(), (DateRange)dateRange);
            return Ok(new General.ResponseResult { Data = data });
        }

        [HttpGet("ChartCard/{dateRange}")]
        public IActionResult ChartCard(int dateRange)
        {
            var data = _dashboardService.GetChartCard(User.GetUserId(), (DateRange)dateRange);
            return Ok(new General.ResponseResult { Data = data });
        }
    }
}
