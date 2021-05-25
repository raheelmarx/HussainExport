using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HussainExport.Client.Models
{
    public class RoleVM
    {
        //[Key]
        //public long Id { get; set; }
        //public string Name { get; set; }
        //public bool? IsActive { get; set; }
        //public DateTime? DateAdded { get; set; }
        //public DateTime? DateUpdated { get; set; }

        public RoleVM()
        {
            Users = new HashSet<UserVM>();
        }

        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<UserVM> Users { get; set; }
    }
}
