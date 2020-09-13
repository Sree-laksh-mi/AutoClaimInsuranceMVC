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
        public string insurerId { get; set; }
        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
        [Required]
        public string address { get; set; }
        [Required]
        public string mobileNumber { get; set; }


        public virtual ICollection<Insurance> Insurences { get; set; }
        public virtual ICollection<Claim> Claims { get; set; }
        public virtual ICollection<RegisteredUser> RegisteredUsers { get; set; }
        
    }
}