using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HussainExport.API.Entities
{
    public partial class Customer
    {
        public Customer()
        {
            SaleContract = new HashSet<SaleContract>();
        }

        public long CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerBussinessDetails { get; set; }
        public string CustomerDescription { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<SaleContract> SaleContract { get; set; }
    }
}
