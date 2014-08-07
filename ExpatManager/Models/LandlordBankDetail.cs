using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ExpatManager.Models
{
    public class LandlordBankDetail : ModifiedBaseClass
    {
        public int LandlordBankDetailID { get; set; }

        //public int AgentDetailID { get; set; }
        //public virtual AgentDetail AgentDetails { get; set; }
        public virtual AgentDetail AgentDetails { get; set; }

        [Display(Name = "Agent Detail")]
        public int AgentDetailID { get; set; }

        //public virtual AgentDetail AgentDetails { get; set; }

        //[Display(Name = "Agreement Detail")]
        //public int AgreementDetailID { get; set; }
        private string _AccountName;

        [Required(ErrorMessage = "Account name is required.")]
        [Column("AccountName")]
        [Display(Name = "Account Name")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string AccountName
        {
            get
            {
                if (string.IsNullOrEmpty(_AccountName))
                {
                    return _AccountName;
                }
                return _AccountName.ToUpper();
            }
            set { _AccountName = value; }
        }

        private string _BankName;

        [Required(ErrorMessage = "Bank name is required.")]
        [Column("BankName")]
        [Display(Name = "Bank Name")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string BankName
        {
            get
            {
                if (string.IsNullOrEmpty(_BankName))
                {
                    return _BankName;
                }
                return _BankName.ToUpper();
            }
            set { _BankName = value; }
        }

        //Changed to string as int was removing the left 0000
        [Required(ErrorMessage = "Bank account no is required.")]
        [Display(Name = "Bank Account No")]
        [MaxLength(10)]
        [StringLength(10, ErrorMessage = "Name cannot be longer than 10 characters.")] 
        public string BankAccountNo { get; set; }

        //Changed to string as int was removing the left 0000
        [Required(ErrorMessage = "Sort code is required.")]
        [Display(Name = "Sort Code")]
        [MaxLength(10)]
        [StringLength(10, ErrorMessage = "Name cannot be longer than 10 characters.")] 
        public string SortCode { get; set; }

        [Display(Name = "Agent and Bank Details")]
        public string ShortBankDetails
        {
            get
            {
                return Helper.StringExtensions.Left(AccountName,15) + "-" + BankAccountNo + "/" + SortCode;
            }
        }

        [Display(Name = "Agent and Bank Details")]
        public string LongBankDetails
        {
            get
            {
                return AccountName + "-" + AgentDetails.Address1 + "-" + BankAccountNo + "/" + SortCode + " - " + LandlordBankDetailID.ToString();
            }
        }
    }
}