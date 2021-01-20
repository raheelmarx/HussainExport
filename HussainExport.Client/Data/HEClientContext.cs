using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HussainExport.Client.Models;

namespace HussainExport.Client.Data
{
    public class HEClientContext : DbContext
    {
        public HEClientContext (DbContextOptions<HEClientContext> options)
            : base(options)
        {
        }

        public DbSet<HussainExport.Client.Models.RoleVM> RoleVM { get; set; }
    }
}
