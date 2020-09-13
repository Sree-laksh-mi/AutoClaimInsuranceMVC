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
        public int claimId { get; set; }
        [Required(ErrorMessage = "InsurerId Required")]
        public string insurerId { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "MailId Required")]
        public string MailID { get; set; }
        [Required(ErrorMessage = "Policy Number Required")]
        public int policyNumber { get; set; }
        [Required(ErrorMessage = "Date and Time Required")]
        [DataType(DataType.DateTime)]
        public DateTime dateAndTime { get; set; }
        [Required(ErrorMessage = "Please select Yes or No")]
        public string policeCase { get; set; }
        [Required]
        public string firNumber { get; set; }
        [Required(ErrorMessage = "No file selected")]
        public string licenseCopy { get; set; }
        [Required(ErrorMessage = "No file selected")]
        public string rcCopy { get; set; }
        [Required]
        public string status { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime claimDate { get; set; }


        public virtual Insurer insurer { get; set; }
        public virtual Insurance insurance { get; set; }

        public virtual ICollection<Report> Reports { get; set; }
    }
}