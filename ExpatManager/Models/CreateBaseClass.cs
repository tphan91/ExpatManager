using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ExpatManager.Infrastructure;

namespace ExpatManager.Models
{
    public class CreateBaseClass
    {
        //[DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Create Date Time")]
        [DataType(DataType.Date)]
        [DateRange(Min = "1901/01/01", Max = "2050/12/30",SuppressDataTypeUpdate=true)]
        public DateTime? CreateDateTime { get; set; }

        //[ScaffoldColumn(false)]
        //[Required(ErrorMessage = "Create by is required.")]
        [Column("CreateBy")]
        [Display(Name = "Create By")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")] 
        public string CreateBy { get; set; }
    }
}