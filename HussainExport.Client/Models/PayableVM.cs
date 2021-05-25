using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HussainExport.Client.Models
{
    public class PayableVM
    {
        public PayableVM()
        {
           // Purchases = new HashSet<Purchase>();
            TblAccounts = new HashSet<TblAccountVM>();
        }
        [Key]
        public long PayableId { get; set; }
        public string PayableName { get; set; }
        public string PayableDescription { get; set; }
        public string PayableAddress { get; set; }
        public string PayablePhone { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? IsActive { get; set; }

        //public virtual ICollection<Purchase> Purchases { get; set; }
        public virtual ICollection<TblAccountVM> TblAccounts { get; set; }
    }
}
