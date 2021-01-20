using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HussainExport.Client.Models
{
    public class UserVM
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public long? RoleId { get; set; }
    }
}
