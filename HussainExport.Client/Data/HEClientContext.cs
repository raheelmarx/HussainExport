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

        public DbSet<HussainExport.Client.Models.SaleContractVM> SaleContractVM { get; set; }

        public DbSet<HussainExport.Client.Models.CurrencyVM> CurrencyVM { get; set; }

        public DbSet<HussainExport.Client.Models.CustomerVM> CustomerVM { get; set; }

        public DbSet<HussainExport.Client.Models.UnitVM> UnitVM { get; set; }

        public DbSet<HussainExport.Client.Models.SaleContractItemVM> SaleContractItemVM { get; set; }

        public DbSet<HussainExport.Client.Models.FabricPurchaseVM> FabricPurchaseVM { get; set; }

        public DbSet<HussainExport.Client.Models.FabricPurchaseItemVM> FabricPurchaseItemVM { get; set; }
    }
}
