using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HussainExport.API.Entities
{
    public partial class Inventory
    {
        public long InventoryId { get; set; }
        public long? TransactionId { get; set; }
        public string InventoryName { get; set; }
        public string InventoryDescription { get; set; }
        public decimal? InventoryQuantity { get; set; }
        public decimal? InventoryPerUnitCost { get; set; }
        public string InventoryTotalAmount { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? IsActive { get; set; }

        public virtual AccountTransaction Transaction { get; set; }
    }
}
