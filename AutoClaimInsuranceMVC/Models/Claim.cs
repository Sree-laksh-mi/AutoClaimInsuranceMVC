using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoClaimInsuranceMVC.Models
{
    public class Claim
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name ="Claim ID")]
        public int claimId { get; set; }
        [Required(ErrorMessage = "InsurerId Required")]
        [Display(Name ="Insurer ID")]
        public string insurerId { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "MailId Required")]
        [Display(Name = "Mail ID")]
        public string MailID { get; set; }
        [Required(ErrorMessage = "Policy Number Required")]
        [Display(Name = "Policy Number")]
        public string policyNumber { get; set; }
        [Required]
        [Display(Name = "Loss or Damage due to")]
        public string reason{ get; set; }
        [Required(ErrorMessage = "Date and Time Required")]
        [Display(Name = "Date And Time")]
        [DataType(DataType.DateTime)]
       
        public DateTime dateAndTime { get; set; }
        [Required(ErrorMessage = "Please select Yes or No")]
        [Display(Name = "Police Case")]
        public string policeCase { get; set; }
        [Required(ErrorMessage = "No file selected")]
        [Display(Name = "Licence Copy")]
        public string licenseCopy { get; set; }
        [Required(ErrorMessage = "No file selected")]
        [Display(Name = "RC Copy")]
        public string rcCopy { get; set; }
        [Required]
        [Display(Name ="Status")]
        public string status { get; set; }
        [Required]
        [Display(Name = "Claim Date")]
        [DataType(DataType.Date)]
        public DateTime claimDate { get; set; }


        public virtual Insurer insurer { get; set; }
        

        public virtual ICollection<Report> Reports { get; set; }
    }
}