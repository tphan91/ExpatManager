using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ExpatManager.Infrastructure;
using ExpatManager.Models;

namespace ExpatManager.Models
{
    public class ExpatriateHistory : ExpatriateBaseClass
    {
        public int ExpatriateHistoryID { get; set; }

        [Display(Name = "Expatriate")]
        public virtual Expatriate Expatriates { get; set; }
    }
}