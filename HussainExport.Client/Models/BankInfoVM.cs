using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HussainExport.Client.Models
{
    public class BankInfoVM
    {
        public BankInfoVM()
        {
            SaleContracts = new HashSet<SaleContractVM>();
        }
        [Key]
        [Display(Name = "Bank")]
        public int Id { get; set; }
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }
        [Display(Name = "Account No")]
        public string AccountNo { get; set; }
        [Display(Name = "IBAN")]
        public string Iban { get; set; }
        [Display(Name = "Branch Code")]
        public string BranchCode { get; set; }
        [Display(Name = "Branch Address")]
        public string BranchAddress { get; set; }
        [Display(Name = "Swift Code")]
        public string SwiftCode { get; set; }

        public virtual ICollection<SaleContractVM> SaleContracts { get; set; }
    }
}
