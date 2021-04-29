using System;
using System.Collections.Generic;

namespace HussainExport.API.Entities
{
    public partial class Receivable
    {
        public Receivable()
        {
            TblAccount = new HashSet<TblAccount>();
        }

        public long ReceivableId { get; set; }
        public string ReceivableName { get; set; }
        public string ReceivableDescription { get; set; }
        public string ReceivableAddress { get; set; }
        public string ReceivablePhone { get; set; }
        public long? CustomerId { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? IsActive { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<TblAccount> TblAccount { get; set; }
    }
}
