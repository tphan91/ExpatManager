using System;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
using ExpatManager.Infrastructure;

namespace ExpatManager.Models
{
    //public partial class EntitiesExtentsion : ObjectContext
    //{
    //    public EntitiesExtension()
    //    {
    //        OnContextCreated();
    //    }
        
    //    partial void OnContextCreated();
    //}

    public class Audit
    {
        [Display(Name = "Audit Id")]
        [Key]
        public string AuditId { get; set; }

        [Display(Name = "Create Date Time")]
        [DataType(DataType.Date)]
        [DateRange(Min = "1901/01/01", Max = "2050/12/30",SuppressDataTypeUpdate=true)]
        public DateTime RevisionStamp { get; set; }

        [Column("TableName")]
        [Display(Name = "Table Name")]
        [MaxLength(30)]
        [StringLength(30, ErrorMessage = "Table Name cannot be longer than 50 characters.")]
        public string TableName { get; set; }

        [Column("UserName")]
        [Display(Name = "User Name")]
        [MaxLength(15)]
        [StringLength(15, ErrorMessage = "User Name cannot be longer than 15 characters.")]
        public string UserName  { get; set; }

        [Column("Actions")]
        [Display(Name = "Actions")]
        [MaxLength(1)]
        [StringLength(1, ErrorMessage = "Actions cannot be longer than 1 characters.")]
        public string Action { get; set; }

        [Column("OldData")]
        [Display(Name = "Old Data")]
        public string OldData { get; set; }

        [Column("NewData")]
        [Display(Name = "New Data")]
        public string NewData { get; set; }

        [Column("ChangedColumns")]
        [Display(Name = "Changed columns")]
        [MaxLength(1000)]
        [StringLength(1000, ErrorMessage = "Changed columns cannot be longer than 1000 characters.")]
        public string ChangedColumn { get; set; }
    }
}
