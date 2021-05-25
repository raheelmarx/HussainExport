using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HussainExport.Client.Models
{
    public class TransactionTypeVM
    {
        public TransactionTypeVM()
        {
            AccountTransactions = new HashSet<AccountTransactionVM>();
        }
        [Key]
        public long TransactionTypeId { get; set; }
        public string TransactionTypeName { get; set; }
        public string Description { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<AccountTransactionVM> AccountTransactions { get; set; }
    }
}
