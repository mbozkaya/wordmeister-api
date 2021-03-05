using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Model
{
    public class UserWordSetting : BaseModel
    {
        [ForeignKey("UserId")]
        public long UserId { get; set; }

        public bool IsIncludeLearned { get; set; }
        public bool IsIncludeFavorite { get; set; }
        public bool IsIncludePoint { get; set; }
        public decimal? Point { get; set; }
        public byte? ConditionType { get; set; }
        public string OrderBy { get; set; }
        public string Order { get; set; }
        public virtual User User { get; set; }
    }
}
