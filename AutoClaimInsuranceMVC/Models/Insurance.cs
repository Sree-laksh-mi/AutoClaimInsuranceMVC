using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutoClaimInsuranceMVC.Models
{
    public class Insurance
    {
        [Key]
        [Required]
        [Display(Name = "Policy Number")]
        public string policyNumber { get; set; }
        [Required]
        [Display(Name = "Coverage")]
        public string coverage { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime startDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime endDate { get; set; }
        [Required]
        [Display(Name = "Vehicle Registration Number")]
        public string vehilceRegistrationNumber { get; set; }
        [Required]
        [Display(Name = "Chassis Number")]
        public int chassisNumber { get; set; }
        [Required]
        [Display(Name = "Model")]
        public string model { get; set; }
        [Required]
        [Display(Name = "Value Insured")]
        public double valueInsured { get; set; }
        [Required]
        [Display(Name = "Insurer ID")]
        public string insurerId { get; set; }

        public virtual Insurer insurer { get; set; }

      

    }
}