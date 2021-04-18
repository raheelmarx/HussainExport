using System;
using System.Collections.Generic;

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
