using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutoClaimInsuranceMVC.Models
{
    public class Insurer
    {
        [Key]
        [Required]
        [Display(Name = "Insurer ID")]
        public string insurerId { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string firstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string lastName { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string address { get; set; }
        [Required]
        [Display(Name = "Mobile Number")]
        public string mobileNumber { get; set; }


        public virtual ICollection<Insurance> Insurences { get; set; }
        public virtual ICollection<Claim> Claims { get; set; }
        public virtual ICollection<RegisteredUser> RegisteredUsers { get; set; }
        
    }
}