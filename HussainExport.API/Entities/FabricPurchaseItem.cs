using System;
using System.Collections.Generic;

#nullable disable

namespace HussainExport.API.Entities
{
    public partial class FabricPurchaseItem
    {
        public long FabricPurchaseItemId { get; set; }
        public long? FabricPurchaseId { get; set; }
        public string Yarn { get; set; }
        public decimal? YarnRatePerIbs { get; set; }
        public decimal? CountMargin { get; set; }
        public decimal? WeightPerMeterIbs { get; set; }
        public decimal? RequiredBags { get; set; }
        public decimal? FabricRatePerMeter { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual FabricPurchase FabricPurchase { get; set; }
    }
}
