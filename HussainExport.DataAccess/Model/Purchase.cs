using System;
using System.Collections.Generic;

#nullable disable

namespace HussainExport.DataAccess.Model
{
    public partial class Purchase
    {
        public Purchase()
        {
            PurchaseReceiveds = new HashSet<PurchaseReceived>();
        }

        public long PurchaseId { get; set; }
        public long? PayableId { get; set; }
        public string PurchaseTitle { get; set; }
        public string PurchaseDescription { get; set; }
        public decimal? PurchaseTotalQuantity { get; set; }
        public decimal? PurchasePerUnitCost { get; set; }
        public decimal? PurchaseTotalAmount { get; set; }
        public decimal? Gazana { get; set; }
        public decimal? Quality { get; set; }
        public decimal? Than { get; set; }
        public decimal? Meter { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? IsActive { get; set; }

        public virtual Payable Payable { get; set; }
        public virtual ICollection<PurchaseReceived> PurchaseReceiveds { get; set; }
    }
}
