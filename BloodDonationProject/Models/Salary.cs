//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BloodDonationProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Salary
    {
        public int id { get; set; }
        public int UserId { get; set; }
        public string January { get; set; }
        public string February { get; set; }
        public string March { get; set; }
        public string April { get; set; }
        public string May { get; set; }
        public string June { get; set; }
        public string July { get; set; }
        public string August { get; set; }
        public string September { get; set; }
        public string October { get; set; }
        public string November { get; set; }
        public string December { get; set; }
    
        public virtual userInfo userInfo { get; set; }
    }
}
