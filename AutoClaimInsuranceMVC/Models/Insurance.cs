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
        public string policyNumber { get; set; }
        [Required]
        public string coverage { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime startDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime endDate { get; set; }
        [Required]
        public string vehilceRegistrationNumber { get; set; }
        [Required]
        public int chassisNumber { get; set; }
        [Required]
        public string model { get; set; }
        [Required]
        public double valueInsured { get; set; }
        [Required]
        public string insurerId { get; set; }


        public virtual Insurer insurer { get; set; }



    }
}