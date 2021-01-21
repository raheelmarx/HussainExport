using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HussainExport.Client.Models
{
    public class CurrencyVM
    {
        //public CurrencyVM()
        //{
        //    SaleContract = new HashSet<SaleContractVM>();
        //}
        [Key]
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

       // public virtual ICollection<SaleContractVM> SaleContract { get; set; }
    }
}
