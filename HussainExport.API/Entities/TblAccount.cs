using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HussainExport.API.Entities
{
    public partial class TblAccount
    {
        public TblAccount()
        {
            AccountTransactionAccountCredit = new HashSet<AccountTransaction>();
            AccountTransactionAccountDebit = new HashSet<AccountTransaction>();
        }

        public long AccountId { get; set; }
        public string AccountTitle { get; set; }
        public string AccountCode { get; set; }
        public string AccountDescription { get; set; }
        public long? ReceivablesId { get; set; }
        public long? PayableId { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? IsActive { get; set; }
        public long? AccountTypeId { get; set; }

        public virtual AccountType AccountType { get; set; }
        public virtual Payable Payable { get; set; }
        public virtual Receivable Receivables { get; set; }
        public virtual ICollection<AccountTransaction> AccountTransactionAccountCredit { get; set; }
        public virtual ICollection<AccountTransaction> AccountTransactionAccountDebit { get; set; }
    }
}
