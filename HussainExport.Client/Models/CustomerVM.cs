using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HussainExport.Client.Models
{
    public class CustomerVM
    {
       
        public CustomerVM()
        {
            SaleContract = new HashSet<SaleContractVM>();
        }

        [Key]
        public long CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerBussinessDetails { get; set; }
        public string CustomerDescription { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<SaleContractVM> SaleContract { get; set; }

    }
}
