﻿using System;
using System.Collections.Generic;

#nullable disable

namespace HussainExport.DataAccess.Model
{
    public partial class SaleContractItem
    {
        public long SaleContractItemId { get; set; }
        public long? SaleContractId { get; set; }
        public string Quality { get; set; }
        public string Article { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public int? UnitId { get; set; }
        public string Price { get; set; }
        public string Quantity { get; set; }
        public string Amount { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual SaleContract SaleContract { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
