using System;
using System.Collections.Generic;

#nullable disable

namespace HussainExport.DataAccess.Model
{
    public partial class SaleContract
    {
        public SaleContract()
        {
            SaleContractItems = new HashSet<SaleContractItem>();
        }

        public long SaleContractId { get; set; }
        public string SaleContractNumber { get; set; }
        public long? CustomerId { get; set; }
        public long? TotalAmount { get; set; }
        public int? CurrencyId { get; set; }
        public string DeliveryTime { get; set; }
        public string ShipmentDetails { get; set; }
        public string Packing { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual Currency Currency { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<SaleContractItem> SaleContractItems { get; set; }
    }
}
