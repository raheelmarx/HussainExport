using System;
using System.Collections.Generic;

#nullable disable

namespace HussainExport.API.Entities
{
    public partial class FactoryOverheadExpense
    {
        public long Fohid { get; set; }
        public long? PaymentSourceAccountId { get; set; }
        public string PaymentSourceAccountCode { get; set; }
        public decimal? Amount { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public long? AddedBy { get; set; }
        public long? UpdatedBy { get; set; }

        public virtual TblAccount PaymentSourceAccount { get; set; }
    }
}
