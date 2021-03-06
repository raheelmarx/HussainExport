﻿using System;
using System.Collections.Generic;

#nullable disable

namespace HussainExport.API.Entities
{
    public partial class Customer
    {
        public Customer()
        {
            Receivables = new HashSet<Receivable>();
            SaleContracts = new HashSet<SaleContract>();
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

        public virtual ICollection<Receivable> Receivables { get; set; }
        public virtual ICollection<SaleContract> SaleContracts { get; set; }
    }
}
