using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ExpatManager.Infrastructure;

namespace ExpatManager.Models
{
    public class Parking : ModifiedBaseClass
    {
        [Key]
        public int ParkingID { get; set; }

        [Display(Name = "Parking Amount")]
        [DataType(DataType.Currency)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:N2}")]
        public decimal? ParkingAmount { get; set; }

        [Display(Name = "Paid By")]
        [Column("ParkingPaidBy")]
        [MaxLength(20)]
        [StringLength(20, ErrorMessage = "Paid by cannot be longer than 20 characters.")]
        public string ParkingPaidBy { get; set; }

        [Display(Name = "Parking Comment")]
        [Column("ParkingComment")]
        [MaxLength(255)]
        [DataType(DataType.MultilineText)]
        [StringLength(255, ErrorMessage = "Parking comment cannot be longer than 255 characters.")]
        public string ParkingComment { get; set; }
    }
}