using System;
using System.Collections.Generic;

#nullable disable

namespace HussainExport.DataAccess.Model
{
    public partial class Payable
    {
        public Payable()
        {
            Purchases = new HashSet<Purchase>();
            TblAccounts = new HashSet<TblAccount>();
        }

        public long PayableId { get; set; }
        public string PayableName { get; set; }
        public string PayableDescription { get; set; }
        public string PayableAddress { get; set; }
        public string PayablePhone { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Purchase> Purchases { get; set; }
        public virtual ICollection<TblAccount> TblAccounts { get; set; }
    }
}
