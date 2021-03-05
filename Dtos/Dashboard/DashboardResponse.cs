using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Dtos.Dashboard
{
    public class DashboardResponse
    {
        public class StandartDashboardCard
        {
            public int WordCount { get; set; }
            public decimal Rate { get; set; }
            public int DateRange { get; set; }
        }

        public class Chart
        {
            public List<Dataset> Datasets { get; set; } = new List<Dataset>();
            public List<string> Labels { get; set; } = new List<string>();
            public int DateRange { get; set; }
        }

        public class Dataset
        {
            public int[] Data { get; set; }
            public string Label { get; set; }
        }

        public class LatestWords
        {
            public long Id { get; set; }
            public string Word { get; set; }
            public string Description { get; set; }
            public DateTime CreatedDate { get; set; }
            public byte Status { get; set; }
        }

        public class AllCards
        {
            public StandartDashboardCard TotalWords { get; set; } = new StandartDashboardCard();
            public StandartDashboardCard LearnedWords { get; set; } = new StandartDashboardCard();
            public StandartDashboardCard TotalSentences { get; set; } = new StandartDashboardCard();
            public decimal ProgressRate { get; set; }
            public Chart ChartData { get; set; } = new Chart();
            public List<LatestWords> LatestWords { get; set; } = new List<LatestWords>();
        }

    }
}
