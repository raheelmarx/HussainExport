using System;
using System.Collections.Generic;

#nullable disable

namespace HussainExport.API.Entities
{
    public partial class AccountTransaction
    {
        public AccountTransaction()
        {
            Assets = new HashSet<Asset>();
            Inventories = new HashSet<Inventory>();
            OwnerEquities = new HashSet<OwnerEquity>();
        }

        public long AccountTransactionId { get; set; }
        public long? Type { get; set; }
        public long? AccountDebitId { get; set; }
        public long? AccountCreditId { get; set; }
        public string VoucherNo { get; set; }
        public DateTime? VoucherDate { get; set; }
        public string AccountDebitCode { get; set; }
        public string AccountCreditCode { get; set; }
        public string Narration { get; set; }
        public decimal? AmountDebit { get; set; }
        public decimal? AmountCredit { get; set; }
        public long SaleContractId { get; set; }
        public string SaleContractNumber { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? IsActive { get; set; }

        public virtual TblAccount AccountCredit { get; set; }
        public virtual TblAccount AccountDebit { get; set; }
        public virtual SaleContract SaleContract { get; set; }
        public virtual TransactionType TypeNavigation { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<OwnerEquity> OwnerEquities { get; set; }
    }
}
