using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ExpatManager.Helper;

namespace ExpatManager.Models
{
    public class Expatriate : ExpatriateBaseClass
    {
        //[Required(ErrorMessage = "CIF is required.")]
        [Column("CIF")]
        [Display(Name = "CIF")]
        [StringLength(10, ErrorMessage = "CIF cannot be longer than 10 characters.")]
        public String CIF { get; set; }

        private string _Firstname;
        //[Required(ErrorMessage = "First name is required.")]
        [Column("FirstName")]
        [Display(Name = "First Name")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public string FirstName
        {
            get
            {
                if (string.IsNullOrEmpty(_Firstname))
                {
                    return _Firstname;
                }
                return _Firstname.ToUpper();
            }
            set { _Firstname = value; }
        }

        private string _Lastname;

        //[Required(ErrorMessage = "Last name is required.")]
        [Column("LastName")]
        [Display(Name = "Last Name")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        public string LastName
        {
            get
            {
                if (string.IsNullOrEmpty(_Lastname))
                {
                    return _Lastname;
                }
                return _Lastname.ToUpper();
            }
            set { _Lastname = value; }
        }

        [Display(Name = "Name")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }

        [Display(Name = "Name and CIF")]
        public string FullNameCIF
        {
            get
            {
                return Title + ". " + LastName + ", " + FirstName + " - " + CIF;
            }
        }

        private string _Comment;
        [DataType(DataType.MultilineText)]
        [Display(Name = "Comment")]
        [MaxLength(500)]
        [StringLength(500, ErrorMessage = "Name cannot be longer than 500 characters.")]
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

        [Display(Name = "GM Flag")]
        public Boolean GMFlag { get; set; }

        public virtual ICollection<AgreementDetail> AgreementDetails { get; set; }
        public virtual ICollection<ExpatriateHistory> ExpatriateHistorys { get; set; }
        public virtual ICollection<Family> Familys { get; set; }
        public virtual ICollection<vPersonJob> PersonJobs { get; set; }
        public virtual ICollection<ExpatriateDocumentUpload> ExpatriateDocumentUploads { get; set; }
    }
}