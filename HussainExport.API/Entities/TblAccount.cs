﻿using System;
using System.Collections.Generic;

#nullable disable

namespace HussainExport.API.Entities
{
    public partial class TblAccount
    {
        public TblAccount()
        {
            AccountTransactionAccountCredits = new HashSet<AccountTransaction>();
            AccountTransactionAccountDebits = new HashSet<AccountTransaction>();
            FactoryOverheadExpenses = new HashSet<FactoryOverheadExpense>();
            SaleContractExpenses = new HashSet<SaleContractExpense>();
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
        public virtual ICollection<AccountTransaction> AccountTransactionAccountCredits { get; set; }
        public virtual ICollection<AccountTransaction> AccountTransactionAccountDebits { get; set; }
        public virtual ICollection<FactoryOverheadExpense> FactoryOverheadExpenses { get; set; }
        public virtual ICollection<SaleContractExpense> SaleContractExpenses { get; set; }
    }
}
