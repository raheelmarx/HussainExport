using System;
using System.Collections.Generic;

#nullable disable

namespace HussainExport.API.Entities
{
    public partial class FabricPurchase
    {
        public FabricPurchase()
        {
            FabricPurchaseItems = new HashSet<FabricPurchaseItem>();
        }

        public long FabricPurchaseId { get; set; }
        public long? SaleContractId { get; set; }
        public string SaleContractNumber { get; set; }
        public long? Weaver { get; set; }
        public string ContQuality { get; set; }
        public string Gstquality { get; set; }
        public bool? IsConversionContract { get; set; }
        public string ConversionRate { get; set; }
        public decimal? PerPickRate { get; set; }
        public decimal? PerMeterRate { get; set; }
        public decimal? QuantityInMeters { get; set; }
        public string Broker { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual SaleContract SaleContract { get; set; }
        public virtual ICollection<FabricPurchaseItem> FabricPurchaseItems { get; set; }
    }
}
