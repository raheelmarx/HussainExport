﻿using System;
using System.Collections.Generic;

#nullable disable

namespace HussainExport.DataAccess.Model
{
    public partial class PurchaseReceived
    {
        public long PurchaseReceivedId { get; set; }
        public long? PurchaseId { get; set; }
        public decimal? QuantityReceived { get; set; }
        public decimal? PurchaseReceivedPerUnitCost { get; set; }
        public decimal? PurchaseReceivedTotalAmount { get; set; }
        public decimal? Gazana { get; set; }
        public decimal? Quality { get; set; }
        public decimal? Than { get; set; }
        public decimal? Meter { get; set; }
        public decimal? Gain { get; set; }
        public decimal? Rejection { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? IsActive { get; set; }

        public virtual Purchase Purchase { get; set; }
    }
}
