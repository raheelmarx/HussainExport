using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HussainExport.API.Entities
{
    public partial class HEDBContext : DbContext
    {
        public HEDBContext()
        {
        }

        public HEDBContext(DbContextOptions<HEDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccountTransaction> AccountTransaction { get; set; }
        public virtual DbSet<AccountType> AccountType { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Assets> Assets { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<Currency> Currency { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Inventory> Inventory { get; set; }
        public virtual DbSet<MigrationHistory> MigrationHistory { get; set; }
        public virtual DbSet<OwnerEquity> OwnerEquity { get; set; }
        public virtual DbSet<Payable> Payable { get; set; }
        public virtual DbSet<Purchase> Purchase { get; set; }
        public virtual DbSet<PurchaseReceived> PurchaseReceived { get; set; }
        public virtual DbSet<Receivable> Receivable { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<SaleContract> SaleContract { get; set; }
        public virtual DbSet<SaleContractItem> SaleContractItem { get; set; }
        public virtual DbSet<TblAccount> TblAccount { get; set; }
        public virtual DbSet<TransactionType> TransactionType { get; set; }
        public virtual DbSet<Unit> Unit { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-GA7O6QV\\SQLEXPRESS;Database=HEDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountTransaction>(entity =>
            {
                entity.ToTable("Account_Transaction");

                entity.Property(e => e.AccountTransactionId).HasColumnName("Account_Transaction_ID");

                entity.Property(e => e.AccountCreditCode)
                    .HasColumnName("Account_Credit_Code")
                    .HasMaxLength(150);

                entity.Property(e => e.AccountCreditId).HasColumnName("Account_Credit_ID");

                entity.Property(e => e.AccountDebitCode)
                    .HasColumnName("Account_Debit_Code")
                    .HasMaxLength(150);

                entity.Property(e => e.AccountDebitId).HasColumnName("Account_Debit_ID");

                entity.Property(e => e.AmountCredit)
                    .HasColumnName("Amount_Credit")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.AmountDebit)
                    .HasColumnName("Amount_Debit")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.Narration).HasMaxLength(500);

                entity.Property(e => e.SaleContractNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VoucherDate)
                    .HasColumnName("Voucher_Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.VoucherNo)
                    .HasColumnName("Voucher_No")
                    .HasMaxLength(150);

                entity.HasOne(d => d.AccountCredit)
                    .WithMany(p => p.AccountTransactionAccountCredit)
                    .HasForeignKey(d => d.AccountCreditId)
                    .HasConstraintName("FK_Account_Transaction_tblAccount");

                entity.HasOne(d => d.AccountDebit)
                    .WithMany(p => p.AccountTransactionAccountDebit)
                    .HasForeignKey(d => d.AccountDebitId)
                    .HasConstraintName("FK_Account_Transaction_Account");

                entity.HasOne(d => d.SaleContract)
                    .WithMany(p => p.AccountTransaction)
                    .HasForeignKey(d => d.SaleContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_Transaction_SaleContract");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.AccountTransaction)
                    .HasForeignKey(d => d.Type)
                    .HasConstraintName("FK_Account_Transaction_Transaction_Type");
            });

            modelBuilder.Entity<AccountType>(entity =>
            {
                entity.ToTable("Account_Type");

                entity.Property(e => e.AccountTypeId).HasColumnName("Account_Type_ID");

                entity.Property(e => e.AccountTypeDescription)
                    .HasColumnName("Account_Type_Description")
                    .HasMaxLength(500);

                entity.Property(e => e.AccountTypeName)
                    .HasColumnName("Account_Type_Name")
                    .HasMaxLength(150);

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("RoleNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId)
                    .HasName("IX_UserId");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId");
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey, e.UserId })
                    .HasName("PK_dbo.AspNetUserLogins");

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId");
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK_dbo.AspNetUserRoles");

                entity.HasIndex(e => e.RoleId)
                    .HasName("IX_RoleId");

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_UserId");

                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.Property(e => e.RoleId).HasMaxLength(128);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId");
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.UserName)
                    .HasName("UserNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.LockoutEndDateUtc).HasColumnType("datetime");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<Assets>(entity =>
            {
                entity.Property(e => e.AssetsId).HasColumnName("Assets_ID");

                entity.Property(e => e.AssetsDescription)
                    .HasColumnName("Assets_Description")
                    .HasMaxLength(500);

                entity.Property(e => e.AssetsName)
                    .HasColumnName("Assets_Name")
                    .HasMaxLength(150);

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.TransactionId).HasColumnName("Transaction_ID");

                entity.HasOne(d => d.Transaction)
                    .WithMany(p => p.Assets)
                    .HasForeignKey(d => d.TransactionId)
                    .HasConstraintName("FK_Assets_Account_Transaction");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.CompanyId).HasColumnName("Company_ID");

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.CompanyAddress)
                    .HasColumnName("Company_Address")
                    .HasMaxLength(500);

                entity.Property(e => e.CompanyDescription)
                    .HasColumnName("Company_Description")
                    .HasMaxLength(500);

                entity.Property(e => e.CompanyName)
                    .HasColumnName("Company_Name")
                    .HasMaxLength(150);

                entity.Property(e => e.CompanyPhone)
                    .HasColumnName("Company_Phone")
                    .HasMaxLength(50);

                entity.Property(e => e.Country).HasMaxLength(50);

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.Property(e => e.CurrencyName).HasMaxLength(50);

                entity.Property(e => e.CurrencySymbol).HasMaxLength(5);

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(50);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.Contact).HasMaxLength(50);

                entity.Property(e => e.CustomerBussinessDetails).HasMaxLength(500);

                entity.Property(e => e.CustomerDescription).HasMaxLength(500);

                entity.Property(e => e.CustomerName).HasMaxLength(500);

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.Property(e => e.InventoryId).HasColumnName("Inventory_ID");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.InventoryDescription)
                    .HasColumnName("Inventory_Description")
                    .HasMaxLength(500);

                entity.Property(e => e.InventoryName)
                    .HasColumnName("Inventory_Name")
                    .HasMaxLength(150);

                entity.Property(e => e.InventoryPerUnitCost)
                    .HasColumnName("Inventory_PerUnitCost")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.InventoryQuantity)
                    .HasColumnName("Inventory_Quantity")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.InventoryTotalAmount)
                    .HasColumnName("Inventory_TotalAmount")
                    .HasMaxLength(150);

                entity.Property(e => e.TransactionId).HasColumnName("Transaction_ID");

                entity.HasOne(d => d.Transaction)
                    .WithMany(p => p.Inventory)
                    .HasForeignKey(d => d.TransactionId)
                    .HasConstraintName("FK_Inventory_Account_Transaction");
            });

            modelBuilder.Entity<MigrationHistory>(entity =>
            {
                entity.HasKey(e => new { e.MigrationId, e.ContextKey })
                    .HasName("PK_dbo.__MigrationHistory");

                entity.ToTable("__MigrationHistory");

                entity.Property(e => e.MigrationId).HasMaxLength(150);

                entity.Property(e => e.ContextKey).HasMaxLength(300);

                entity.Property(e => e.Model).IsRequired();

                entity.Property(e => e.ProductVersion)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<OwnerEquity>(entity =>
            {
                entity.Property(e => e.OwnerEquityId).HasColumnName("OwnerEquity_ID");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.OwnerEquityDescription)
                    .HasColumnName("OwnerEquity_Description")
                    .HasMaxLength(500);

                entity.Property(e => e.OwnerEquityName)
                    .HasColumnName("OwnerEquity_Name")
                    .HasMaxLength(150);

                entity.Property(e => e.TransactionId).HasColumnName("Transaction_ID");

                entity.HasOne(d => d.Transaction)
                    .WithMany(p => p.OwnerEquity)
                    .HasForeignKey(d => d.TransactionId)
                    .HasConstraintName("FK_OwnerEquity_Account_Transaction");
            });

            modelBuilder.Entity<Payable>(entity =>
            {
                entity.Property(e => e.PayableId).HasColumnName("Payable_ID");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.PayableAddress)
                    .HasColumnName("Payable_Address")
                    .HasMaxLength(500);

                entity.Property(e => e.PayableDescription)
                    .HasColumnName("Payable_Description")
                    .HasMaxLength(500);

                entity.Property(e => e.PayableName)
                    .HasColumnName("Payable_Name")
                    .HasMaxLength(150);

                entity.Property(e => e.PayablePhone)
                    .HasColumnName("Payable_Phone")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.Property(e => e.PurchaseId).HasColumnName("Purchase_ID");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.Gazana).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.Meter).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PayableId).HasColumnName("Payable_ID");

                entity.Property(e => e.PurchaseDescription)
                    .HasColumnName("Purchase_Description")
                    .HasMaxLength(500);

                entity.Property(e => e.PurchasePerUnitCost)
                    .HasColumnName("Purchase_PerUnitCost")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PurchaseTitle)
                    .HasColumnName("Purchase_Title")
                    .HasMaxLength(150);

                entity.Property(e => e.PurchaseTotalAmount)
                    .HasColumnName("Purchase_TotalAmount")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PurchaseTotalQuantity)
                    .HasColumnName("Purchase_TotalQuantity")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Quality).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Than).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Payable)
                    .WithMany(p => p.Purchase)
                    .HasForeignKey(d => d.PayableId)
                    .HasConstraintName("FK_Purchase_Payable");
            });

            modelBuilder.Entity<PurchaseReceived>(entity =>
            {
                entity.Property(e => e.PurchaseReceivedId).HasColumnName("PurchaseReceived_ID");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.Gain).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Gazana).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.Meter).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PurchaseId).HasColumnName("Purchase_ID");

                entity.Property(e => e.PurchaseReceivedPerUnitCost)
                    .HasColumnName("PurchaseReceived_PerUnitCost")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PurchaseReceivedTotalAmount)
                    .HasColumnName("PurchaseReceived_TotalAmount")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Quality).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.QuantityReceived).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Rejection).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Than).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Purchase)
                    .WithMany(p => p.PurchaseReceived)
                    .HasForeignKey(d => d.PurchaseId)
                    .HasConstraintName("FK_PurchaseReceived_Purchase");
            });

            modelBuilder.Entity<Receivable>(entity =>
            {
                entity.Property(e => e.ReceivableId).HasColumnName("Receivable_ID");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.ReceivableAddress)
                    .HasColumnName("Receivable_Address")
                    .HasMaxLength(500);

                entity.Property(e => e.ReceivableDescription)
                    .HasColumnName("Receivable_Description")
                    .HasMaxLength(500);

                entity.Property(e => e.ReceivableName)
                    .IsRequired()
                    .HasColumnName("Receivable_Name")
                    .HasMaxLength(150);

                entity.Property(e => e.ReceivablePhone)
                    .HasColumnName("Receivable_Phone")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Receivable)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Receivable_Customer");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<SaleContract>(entity =>
            {
                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.DeliveryTime).HasMaxLength(500);

                entity.Property(e => e.Packing).HasMaxLength(1000);

                entity.Property(e => e.SaleContractNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShipmentDetails).HasMaxLength(1000);

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 4)");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.SaleContract)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_SaleContract_Currency");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.SaleContract)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_SaleContract_Customer");
            });

            modelBuilder.Entity<SaleContractItem>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.Article).HasMaxLength(500);

                entity.Property(e => e.Color).HasMaxLength(50);

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.Quality).HasMaxLength(500);

                entity.Property(e => e.Quantity).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.Size).HasMaxLength(50);

                entity.HasOne(d => d.SaleContract)
                    .WithMany(p => p.SaleContractItem)
                    .HasForeignKey(d => d.SaleContractId)
                    .HasConstraintName("FK_SaleContractItem_SaleContract");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.SaleContractItem)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("FK_SaleContractItem_SaleContractItem");
            });

            modelBuilder.Entity<TblAccount>(entity =>
            {
                entity.HasKey(e => e.AccountId)
                    .HasName("PK_Account");

                entity.ToTable("tblAccount");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.AccountCode)
                    .HasColumnName("Account_Code")
                    .HasMaxLength(150);

                entity.Property(e => e.AccountDescription)
                    .HasColumnName("Account_Description")
                    .HasMaxLength(500);

                entity.Property(e => e.AccountTitle)
                    .HasColumnName("Account_Title")
                    .HasMaxLength(150);

                entity.Property(e => e.AccountTypeId).HasColumnName("Account_Type_ID");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.PayableId).HasColumnName("Payable_ID");

                entity.Property(e => e.ReceivablesId).HasColumnName("Receivables_ID");

                entity.HasOne(d => d.AccountType)
                    .WithMany(p => p.TblAccount)
                    .HasForeignKey(d => d.AccountTypeId)
                    .HasConstraintName("FK_tblAccount_Account_Type");

                entity.HasOne(d => d.Payable)
                    .WithMany(p => p.TblAccount)
                    .HasForeignKey(d => d.PayableId)
                    .HasConstraintName("FK_tblAccount_Payable");

                entity.HasOne(d => d.Receivables)
                    .WithMany(p => p.TblAccount)
                    .HasForeignKey(d => d.ReceivablesId)
                    .HasConstraintName("FK_tblAccount_Receivable");
            });

            modelBuilder.Entity<TransactionType>(entity =>
            {
                entity.ToTable("Transaction_Type");

                entity.Property(e => e.TransactionTypeId).HasColumnName("Transaction_Type_Id");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(150);

                entity.Property(e => e.TransactionTypeName)
                    .HasColumnName("Transaction_Type_Name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.UnitName).HasMaxLength(50);

                entity.Property(e => e.UnitSymbol).HasMaxLength(5);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.UserName).HasMaxLength(100);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_User_Role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
