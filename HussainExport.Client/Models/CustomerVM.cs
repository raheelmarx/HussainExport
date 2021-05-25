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
        [Display(Name = "Customer")]
        public long CustomerId { get; set; }
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
        [Display(Name = "Customer Bussiness Details")]
        public string CustomerBussinessDetails { get; set; }
        [Display(Name = "Customer Description")]
        public string CustomerDescription { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        [Display(Name = "Is Active")]
        public bool? IsActive { get; set; }
        [Display(Name = "Date Added")]
        public DateTime? DateAdded { get; set; }
        [Display(Name = "Date Updated")]
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<SaleContractVM> SaleContract { get; set; }

    }
}
