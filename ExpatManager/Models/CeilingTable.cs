using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ExpatManager.Models
{
    public class CeilingTable : ModifiedBaseClass
    {
        [Key]
        public int CeilingTableID { get; set; }

        //Used to help with the sort order
        //[Column("JobLevel", TypeName = "int")]
        //[Display(Name = "Job Level")]
        //public int JobLevel { get; set; }

        [Required]
        [Display(Name = "Job Grade")]
        public virtual int JobGradeId { get { return (int)this.JobGrade; } set { JobGrade = (Enums.EnumJobGrade)value; } }

        [Display(Name = "Job Grade")]
        [EnumDataType(typeof(Enums.EnumJobGrade))]
        public Enums.EnumJobGrade JobGrade { get; set; }

        [Required]
        [Display(Name = "Family Status")]
        public virtual int FamilyStatusId { get { return (int)this.FamilyStatus; } set { FamilyStatus = (Enums.EnumFamilyStatus)value; } }

        [Display(Name = "Family Status")]
        [EnumDataType(typeof(Enums.EnumFamilyStatus))]
        public Enums.EnumFamilyStatus FamilyStatus { get; set; }

        [Display(Name = "Ceiling Value")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:N2}")]
        public decimal CeilingValue { get; set; }
    }
}