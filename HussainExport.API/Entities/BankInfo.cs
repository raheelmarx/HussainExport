using System;
using System.Collections.Generic;

#nullable disable

namespace HussainExport.API.Entities
{
    public partial class BankInfo
    {
        public BankInfo()
        {
            SaleContracts = new HashSet<SaleContract>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string BankName { get; set; }
        public string AccountNo { get; set; }
        public string Iban { get; set; }
        public string BranchCode { get; set; }
        public string BranchAddress { get; set; }
        public string SwiftCode { get; set; }

        public virtual ICollection<SaleContract> SaleContracts { get; set; }
    }
}
