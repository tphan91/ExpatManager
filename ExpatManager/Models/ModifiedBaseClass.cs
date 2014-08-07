using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ExpatManager.Infrastructure;

namespace ExpatManager.Models
{
    public class ModifiedBaseClass : CreateBaseClass
    {
        [Display(Name = "Modified Date Time")]
        //[DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        //[Required(ErrorMessage = "Modified date time is required.")]
        [DataType(DataType.Date)]
        [DateRange(Min = "1901/01/01", Max = "2050/12/31")]
        public DateTime? ModifiedDateTime { get; set; }

        //[Required(ErrorMessage = "Modified by is required.")]

        [Column("ModifiedBy")]
        [Display(Name = "Modified By")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string ModifiedBy { get; set; }

        private bool isStatus = true;
        [Display(Name = "Active")]
        public bool Status { get { return this.isStatus; } set { this.isStatus = value; } }
    }
}