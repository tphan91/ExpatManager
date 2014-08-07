using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ExpatManager.Models
{
    public class ExpatriateDocumentUpload : ModifiedBaseClass
    {
        [Key]
        public int ExpatriateDocumentUploadID { get; set; }

        [Column("ExpatriateID")]
        [Display(Name = "Expatriate")]
        public int ExpatriateID { get; set; }

        [Display(Name = "Expatriate")]
        public virtual Expatriate Expatriates { get; set; }

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
    }
}