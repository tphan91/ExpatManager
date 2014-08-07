using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ExpatManager.Infrastructure;

namespace ExpatManager.Models
{
    public class AgreementPayment : ModifiedBaseClass 
    {
        public int AgreementPaymentID { get; set; }

        private string _CostCode;

        [Required(ErrorMessage = "Cost code is required.")]
        [Column("CostCode")]
        [Display(Name = "Cost Code")]
        [MaxLength(4)]
        [StringLength(4, ErrorMessage = "Name cannot be longer than 5 characters.")]
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

        public virtual AgreementDetail AgreementDetail { get; set; }

        [Display(Name = "Agreement Detail")]
        public int AgreementDetailID { get; set; }

        //[DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Payment Date")]
        [Required(ErrorMessage = "Payment date is required.")]
        [DataType(DataType.Date)]
        [DateRange(Min = "1901/01/01", Max = "2030/12/31",SuppressDataTypeUpdate=true)]
        public DateTime PaymentDate { get; set; }

        //[DisplayFormat(DataFormatString = "{0:MMM}", ApplyFormatInEditMode = true)]
        //[Display(Name = "Payment Date")]
        //[Required(ErrorMessage = "Payment date is required.")]
        //[DataType(DataType.Date)]
        //[DateRange(Min = "1901/01/01", Max = "2030/12/31", SuppressDataTypeUpdate = true)]
        public String PaymentDateMonthYear
        {
            get
            {
                return String.Format("{0:MMM yyyy}",PaymentDate);
            }
        }

        [Required(ErrorMessage = "Monthly payment is required")]
        [Display(Name = "Monthly Payment")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:N2}")]
        public decimal MonthlyPayment { get; set; }

        [Required(ErrorMessage = "Pro rata payment is required")]
        [Display(Name = "Pro Rata Payment")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:N2}")]
        public decimal ProRataPayment { get; set; }

        [Required(ErrorMessage = "Total amount is required")]
        [Display(Name = "Total Amount")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:N2}")]
        public decimal TotalAmount { get; set; }

    }
}