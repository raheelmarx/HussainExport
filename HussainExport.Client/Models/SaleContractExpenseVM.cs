using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HussainExport.Client.Models
{
    public class SaleContractExpenseVM
    {
        [Key]
        public long ExpenseId { get; set; }
        [Display(Name = "Sale Contract")]
        public long? SaleContractId { get; set; }
        [Display(Name = "Sale Contract Number")]
        public string SaleContractNumber { get; set; }
        [Display(Name = "Payment Source Account")]
        public long? PaymentSourceAccountId { get; set; }
        [Display(Name = "Payment Source Account Code")]
        public string PaymentSourceAccountCode { get; set; }
        [Display(Name = "Amount")]
        public decimal? Amount { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Is Active")]
        public bool? IsActive { get; set; }
        [Display(Name = "Date Added")]
        public DateTime? DateAdded { get; set; }
        [Display(Name = "Date Updated")]
        public DateTime? DateUpdated { get; set; }
        [Display(Name = "Added By")]
        public long? AddedBy { get; set; }
        [Display(Name = "Updated By")]
        public long? UpdatedBy { get; set; }

        public virtual TblAccountVM PaymentSourceAccount { get; set; }
        public virtual SaleContractVM SaleContract { get; set; }
    }
}
