using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ExpatManager.Models
{
    public class vPersonJob
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("personId", TypeName="int")]
        public int PersonJobID { get; set; }

        [Column("payRollNo")]
        [Display(Name = "PayRollNo")]
        [MaxLength(10)]
        [StringLength(10, ErrorMessage = "Name cannot be longer than 10 characters.")]
        public string PayRollNo { get; set; }

        [Column("userName")]
        [Display(Name = "UserName")]
        [MaxLength(20)]
        [StringLength(20, ErrorMessage = "Name cannot be longer than 20 characters.")]
        public string UserName { get; set; }

        [Column("title")]
        [Display(Name = "Title")]
        [MaxLength(10)]
        [StringLength(10, ErrorMessage = "Name cannot be longer than 10 characters.")] 
        public string Title { get; set; }

        [Column("firstName")]
        [Display(Name = "First Name")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")] 
        public string FirstName { get; set; }

        [Column("lastName")]
        [Display(Name = "LastName")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")] 
        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }

        [Column("deptId", TypeName = "int")]
        [Display(Name = "DeptId")]
        public int DeptId { get; set; }

        [Column("deptName")]
        [Display(Name = "Department Name")]
        [MaxLength(100)]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")] 
        public string DeptName { get; set; }

        [Column("deptCode")]
        [Display(Name = "Cost Code")]
        [MaxLength(4)]
        [StringLength(4, ErrorMessage = "Name cannot be longer than 4 characters.")] 
        public string CostCode { get; set; }

        [Column("divisionId", TypeName = "int")]
        [Display(Name = "DivId")]
        public int? DivId { get; set; }

        //[Required(ErrorMessage = "TelNo is required.")]
        [Column("TelNo")]
        [Display(Name = "Tel No")]
        [MaxLength(20)]
        [StringLength(20, ErrorMessage = "Name cannot be longer than 20 characters.")] 
        public string TelNo { get; set; }
    }
}