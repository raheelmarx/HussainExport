using System;
using System.Collections.Generic;

namespace HussainExport.API.Entities
{
    public partial class Currency
    {
        public Currency()
        {
            SaleContract = new HashSet<SaleContract>();
        }

        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<SaleContract> SaleContract { get; set; }
    }
}
