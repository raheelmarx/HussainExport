using System;
using System.Collections.Generic;

#nullable disable

namespace HussainExport.DataAccess.Model
{
    public partial class TransactionType
    {
        public TransactionType()
        {
            AccountTransactions = new HashSet<AccountTransaction>();
        }

        public long TransactionTypeId { get; set; }
        public string TransactionTypeName { get; set; }
        public string Description { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<AccountTransaction> AccountTransactions { get; set; }
    }
}
