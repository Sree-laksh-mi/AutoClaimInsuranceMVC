using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoClaimInsuranceMVC.Models
{
    public class Report
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Report ID")]
        public int reportId { get; set; }
        [Required]
        [Display(Name = "Officer ID")]
        public string officerId { get; set; }
        [Required]
        [Display(Name = "Claim ID")]
        public int claimId { get; set; }
        [Required]
        [Display(Name = "Status")]
        public string status { get; set; }
        [Required]
        [Display(Name = "Content")]
        public string content { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Report Date")]
        public DateTime reportDate { get; set; }
        [Required]
        [Display(Name = "Amount")]
        public double amount { get; set; }



      
        public virtual Claim claim { get; set; }
        public virtual Officer Officer { get; set; }
    }
}