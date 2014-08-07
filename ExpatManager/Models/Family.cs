using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ExpatManager.Infrastructure;

namespace ExpatManager.Models
{
    public class Family : ModifiedBaseClass
    {
        [Key]
        public int FamilyID { get; set; }

        [Required]
        [Display(Name = "Family Type")]
        public virtual int FamilyTypeId { get { return (int)this.FamilyType; } set { FamilyType = (Enums.EnumFamilyType)value; } }

        [Display(Name = "Family Type")]
        [EnumDataType(typeof(Enums.EnumFamilyType))]
        public Enums.EnumFamilyType FamilyType { get; set; }

        [Display(Name = "Arrive Date")]
        //[DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DateRange(Min = "1904/01/01", Max = "2030/12/31", SuppressDataTypeUpdate = true)]
        public DateTime? ArriveDate { get; set; }

        [Display(Name = "Leave Date")]
        //[DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DateRange(Min = "1905/01/01", Max = "2030/12/31", SuppressDataTypeUpdate = true)]
        public DateTime? LeaveDate { get; set; }

        [Display(Name = "Date Of Birth")]
        //[DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DateRange(Min = "1906/01/01", Max = "2030/12/31", SuppressDataTypeUpdate = true)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Expatriate")]
        public virtual Expatriate Expatriates { get; set; }

        [Display(Name = "Expatriate")]
        public int ExpatriateID { get; set; }

        //public ICollection<TTargetEntity> Familys { get; set; }
    }
}