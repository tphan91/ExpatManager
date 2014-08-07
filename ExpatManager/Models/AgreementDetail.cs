using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ExpatManager.Infrastructure;
using System.Web.Mvc;

namespace ExpatManager.Models
{
    public class AgreementDetail : ContactBaseClass
    {
        public int AgreementDetailID { get; set; }

        [Display(Name = "Agreement No")]
        public int AgreementNo { get; set; }

        [Display(Name = "Start Date")]
        //[DateRange(SuppressDataTypeUpdate = true)]
        [DateRange(Min = "2000/01/01", Max = "2030/12/31")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Start date is required.")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Expire Date")]
        //[DateRange(SuppressDataTypeUpdate = true)]
        [DateRange(Min = "2000/01/01", Max = "2030/12/31")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Expire date is required.")]
        [DataType(DataType.Date)]
        public DateTime ExpireDate { get; set; }

        [Display(Name = "Length In Months")]
        [Required(ErrorMessage = "Length in months is required.")]
        public int Length { get; set; }

        private string _CostCode;

        [Required(ErrorMessage = "Cost code is required.")]
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

        //[Required(ErrorMessage = "Monthly payment is required")]
        [Display(Name = "Monthly Payment")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:N2}")]
        public decimal MonthlyPayment { get; set; }

        [Required(ErrorMessage = "Pro rata payment is required")]
        [Display(Name = "Pro Rata Payment")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:N2}")]
        public decimal ProRataPayment { get; set; }

        //[Required(ErrorMessage = "Total amount is required")]
        [Display(Name = "Total Amount")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:N2}")]
        public decimal TotalAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [DateRange(Min = "2000/01/01", Max = "2030/12/31")]
        [Required(ErrorMessage = "Pro rata payment Date is required.")]
        [Display(Name = "Pro Rata Date")]
        [DataType(DataType.Date)]
        public DateTime ProRataPaymentDate { get; set; }

        private string _Comment;

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comment")]
        [MaxLength(1000)]
        [StringLength(1000, ErrorMessage = "Name cannot be longer than 1000 characters.")]
        public string Comment
        {
            get
            {
                if (string.IsNullOrEmpty(_Comment))
                {
                    return _Comment;
                }
                return _Comment.ToUpper();
            }
            set { _Comment = value; }
        }

        [Display(Name = "Desposit Amount")]
        public decimal? DepositAmount { get; set; }

        [Display(Name = "Show Create Link")]
        public Boolean ShowPaymentCreateLink { get; set; }

        [Display(Name = "Show Create Link")]
        public Boolean ShowBankCreateLink { get; set; }
        //public virtual ICollection<LandlordBankDetail> LandlordBankDetails { get; set; }

        [Display(Name = "Ceiling Breach")]
        public decimal CeilingBreach { get; set; }

        [Display(Name = "Agent Detail")]
        public int? AgentDetailID { get; set; }

        [Display(Name = "Agent Detail")]
        public virtual AgentDetail AgentDetails { get; set; }

        public IEnumerable<SelectListItem> AgentList { get; set; }

        [Display(Name = "Agreement Payment")]
        public virtual ICollection<AgreementPayment> AgreementPayments { get; set; }

        [Display(Name = "Document Upload")]
        public virtual ICollection<AgreementDetailDocumentUpload> AgreementDetailDocumentUploads { get; set; }

        [Display(Name = "Bank Details")]
        public int? LandlordBankDetailID { get; set; }

        [Display(Name = "Bank Details")]
        public virtual LandlordBankDetail LandlordBankDetails { get; set; }

        public string LandlordBankAutoComplete
        {
            get
            {
                return LandlordBankDetails.AccountName + ", " + LandlordBankDetails.AgentDetails.Address1 + ", " + LandlordBankDetails.BankAccountNo + "/" + LandlordBankDetails.SortCode + " - " + LandlordBankDetails.LandlordBankDetailID.ToString();
            }
        }

        [Display(Name = "Expatriate")]
        public int ExpatriateID { get; set; }

        public virtual Expatriate Expatriates { get; set; }

        [Required(ErrorMessage = "Reference is required.")]
        [Column("Reference")]
        [Display(Name = "Reference")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Reference { get; set; }

        //Council Tax information
        [Display(Name = "Council Name")]
        [MaxLength(100)]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string CouncilName { get; set; }

        [Display(Name = "Council Tax Amount")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:N2}")]
        public decimal? CouncilTaxAmount { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Council Tax Comment")]
        [MaxLength(1000)]
        [StringLength(1000, ErrorMessage = "Council Tax Comment cannot be longer than 1000 characters.")]
        public string CouncilTaxComment { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Sent To AP")]
        [DataType(DataType.Date)]
        public DateTime? DateSentToAP { get; set; }

        [Column("CouncilTaxAccountReference")]
        [Display(Name = "Account Reference")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "Council Tax Account Reference cannot be longer than 50 characters.")]
        public string CouncilTaxAccountReference { get; set; }

        [Column("AccountAuthorisation")]
        [Display(Name = "Account Authorisation")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "Account Authorisation cannot be longer than 50 characters.")]
        public string AccountAuthorisation { get; set; }

        //Council Tax information
        [Display(Name = "Council Name")]
        [MaxLength(100)]
        [StringLength(100, ErrorMessage = "Council Name Pro Rata cannot be longer than 100 characters.")]
        public string CouncilNameProRata { get; set; }

        [Display(Name = "Council Tax Amount")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:N2}")]
        public decimal? CouncilTaxAmountProRata { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Council Tax Comment")]
        [MaxLength(1000)]
        [StringLength(1000, ErrorMessage = "Council Tax Comment Pro Rata cannot be longer than 1000 characters.")]
        public string CouncilTaxCommentProRata { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Sent To AP")]
        [DataType(DataType.Date)]
        public DateTime? DateSentToAPProRata { get; set; }

        [Column("CouncilTaxAccountReferenceProRata")]
        [Display(Name = "Account Reference")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "Council Tax Account Reference Pro Rata cannot be longer than 50 characters.")]
        public string CouncilTaxAccountReferenceProRata { get; set; }

        [Column("AccountAuthorisationProRata")]
        [Display(Name = "Account Authorisation")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "Account Authorisation Pro Rata cannot be longer than 50 characters.")]
        public string AccountAuthorisationProRata { get; set; }
        //--------------------------------------------

        [Column("RefundReason")]
        [Display(Name = "Refund Reason")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "Refund Reason cannot be longer than 50 characters.")]
        public string RefundReason { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Council Contacted Date")]
        [DataType(DataType.Date)]
        public DateTime? CouncilContactedDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Refund Date")]
        [DataType(DataType.Date)]
        public DateTime? RefundDate { get; set; }

        [Display(Name = "Refund Amount")]
        [DataType(DataType.Currency)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:N2}")]
        public decimal? RefundAmount { get; set; }

        [DataType(DataType.MultilineText)]
        [Column("RefundComment")]
        [Display(Name = "Refund Comment")]
        [MaxLength(1000)]
        [StringLength(1000, ErrorMessage = "Refund comment cannot be longer than 1000 characters.")]
        public string RefundComment  { get; set; }
     
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Accommodation Form Sent To Expat")]
        [DataType(DataType.Date)]
        public DateTime? AccommodationSentToExpat { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Chased Signed Memorandums")]
        [DataType(DataType.Date)]
        [Column("ChasedSignedAgreement")]
        public DateTime? ChasedSignedAgreement { get; set; }

        [DataType(DataType.MultilineText)]
        [Column("RenewalComment")]
        [Display(Name = "Renewal Comment")]
        [MaxLength(3000)]
        [StringLength(3000, ErrorMessage = "Renewal comment cannot be longer than 3000 characters.")]
        public string RenewalComment { get; set; }

        //Required Documents
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Agreement Sent To Agents")]
        [DataType(DataType.Date)]
        public DateTime? AgreementSentToAgent { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Chased Signed Agreement")]
        [DataType(DataType.Date)]
        public DateTime? InitialChasedSignedAgreement { get; set; }

        [Display(Name = "AccommodationForm")]
        public bool AccommodationForm { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Chased Inventory")]
        [DataType(DataType.Date)]
        public DateTime? ChasedInventory { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Memorandum Received From Agent")]
        [DataType(DataType.Date)]
        public DateTime? AgreementReceivedFromAgent { get; set; }

        //Parking GM information
        [Display(Name = "Parking Detail")]
        public int? ParkingID { get; set; }

        [Display(Name = "Parking")]
        public virtual Parking Parkings { get; set; }

        //Bank Initial Tab
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Agreement Sent To Agent")]
        [DataType(DataType.Date)]
        public DateTime? CurrentAgreementSentToAgent { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Signed Agreement Received")]
        [DataType(DataType.Date)]
        public DateTime? SignedAgreementReceived { get; set; }

        [Display(Name = "Indemnity Letter")]
        public bool IndemnityLetter { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Inventory Checkin Sent To Expat")]
        [DataType(DataType.Date)]
        public DateTime? InventoryCheckinExpat { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Inventory Checkin Sent To Agent")]
        [DataType(DataType.Date)]
        public DateTime? InventoryCheckinAgent { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Deposit Date")]
        [DataType(DataType.Date)]
        public DateTime? DepositDate { get; set; }

        [DataType(DataType.MultilineText)]
        [Column("InventoryComment")]
        [Display(Name = "Inventory Comment")]
        [MaxLength(500)]
        [StringLength(500, ErrorMessage = "Inventory comment cannot be longer than 500 characters.")]
        public string InventoryComment { get; set; }

        //Gas Safety Certificate
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Expiry Date")]
        [DataType(DataType.Date)]
        public DateTime? GasExpiryDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Chase Date")]
        [DataType(DataType.Date)]
        public DateTime? GasChaseDate { get; set; }

        [Display(Name = "Not Required")]
        public bool GasNotRequired { get; set; }

        //Termination
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        //[Required(ErrorMessage = "Termination date is required.")]
        [Display(Name = "Termination Date")]
        [DataType(DataType.Date)]
        public DateTime? TerminationDate { get; set; }

        [Required]
        [Display(Name = "DepartureReasonId")]
        public virtual int DepartureReasonId { get { return (int)this.DepartureReason; } set { DepartureReason = (Enums.EnumDepartureReason)value; } }

        [Display(Name = "Departure Reason")]
        [EnumDataType(typeof(Enums.EnumDepartureReason))]
        public Enums.EnumDepartureReason DepartureReason { get; set; }

        [Display(Name = "Rent Refund")]
        [DataType(DataType.Currency)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:N2}")]
        public decimal? RentRefund { get; set; }

        [DataType(DataType.MultilineText)]
        [Column("TerminationComment")]
        [Display(Name = "Termination Comment")]
        [MaxLength(1000)]
        [StringLength(1000, ErrorMessage = "Termination comment cannot be longer than 1000 characters.")]
        public string TerminationComment { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Notice Served")]
        [DataType(DataType.Date)]
        public DateTime? NoticeServed { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Check Out Date")]
        [DataType(DataType.Date)]
        public DateTime? CheckOut { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Rent Paid Until")]
        [DataType(DataType.Date)]
        public DateTime? RentPaidUntil { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Cleaning Date")]
        [DataType(DataType.Date)]
        public DateTime? CleaningDate { get; set; }

        [DataType(DataType.MultilineText)]
        [Column("CleaningComment")]
        [Display(Name = "Cleaning Comment")]
        [MaxLength(255)]
        [StringLength(255, ErrorMessage = "Cleaning comment cannot be longer than 255 characters.")]
        public string CleaningComment { get; set; }

        [DataType(DataType.MultilineText)]
        [Column("DepositComment")]
        [Display(Name = "Deposit Comment")]
        [MaxLength(255)]
        [StringLength(255, ErrorMessage = "Deposit comment cannot be longer than 255 characters.")]
        public string DepositComment { get; set; }

        [Display(Name = "Deposit Returned")]
        [DataType(DataType.Currency)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:N2}")]
        public decimal? DepositReturned { get; set; }

        [Display(Name = "Deposit Deduction")]
        [DataType(DataType.Currency)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:N2}")]
        public decimal? DepositDeduction { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Deposit Received")]
        [DataType(DataType.Date)]
        public DateTime? DepositReceived { get; set; }

        [Display(Name = "No of Bedrooms")]
        public int? NoOfBedroom { get; set; }

    }
}

