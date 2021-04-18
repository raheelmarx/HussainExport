using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HussainExport.Client.Models
{
    public class FabricPurchaseVM
    {
        [Key]
        public long FabricPurchaseId { get; set; }
        public long? SaleContractId { get; set; }
        public string SaleContractNumber { get; set; }
        public string Weaver { get; set; }
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

        public virtual SaleContractVM SaleContract { get; set; }
        public virtual ICollection<FabricPurchaseItemVM> FabricPurchaseItem { get; set; }
    }
}
