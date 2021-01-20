using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HussainExport.Client.Models
{
    public class RoleVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
