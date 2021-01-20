using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

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
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<TblAccount> TblAccount { get; set; }
    }
}
