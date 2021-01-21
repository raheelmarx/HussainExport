using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HussainExport.Client.Models
{
    public class SaleContractItemVM
    {
        [Key]
        public long SaleContractItemId { get; set; }
        public long? SaleContractId { get; set; }
        public string Quality { get; set; }
        public string Article { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public int? UnitId { get; set; }
        public string Price { get; set; }
        public string Quantity { get; set; }
        public string Amount { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public virtual SaleContractVM SaleContract { get; set; }
        public virtual UnitVM Unit { get; set; }

    }
}
