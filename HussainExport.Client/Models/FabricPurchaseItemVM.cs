using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HussainExport.Client.Models
{
    public class FabricPurchaseItemVM
    {
        [Key]
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

        public virtual FabricPurchaseVM FabricPurchase { get; set; }
    }
}
