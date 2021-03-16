using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HussainExport.Client.Models
{
    public class SaleContractVM
    {
        public SaleContractVM()
        {
            SaleContractItem = new HashSet<SaleContractItemVM>();
        }
        [Key]
        public long SaleContractId { get; set; }
        public string SaleContractNumber { get; set; }
        public long? CustomerId { get; set; }
        public decimal? TotalAmount { get; set; }
        public int? CurrencyId { get; set; }
        public string DeliveryTime { get; set; }
        public string ShipmentDetails { get; set; }
        public string Packing { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public virtual CurrencyVM Currency { get; set; }
        public virtual CustomerVM Customer { get; set; }
        public virtual ICollection<SaleContractItemVM> SaleContractItem { get; set; }
    }
}
