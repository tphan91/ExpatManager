using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ExpatManager.Models
{
    public class AgentDetail: ContactBaseClass
    {
        [Display(Name = "Agent Detail")]
        public int AgentDetailID { get; set; }


        private string _AgentName;

        [Required(ErrorMessage = "Agent name is required.")]
        [Column("AgentName")]
        [Display(Name = "Bank Account Name")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "Agent Name cannot be longer than 50 characters.")]
        public string AgentName
        {
            get
            {
                if (string.IsNullOrEmpty(_AgentName))
                {
                    return _AgentName;
                }
                return _AgentName.ToUpper();
            }
            set { _AgentName = value; }
        }

        private string _BranchOffice;

        [Column("Branch/Office")]
        [Display(Name = "Branch/Office")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "Branch/office cannot be longer than 50 characters.")]
        public string BranchOffice
        {
            get
            {
                if (string.IsNullOrEmpty(_BranchOffice))
                {
                    return _BranchOffice;
                }
                return _BranchOffice.ToUpper();
            }
            set { _BranchOffice = value; }
        }

        private string _ContactName;

        [Column("ContactName")]
        [Display(Name = "Contact Name")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "Contact name cannot be longer than 50 characters.")]
        public string ContactName
        {
            get
            {
                if (string.IsNullOrEmpty(_ContactName))
                {
                    return _ContactName;
                }
                return _ContactName.ToUpper();
            }
            set { _ContactName = value; }
        }

        public virtual ICollection<LandlordBankDetail> LandlordBankDetails { get; set; }
    }
}