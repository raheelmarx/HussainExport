using System;
using System.Collections.Generic;

namespace HussainExport.API.Entities
{
    public partial class Assets
    {
        public long AssetsId { get; set; }
        public long? TransactionId { get; set; }
        public string AssetsName { get; set; }
        public string AssetsDescription { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? IsActive { get; set; }

        public virtual AccountTransaction Transaction { get; set; }
    }
}
