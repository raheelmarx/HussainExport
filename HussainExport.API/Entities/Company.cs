using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HussainExport.API.Entities
{
    public partial class Company
    {
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyDescription { get; set; }
        public string CompanyPhone { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? IsActive { get; set; }
    }
}
