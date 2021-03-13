using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Model
{
    public class UploadFile : BaseModel
    {
        public Guid Guid { get; set; }
        public string Extension { get; set; }
        public string OriginalName { get; set; }
        public string Uri { get; set; }
        public string Description { get; set; }
        [ForeignKey("UserId")]
        public long UserId { get; set; }
        public int Type { get; set; }
        public virtual User User { get; set; }
        public bool Status { get; set; }
        public bool IsSelected { get; set; }
    }
}
