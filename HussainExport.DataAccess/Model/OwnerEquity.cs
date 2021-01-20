using System;
using System.Collections.Generic;

#nullable disable

namespace HussainExport.DataAccess.Model
{
    public partial class OwnerEquity
    {
        public long OwnerEquityId { get; set; }
        public long? TransactionId { get; set; }
        public string OwnerEquityName { get; set; }
        public string OwnerEquityDescription { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? IsActive { get; set; }

        public virtual AccountTransaction Transaction { get; set; }
    }
}
