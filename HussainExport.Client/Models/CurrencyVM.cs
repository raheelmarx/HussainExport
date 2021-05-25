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
        [Display(Name = "Currency")]
        public int CurrencyId { get; set; }
        [Display(Name = "Currency Name")]
        public string CurrencyName { get; set; }
        [Display(Name = "Currency Symbol")]
        public string CurrencySymbol { get; set; }
        public string Description { get; set; }
        [Display(Name = "Is Active")]
        public bool? IsActive { get; set; }
        [Display(Name = "Date Added")]
        public DateTime? DateAdded { get; set; }
        [Display(Name = "Date Updated")]
        public DateTime? DateUpdated { get; set; }

       // public virtual ICollection<SaleContractVM> SaleContract { get; set; }
    }
}
