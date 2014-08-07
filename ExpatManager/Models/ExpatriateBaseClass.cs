using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ExpatManager.Infrastructure;

namespace ExpatManager.Models
{
    public class ExpatriateBaseClass : ModifiedBaseClass
    {
        [Display(Name = "Expatriate")]
        public int ExpatriateID { get; set; }

        private string _CostCode;

        //[Required(ErrorMessage = "Cost code is required.")]
        [Column("CostCode")]
        [Display(Name = "Cost Code")]
        [MaxLength(4)]
        [StringLength(4, ErrorMessage = "Name cannot be longer than 4 characters.")]
        public string CostCode
        {
            get
            {
               if (string.IsNullOrEmpty(_CostCode))
                {
                   return _CostCode;
                }
                return _CostCode.ToUpper();
            }
            set { _CostCode = value; }
        }

        //Ceiling value derives from Family and JobGrade
        //[DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ceiling Value")]
        public decimal CeilingValue { get; set; }

        private string _Promotion;

        [DataType(DataType.MultilineText)]
        [Column("Promotion")]
        [Display(Name = "Reason For Promotion")]
        [MaxLength(500)]
        [StringLength(500, ErrorMessage = "Name cannot be longer than 500 characters.")]
        public string Promotion
        {
            get
            {
                if (string.IsNullOrEmpty(_Promotion))
                {
                    return _Promotion;
                }
                return _Promotion.ToUpper();
            }
            set { _Promotion = value; }
        }
        
        //[Required(ErrorMessage = "Date of promotion time is required.")]
        [DataType(DataType.Date)]
        [Column("DateOfPromotion")]
        [Display(Name = "Date Of Promotion")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        //[DateRange(Min = "2000/01/01", Max = "2040/12/31", SuppressDataTypeUpdate = true)]
        public DateTime? DateOfPromotion { get; set; }

        [Required]
        [Display(Name = "Title")]
        public virtual int TitleId { get { return (int)this.Title; } set { Title = (Enums.EnumTitle)value; } }

        [Display(Name = "Title")]
        [EnumDataType(typeof(Enums.EnumTitle))]
        public Enums.EnumTitle Title { get; set; }

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
    }
}