using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HussainExport.API.Entities
{
    public partial class Unit
    {
        public Unit()
        {
            SaleContractItem = new HashSet<SaleContractItem>();
        }

        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public string UnitSymbol { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<SaleContractItem> SaleContractItem { get; set; }
    }
}
