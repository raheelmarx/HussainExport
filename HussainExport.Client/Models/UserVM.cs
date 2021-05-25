using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HussainExport.Client.Models
{
    [Display(Name = "User")]
    public class UserVM
    {
        public long Id { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "Token")]
        public string Token { get; set; }
        [Display(Name = "Role")]
        public long? RoleId { get; set; }
        public virtual RoleVM Role { get; set; }
    }
}
