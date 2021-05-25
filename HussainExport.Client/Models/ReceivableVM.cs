using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HussainExport.Client.Models
{
    public class ReceivableVM
    {
        public ReceivableVM()
        {
            TblAccounts = new HashSet<TblAccountVM>();
        }
        [Key]
        public long ReceivableId { get; set; }
        public string ReceivableName { get; set; }
        public string ReceivableDescription { get; set; }
        public string ReceivableAddress { get; set; }
        public string ReceivablePhone { get; set; }
        public long? CustomerId { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? IsActive { get; set; }

        public virtual CustomerVM Customer { get; set; }
        public virtual ICollection<TblAccountVM> TblAccounts { get; set; }
    }
}
