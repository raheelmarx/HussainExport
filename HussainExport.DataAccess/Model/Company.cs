using System;
using System.Collections.Generic;

#nullable disable

namespace HussainExport.DataAccess.Model
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
