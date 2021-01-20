using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HussainExport.API.Entities
{
    public partial class Payable
    {
        public Payable()
        {
            Purchase = new HashSet<Purchase>();
            TblAccount = new HashSet<TblAccount>();
        }

        public long PayableId { get; set; }
        public string PayableName { get; set; }
        public string PayableDescription { get; set; }
        public string PayableAddress { get; set; }
        public string PayablePhone { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Purchase> Purchase { get; set; }
        public virtual ICollection<TblAccount> TblAccount { get; set; }
    }
}
