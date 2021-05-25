using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HussainExport.Client.Models
{
    public class TblAccountVM
    {
        public TblAccountVM()
        {
            AccountTransactionAccountCredits = new HashSet<AccountTransactionVM>();
            AccountTransactionAccountDebits = new HashSet<AccountTransactionVM>();
            FactoryOverheadExpenses = new HashSet<FactoryOverheadExpenseVM>();
            SaleContractExpenses = new HashSet<SaleContractExpenseVM>();
        }
        [Key]
        [Display(Name = "Account Code")]
        public long AccountId { get; set; }
        [Display(Name = "Account Title")]
        public string AccountTitle { get; set; }
        [Display(Name = "Account Code")]
        public string AccountCode { get; set; }
        [Display(Name = "Account Description")]
        public string AccountDescription { get; set; }
        [Display(Name = "Receivables Id")]
        public long? ReceivablesId { get; set; }
        [Display(Name = "Payable Id")]
        public long? PayableId { get; set; }
        [Display(Name = "Date Added")]
        public DateTime? DateAdded { get; set; }
        [Display(Name = "Date Updated")]
        public DateTime? DateUpdated { get; set; }
        [Display(Name = "Is Active")]
        public bool? IsActive { get; set; }
        [Display(Name = "Account Type")]
        public long? AccountTypeId { get; set; }

        public virtual AccountTypeVM AccountType { get; set; }
        public virtual PayableVM Payable { get; set; }
        public virtual ReceivableVM Receivables { get; set; }
        public virtual ICollection<AccountTransactionVM> AccountTransactionAccountCredits { get; set; }
        public virtual ICollection<AccountTransactionVM> AccountTransactionAccountDebits { get; set; }
        public virtual ICollection<FactoryOverheadExpenseVM> FactoryOverheadExpenses { get; set; }
        public virtual ICollection<SaleContractExpenseVM> SaleContractExpenses { get; set; }
    }
}
