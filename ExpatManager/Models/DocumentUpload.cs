using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ExpatManager.Models
{
    public class DocumentUpload : ModifiedBaseClass
    {
        [Key]
        public int DocumentUploadID { get; set; }

        [Required(ErrorMessage = "File name is required.")]
        [Column("FileName")]
        [Display(Name = "File Name")]
        [MaxLength(150)]
        [StringLength(150, ErrorMessage = "Name cannot be longer than 50 characters.")] 
        public string FileName { get; set; }

        [Column("FileType")]
        [Display(Name = "FileType")]
        [MaxLength(100)]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 50 characters.")] 
        public string FileType { get; set; }
        
        public int FileSize { get; set; }

        //[Required(ErrorMessage = "Document Description is required.")]
        [Column("DocumentDescription")]
        [Display(Name = "Document Description")]
        [MaxLength(100)]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 50 characters.")] 
        public string DocumentDescription { get; set; }

        public Enums.EnumModelName EnumModelName { get; set; }

        [Column("ModelName")]
        [Display(Name = "Model Name")]
        [MaxLength(20)]
        [StringLength(20, ErrorMessage = "Name cannot be longer than 20 characters.")] 
        public string ModelName { get; set; }

        [Display(Name = "Model Ref ID")]
        public int ModelRefID { get; set; }
    }
}