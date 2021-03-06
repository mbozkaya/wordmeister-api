﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Model
{
    public class MailUserWord
    {
        [Key]
        public long Id { get; set; }
        public int UserWordId { get; set; }
        public virtual UserWord UserWord { get; set; }
    }
}
