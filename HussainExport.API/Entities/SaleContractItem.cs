using System;
using System.Collections.Generic;

namespace HussainExport.API.Entities
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
        public decimal? Price { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Amount { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual SaleContract SaleContract { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
