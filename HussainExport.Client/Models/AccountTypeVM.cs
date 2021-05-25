using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HussainExport.Client.Models
{
    public class AccountTypeVM
    {
        public AccountTypeVM()
        {
            TblAccounts = new HashSet<TblAccountVM>();
        }

        [Key]
        public long AccountTypeId { get; set; }
        public string AccountTypeName { get; set; }
        public string AccountTypeDescription { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<TblAccountVM> TblAccounts { get; set; }
    }
}
