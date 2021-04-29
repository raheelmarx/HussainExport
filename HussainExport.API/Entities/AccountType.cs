using System;
using System.Collections.Generic;

namespace HussainExport.API.Entities
{
    public partial class AccountType
    {
        public AccountType()
        {
            TblAccount = new HashSet<TblAccount>();
        }

        public long AccountTypeId { get; set; }
        public string AccountTypeName { get; set; }
        public string AccountTypeDescription { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<TblAccount> TblAccount { get; set; }
    }
}
