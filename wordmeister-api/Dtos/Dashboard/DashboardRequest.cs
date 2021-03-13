using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static wordmeister_api.Helpers.Enums;

namespace wordmeister_api.Dtos.Dashboard
{
    public class DashboardRequest
    {
        public class AllCards
        {
            public DateRange TotalWords { get; set; }
            public DateRange LearnedWords { get; set; }
            public DateRange TotalSentences { get; set; }
            public DateRange ChartData { get; set; }
        }
    }
}
