using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HussainExport.API.Entities
{
    public partial class TransactionType
    {
        public TransactionType()
        {
            AccountTransaction = new HashSet<AccountTransaction>();
        }

        public long TransactionTypeId { get; set; }
        public string TransactionTypeName { get; set; }
        public string Description { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<AccountTransaction> AccountTransaction { get; set; }
    }
}
