using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HussainExport.Client.Models
{
    public class AccountTransactionVM
    {
        //public AccountTransactionVM()
        //{
        //    Assets = new HashSet<Asset>();
        //    Inventories = new HashSet<Inventory>();
        //    OwnerEquities = new HashSet<OwnerEquity>();
        //}
        [Key]
        public long AccountTransactionId { get; set; }
        [Display(Name = "Transaction Type")]
        public long? Type { get; set; }
        [Display(Name = "Account Debit")]
        public long? AccountDebitId { get; set; }
        [Display(Name = "Account Credit")]
        public long? AccountCreditId { get; set; }
        [Display(Name = "Voucher No")]
        public string VoucherNo { get; set; }
        [Display(Name = "Voucher Date")]
        public DateTime? VoucherDate { get; set; }
        [Display(Name = "Account Debit")]
        public string AccountDebitCode { get; set; }
        [Display(Name = "Account Credit")]
        public string AccountCreditCode { get; set; }
        [Display(Name = "Narration")]
        public string Narration { get; set; }
        [Display(Name = "Amount Debit")]
        public decimal? AmountDebit { get; set; }
        [Display(Name = "Amount Credit")]
        public decimal? AmountCredit { get; set; }
        [Display(Name = "Sale Contract")]
        public long SaleContractId { get; set; }
        [Display(Name = "Sale Contract Number")]
        public string SaleContractNumber { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? IsActive { get; set; }

        public virtual TblAccountVM AccountCredit { get; set; }
        public virtual TblAccountVM AccountDebit { get; set; }
        public virtual SaleContractVM SaleContract { get; set; }
        public virtual TransactionTypeVM TypeNavigation { get; set; }
        //public virtual ICollection<Asset> Assets { get; set; }
        //public virtual ICollection<Inventory> Inventories { get; set; }
        //public virtual ICollection<OwnerEquity> OwnerEquities { get; set; }
    }
}
