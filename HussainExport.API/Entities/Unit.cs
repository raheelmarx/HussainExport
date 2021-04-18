using System;
using System.Collections.Generic;

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
