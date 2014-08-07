using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ExpatManager.Models
{
    public class ContactBaseClass : ModifiedBaseClass
    {
        private string _Address1;

        [Required(ErrorMessage = "Address 1 is required.")]
        [Column("Address1")]
        [Display(Name = "Address")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Address1
        {
            get
            {
                if (string.IsNullOrEmpty(_Address1))
                {
                    return _Address1;
                }
                return _Address1.ToUpper();
            }
            set { _Address1 = value; }
        }

        private string _Address2;

        //[Required(ErrorMessage = "Address 2 is required.")]
        [Column("Address2")]
        [Display(Name = "Address Line 2")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Address2
        {
            get
            {
                if (string.IsNullOrEmpty(_Address2))
                {
                    return _Address2;
                }
                return _Address2.ToUpper();
            }
            set { _Address2 = value; }
        }

        private string _Address3 = "London";
        //[Required(ErrorMessage = "Address 3 is required.")]
        [Column("Address3")]
        [Display(Name = "City")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Address3
        {
            get
            {
                if (string.IsNullOrEmpty(_Address3))
                {
                    return _Address3;
                }
                return _Address3.ToUpper();
            }
            set { _Address3 = value; }
        }

        //[Required(ErrorMessage = "Address 4 is required.")]
        /*
        [Column("Address4")]
        [Display(Name = "Address Line 4")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")] 
        public string Address4 { get; set; }
        */

        private string _PostCode;

        [Column("PostCode")]
        [Display(Name = "Post Code")]
        [MaxLength(20)]
        [StringLength(20, ErrorMessage = "Name cannot be longer than 20 characters.")]
        public string PostCode
        {
            get
            {
                if (string.IsNullOrEmpty(_PostCode))
                {
                    return _PostCode;
                }
                return _PostCode.ToUpper();
            }
            set { _PostCode = value; }
        }

        //[Required(ErrorMessage = "TelNo is required.")]
        [Column("TelNo")]
        [Display(Name = "Tel No")]
        [MaxLength(20)]
        [StringLength(20, ErrorMessage = "Name cannot be longer than 20 characters.")]
        public string TelNo { get; set; }

        private string _Email;

        [Column("Email")]
        [Display(Name = "Email")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Email
        {
            get
            {
                if (string.IsNullOrEmpty(_Email))
                {
                    return _Email;
                }
                return _Email.ToUpper();
            }
            set { _Email = value; }
        }

        [Column("Fax")]
        [Display(Name = "Fax")]
        [MaxLength(20)]
        [StringLength(20, ErrorMessage = "Name cannot be longer than 20 characters.")]
        public string Fax { get; set; }
    }
}