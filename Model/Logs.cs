using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Model
{
    public class Logs
    {
        [Key]
        public long Id { get; set; }
        [Column(TypeName = "text")]
        public string Message { get; set; }
        [Column(TypeName = "text")]
        public string MessageTemplate { get; set; }
        [Column(TypeName = "varchar")]
        public string Level { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime RaiseDate { get; set; }
        [Column(TypeName = "text")]
        public string Exception { get; set; }
        [Column(TypeName = "jsonb")]
        public string Properties { get; set; }
        [Column(TypeName = "jsonb")]
        public string PropsTest { get; set; }
        [Column(TypeName = "text")]
        public string MachineName { get; set; }
    }
}
