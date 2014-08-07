using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ExpatManager.Models
{
    public class AgreementDetailDocumentUpload : ModifiedBaseClass
    {
        [Key]
        public int AgreementDetailDocumentUploadID { get; set; }

        [Column("AgreementDetailID")]
        [Display(Name = "AgreementDetailID")]
        public int AgreementDetailID { get; set; }

        [Display(Name = "Agreement Details")]
        public virtual AgreementDetail AgreementDetails { get; set; }

        [Required(ErrorMessage = "File name is required.")]
        [Column("FileName")]
        [Display(Name = "File Name")]
        [MaxLength(150)]
        [StringLength(150, ErrorMessage = "Name cannot be longer than 150 characters.")] 
        public string FileName { get; set; }

        [Column("FileType")]
        [Display(Name = "File Type")]
        [MaxLength(100)]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")] 
        public string FileType { get; set; }

        [Column("FileSize")]
        [Display(Name = "File Size")]
        public int FileSize { get; set; }

        [Required]
        [Display(Name = "Document Type")]
        public int DocumentTypeId { get { return (int)this.DocumentType; } set { DocumentType = (Enums.EnumDocumentType)value; } }

        [Display(Name = "Document Type")]
        [EnumDataType(typeof(Enums.EnumDocumentType))]
        public Enums.EnumDocumentType DocumentType { get; set; }

        [Column("SecondChecker")]
        [Display(Name = "Second Checker")]
        public bool SecondChecker { get; set; }
    }
}