using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace AutoClaimInsuranceMVC.Models
{
    public class RegisteredUser
    {
        [Required(ErrorMessage = "Insurer ID is required")]
        [Display(Name = "Insurer ID")]
        public string insurerId { get; set; }
        [Key]
        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string userId { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(255, ErrorMessage = "Password should be minimum of 8 characters", MinimumLength = 8)]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and should contain : upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public string password { get; set; }
        [NotMapped]
        [System.ComponentModel.DataAnnotations.Compare("password")]
        [Required(ErrorMessage = "Confirm Password required")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        
        public string confirmPassword { get; set; }


        public virtual Insurer insurer { get; set; }

    }
}