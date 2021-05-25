using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HussainExport.Client.Models
{
    public class SaleContractVM
    {
        //public SaleContractVM()
        //{
        //    SaleContractItem = new HashSet<SaleContractItemVM>();
        //}
        //[Key]
        //public long SaleContractId { get; set; }
        //public string SaleContractNumber { get; set; }
        //public long? CustomerId { get; set; }
        //public decimal? TotalAmount { get; set; }
        //public int? CurrencyId { get; set; }
        //public string DeliveryTime { get; set; }
        //public string ShipmentDetails { get; set; }
        //public string Packing { get; set; }
        //public string Description { get; set; }
        //public bool? IsActive { get; set; }
        //public DateTime? DateAdded { get; set; }
        //public DateTime? DateUpdated { get; set; }
        //public virtual CurrencyVM Currency { get; set; }
        //public virtual CustomerVM Customer { get; set; }
        //public virtual ICollection<SaleContractItemVM> SaleContractItem { get; set; }



        public SaleContractVM()
        {
            //AccountTransactions = new HashSet<AccountTransactionVM>();
            FabricPurchases = new HashSet<FabricPurchaseVM>();
            SaleContractItems = new HashSet<SaleContractItemVM>();
        }
        [Key]
        [Display(Name = "Sale Contract")]
        public long SaleContractId { get; set; }
        [Display(Name = "Sale Contract Number")]
        public string SaleContractNumber { get; set; }
        [Display(Name = "Customer")]
        public long? CustomerId { get; set; }
        [Display(Name = "Total Amount")]
        public decimal? TotalAmount { get; set; }
        [Display(Name = "Total Fabric")]
        public string TotalFabric { get; set; }
        public string Tolerance { get; set; }
        public string Packing { get; set; }
        [Display(Name = "Shipment Details")]
        public string ShipmentDetails { get; set; }
        [Display(Name = "Payment Terms")]
        public string PaymentTerms { get; set; }
        [Display(Name = "Delivery Schedule")]
        public string DeliverySchedule { get; set; }
        [Display(Name = "Currency")]
        public int? CurrencyId { get; set; }
        public string Description { get; set; }
        [Display(Name = "Bank Account")]
        public int? BankDetails { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public long? AddedBy { get; set; }
        public long? UpdatedBy { get; set; }

        public virtual BankInfoVM BankDetailsNavigation { get; set; }
        public virtual CurrencyVM Currency { get; set; }
        public virtual CustomerVM Customer { get; set; }
        //public virtual ICollection<AccountTransaction> AccountTransactions { get; set; }
        public virtual ICollection<FabricPurchaseVM> FabricPurchases { get; set; }
        public virtual ICollection<SaleContractItemVM> SaleContractItems { get; set; }
    }
}
