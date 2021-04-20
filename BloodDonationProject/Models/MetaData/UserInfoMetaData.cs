using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BloodDonationProject.Models.MetaData
{
    public class UserInfoMetaData
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Display(Name ="Phone Number")]
        public string Phone { get; set; }
        [Display(Name ="Your Blood Group")]
        public string BloodGroup { get; set; }
        [Display(Name ="Last Donate")]
        public Nullable<System.DateTime> LastDonate { get; set; }
    }
}