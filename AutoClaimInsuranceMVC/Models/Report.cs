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
        public int reportId { get; set; }
        [Required]
        public string officerId { get; set; }
        [Required]
        public int claimId { get; set; }
        [Required]
        public string status { get; set; }
        [Required]
        public string content { get; set; }
        [DataType(DataType.Date)]
        public DateTime reportDate { get; set; }
        [Required]
        public double amount { get; set; }



      
        public virtual Claim claim { get; set; }
        public virtual Officer Officer { get; set; }
    }
}