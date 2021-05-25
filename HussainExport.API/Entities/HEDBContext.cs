using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

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

        public virtual DbSet<AccountTransaction> AccountTransactions { get; set; }
        public virtual DbSet<AccountType> AccountTypes { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<Asset> Assets { get; set; }
        public virtual DbSet<BankInfo> BankInfos { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<FabricPurchase> FabricPurchases { get; set; }
        public virtual DbSet<FabricPurchaseItem> FabricPurchaseItems { get; set; }
        public virtual DbSet<FactoryOverheadExpense> FactoryOverheadExpenses { get; set; }
        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<MigrationHistory> MigrationHistories { get; set; }
        public virtual DbSet<OwnerEquity> OwnerEquities { get; set; }
        public virtual DbSet<Payable> Payables { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }
        public virtual DbSet<PurchaseReceived> PurchaseReceiveds { get; set; }
        public virtual DbSet<Receivable> Receivables { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SaleContract> SaleContracts { get; set; }
        public virtual DbSet<SaleContractExpense> SaleContractExpenses { get; set; }
        public virtual DbSet<SaleContractItem> SaleContractItems { get; set; }
        public virtual DbSet<TblAccount> TblAccounts { get; set; }
        public virtual DbSet<TransactionType> TransactionTypes { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-GA7O6QV\\SQLEXPRESS;Database=HEDB;Trusted_Connection=True;");
               // optionsBuilder.UseSqlServer("Server=.\\MSSQLSERVER2016;Database=HEDB;Trusted_Connection=False;user id=hedbadmin;password=Abcd@1234;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AccountTransaction>(entity =>
            {
                entity.ToTable("Account_Transaction");

                entity.Property(e => e.AccountTransactionId).HasColumnName("Account_Transaction_ID");

                entity.Property(e => e.AccountCreditCode)
                    .HasMaxLength(150)
                    .HasColumnName("Account_Credit_Code");

                entity.Property(e => e.AccountCreditId).HasColumnName("Account_Credit_ID");

                entity.Property(e => e.AccountDebitCode)
                    .HasMaxLength(150)
                    .HasColumnName("Account_Debit_Code");

                entity.Property(e => e.AccountDebitId).HasColumnName("Account_Debit_ID");

                entity.Property(e => e.AmountCredit)
                    .HasColumnType("decimal(18, 4)")
                    .HasColumnName("Amount_Credit");

                entity.Property(e => e.AmountDebit)
                    .HasColumnType("decimal(18, 4)")
                    .HasColumnName("Amount_Debit");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.Narration).HasMaxLength(500);

                entity.Property(e => e.SaleContractNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VoucherDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Voucher_Date");

                entity.Property(e => e.VoucherNo)
                    .HasMaxLength(150)
                    .HasColumnName("Voucher_No");

                entity.HasOne(d => d.AccountCredit)
                    .WithMany(p => p.AccountTransactionAccountCredits)
                    .HasForeignKey(d => d.AccountCreditId)
                    .HasConstraintName("FK_Account_Transaction_tblAccount");

                entity.HasOne(d => d.AccountDebit)
                    .WithMany(p => p.AccountTransactionAccountDebits)
                    .HasForeignKey(d => d.AccountDebitId)
                    .HasConstraintName("FK_Account_Transaction_Account");

                entity.HasOne(d => d.SaleContract)
                    .WithMany(p => p.AccountTransactions)
                    .HasForeignKey(d => d.SaleContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_Transaction_SaleContract");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.AccountTransactions)
                    .HasForeignKey(d => d.Type)
                    .HasConstraintName("FK_Account_Transaction_Transaction_Type");
            });

            modelBuilder.Entity<AccountType>(entity =>
            {
                entity.ToTable("Account_Type");

                entity.Property(e => e.AccountTypeId).HasColumnName("Account_Type_ID");

                entity.Property(e => e.AccountTypeDescription)
                    .HasMaxLength(500)
                    .HasColumnName("Account_Type_Description");

                entity.Property(e => e.AccountTypeName)
                    .HasMaxLength(150)
                    .HasColumnName("Account_Type_Name");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");
            });

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.Name, "RoleNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.UserName, "UserNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.LockoutEndDateUtc).HasColumnType("datetime");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_UserId");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId");
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey, e.UserId })
                    .HasName("PK_dbo.AspNetUserLogins");

                entity.HasIndex(e => e.UserId, "IX_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId");
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK_dbo.AspNetUserRoles");

                entity.HasIndex(e => e.RoleId, "IX_RoleId");

                entity.HasIndex(e => e.UserId, "IX_UserId");

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

            modelBuilder.Entity<Asset>(entity =>
            {
                entity.HasKey(e => e.AssetsId);

                entity.Property(e => e.AssetsId).HasColumnName("Assets_ID");

                entity.Property(e => e.AssetsDescription)
                    .HasMaxLength(500)
                    .HasColumnName("Assets_Description");

                entity.Property(e => e.AssetsName)
                    .HasMaxLength(150)
                    .HasColumnName("Assets_Name");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.TransactionId).HasColumnName("Transaction_ID");

                entity.HasOne(d => d.Transaction)
                    .WithMany(p => p.Assets)
                    .HasForeignKey(d => d.TransactionId)
                    .HasConstraintName("FK_Assets_Account_Transaction");
            });

            modelBuilder.Entity<BankInfo>(entity =>
            {
                entity.ToTable("BankInfo");

                entity.Property(e => e.AccountNo).HasMaxLength(500);

                entity.Property(e => e.BankName).HasMaxLength(500);

                entity.Property(e => e.BranchAddress).HasMaxLength(500);

                entity.Property(e => e.BranchCode).HasMaxLength(50);

                entity.Property(e => e.Iban)
                    .HasMaxLength(500)
                    .HasColumnName("IBAN");

                entity.Property(e => e.SwiftCode).HasMaxLength(500);

                entity.Property(e => e.Title).HasMaxLength(500);
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company");

                entity.Property(e => e.CompanyId).HasColumnName("Company_ID");

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.CompanyAddress)
                    .HasMaxLength(500)
                    .HasColumnName("Company_Address");

                entity.Property(e => e.CompanyDescription)
                    .HasMaxLength(500)
                    .HasColumnName("Company_Description");

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(150)
                    .HasColumnName("Company_Name");

                entity.Property(e => e.CompanyPhone)
                    .HasMaxLength(50)
                    .HasColumnName("Company_Phone");

                entity.Property(e => e.Country).HasMaxLength(50);

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.ToTable("Currency");

                entity.Property(e => e.CurrencyName).HasMaxLength(50);

                entity.Property(e => e.CurrencySymbol).HasMaxLength(5);

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(50);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.Contact).HasMaxLength(50);

                entity.Property(e => e.CustomerBussinessDetails).HasMaxLength(500);

                entity.Property(e => e.CustomerDescription).HasMaxLength(500);

                entity.Property(e => e.CustomerName).HasMaxLength(500);

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);
            });

            modelBuilder.Entity<FabricPurchase>(entity =>
            {
                entity.ToTable("FabricPurchase");

                entity.Property(e => e.Broker)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ContQuality)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ConversionRate)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.DeliveryTime).HasColumnType("datetime");

                entity.Property(e => e.Gstquality)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("GSTQuality");

                entity.Property(e => e.PerMeterRate).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PerPickRate).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.QuantityInMeters).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SaleContractNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.SaleContract)
                    .WithMany(p => p.FabricPurchases)
                    .HasForeignKey(d => d.SaleContractId)
                    .HasConstraintName("FK_FabricPurchase_SaleContract");
            });

            modelBuilder.Entity<FabricPurchaseItem>(entity =>
            {
                entity.ToTable("FabricPurchaseItem");

                entity.Property(e => e.CountMargin).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.FabricRatePerMeter).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RequiredBags).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.WeightPerMeterIbs).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Yarn)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.YarnRatePerIbs).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.FabricPurchase)
                    .WithMany(p => p.FabricPurchaseItems)
                    .HasForeignKey(d => d.FabricPurchaseId)
                    .HasConstraintName("FK_FabricPurchaseItem_FabricPurchase");
            });

            modelBuilder.Entity<FactoryOverheadExpense>(entity =>
            {
                entity.HasKey(e => e.Fohid);

                entity.ToTable("FactoryOverheadExpense");

                entity.Property(e => e.Fohid).HasColumnName("FOHId");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.PaymentSourceAccountCode).HasMaxLength(150);

                entity.HasOne(d => d.PaymentSourceAccount)
                    .WithMany(p => p.FactoryOverheadExpenses)
                    .HasForeignKey(d => d.PaymentSourceAccountId)
                    .HasConstraintName("FK_FactoryOverheadExpense_tblAccount");
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.ToTable("Inventory");

                entity.Property(e => e.InventoryId).HasColumnName("Inventory_ID");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.InventoryDescription)
                    .HasMaxLength(500)
                    .HasColumnName("Inventory_Description");

                entity.Property(e => e.InventoryName)
                    .HasMaxLength(150)
                    .HasColumnName("Inventory_Name");

                entity.Property(e => e.InventoryPerUnitCost)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("Inventory_PerUnitCost");

                entity.Property(e => e.InventoryQuantity)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("Inventory_Quantity");

                entity.Property(e => e.InventoryTotalAmount)
                    .HasMaxLength(150)
                    .HasColumnName("Inventory_TotalAmount");

                entity.Property(e => e.TransactionId).HasColumnName("Transaction_ID");

                entity.HasOne(d => d.Transaction)
                    .WithMany(p => p.Inventories)
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
                entity.ToTable("OwnerEquity");

                entity.Property(e => e.OwnerEquityId).HasColumnName("OwnerEquity_ID");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.OwnerEquityDescription)
                    .HasMaxLength(500)
                    .HasColumnName("OwnerEquity_Description");

                entity.Property(e => e.OwnerEquityName)
                    .HasMaxLength(150)
                    .HasColumnName("OwnerEquity_Name");

                entity.Property(e => e.TransactionId).HasColumnName("Transaction_ID");

                entity.HasOne(d => d.Transaction)
                    .WithMany(p => p.OwnerEquities)
                    .HasForeignKey(d => d.TransactionId)
                    .HasConstraintName("FK_OwnerEquity_Account_Transaction");
            });

            modelBuilder.Entity<Payable>(entity =>
            {
                entity.ToTable("Payable");

                entity.Property(e => e.PayableId).HasColumnName("Payable_ID");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.PayableAddress)
                    .HasMaxLength(500)
                    .HasColumnName("Payable_Address");

                entity.Property(e => e.PayableDescription)
                    .HasMaxLength(500)
                    .HasColumnName("Payable_Description");

                entity.Property(e => e.PayableName)
                    .HasMaxLength(150)
                    .HasColumnName("Payable_Name");

                entity.Property(e => e.PayablePhone)
                    .HasMaxLength(50)
                    .HasColumnName("Payable_Phone");
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.ToTable("Purchase");

                entity.Property(e => e.PurchaseId).HasColumnName("Purchase_ID");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.Gazana).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.Meter).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PayableId).HasColumnName("Payable_ID");

                entity.Property(e => e.PurchaseDescription)
                    .HasMaxLength(500)
                    .HasColumnName("Purchase_Description");

                entity.Property(e => e.PurchasePerUnitCost)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("Purchase_PerUnitCost");

                entity.Property(e => e.PurchaseTitle)
                    .HasMaxLength(150)
                    .HasColumnName("Purchase_Title");

                entity.Property(e => e.PurchaseTotalAmount)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("Purchase_TotalAmount");

                entity.Property(e => e.PurchaseTotalQuantity)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("Purchase_TotalQuantity");

                entity.Property(e => e.Quality).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Than).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Payable)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => d.PayableId)
                    .HasConstraintName("FK_Purchase_Payable");
            });

            modelBuilder.Entity<PurchaseReceived>(entity =>
            {
                entity.ToTable("PurchaseReceived");

                entity.Property(e => e.PurchaseReceivedId).HasColumnName("PurchaseReceived_ID");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.Gain).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Gazana).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.Meter).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PurchaseId).HasColumnName("Purchase_ID");

                entity.Property(e => e.PurchaseReceivedPerUnitCost)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("PurchaseReceived_PerUnitCost");

                entity.Property(e => e.PurchaseReceivedTotalAmount)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("PurchaseReceived_TotalAmount");

                entity.Property(e => e.Quality).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.QuantityReceived).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Rejection).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Than).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Purchase)
                    .WithMany(p => p.PurchaseReceiveds)
                    .HasForeignKey(d => d.PurchaseId)
                    .HasConstraintName("FK_PurchaseReceived_Purchase");
            });

            modelBuilder.Entity<Receivable>(entity =>
            {
                entity.ToTable("Receivable");

                entity.Property(e => e.ReceivableId).HasColumnName("Receivable_ID");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.ReceivableAddress)
                    .HasMaxLength(500)
                    .HasColumnName("Receivable_Address");

                entity.Property(e => e.ReceivableDescription)
                    .HasMaxLength(500)
                    .HasColumnName("Receivable_Description");

                entity.Property(e => e.ReceivableName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("Receivable_Name");

                entity.Property(e => e.ReceivablePhone)
                    .HasMaxLength(50)
                    .HasColumnName("Receivable_Phone");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Receivables)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Receivable_Customer");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<SaleContract>(entity =>
            {
                entity.ToTable("SaleContract");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.DeliverySchedule).HasMaxLength(500);

                entity.Property(e => e.Packing).HasMaxLength(1000);

                entity.Property(e => e.PaymentTerms).HasMaxLength(500);

                entity.Property(e => e.SaleContractNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShipmentDetails).HasMaxLength(1000);

                entity.Property(e => e.Tolerance).HasMaxLength(500);

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.TotalFabric).HasMaxLength(250);

                entity.HasOne(d => d.BankDetailsNavigation)
                    .WithMany(p => p.SaleContracts)
                    .HasForeignKey(d => d.BankDetails)
                    .HasConstraintName("FK_SaleContract_SaleContract");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.SaleContracts)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_SaleContract_Currency");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.SaleContracts)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_SaleContract_Customer");
            });

            modelBuilder.Entity<SaleContractExpense>(entity =>
            {
                entity.HasKey(e => e.ExpenseId);

                entity.ToTable("SaleContractExpense");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.PaymentSourceAccountCode).HasMaxLength(150);

                entity.Property(e => e.SaleContractNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.PaymentSourceAccount)
                    .WithMany(p => p.SaleContractExpenses)
                    .HasForeignKey(d => d.PaymentSourceAccountId)
                    .HasConstraintName("FK_SaleContractExpense_tblAccount");

                entity.HasOne(d => d.SaleContract)
                    .WithMany(p => p.SaleContractExpenses)
                    .HasForeignKey(d => d.SaleContractId)
                    .HasConstraintName("FK_SaleContractExpense_SaleContract");
            });

            modelBuilder.Entity<SaleContractItem>(entity =>
            {
                entity.ToTable("SaleContractItem");

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
                    .WithMany(p => p.SaleContractItems)
                    .HasForeignKey(d => d.SaleContractId)
                    .HasConstraintName("FK_SaleContractItem_SaleContract");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.SaleContractItems)
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
                    .HasMaxLength(150)
                    .HasColumnName("Account_Code");

                entity.Property(e => e.AccountDescription)
                    .HasMaxLength(500)
                    .HasColumnName("Account_Description");

                entity.Property(e => e.AccountTitle)
                    .HasMaxLength(150)
                    .HasColumnName("Account_Title");

                entity.Property(e => e.AccountTypeId).HasColumnName("Account_Type_ID");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.PayableId).HasColumnName("Payable_ID");

                entity.Property(e => e.ReceivablesId).HasColumnName("Receivables_ID");

                entity.HasOne(d => d.AccountType)
                    .WithMany(p => p.TblAccounts)
                    .HasForeignKey(d => d.AccountTypeId)
                    .HasConstraintName("FK_tblAccount_Account_Type");

                entity.HasOne(d => d.Payable)
                    .WithMany(p => p.TblAccounts)
                    .HasForeignKey(d => d.PayableId)
                    .HasConstraintName("FK_tblAccount_Payable");

                entity.HasOne(d => d.Receivables)
                    .WithMany(p => p.TblAccounts)
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
                    .HasMaxLength(50)
                    .HasColumnName("Transaction_Type_Name");
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.ToTable("Unit");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.UnitName).HasMaxLength(50);

                entity.Property(e => e.UnitSymbol).HasMaxLength(5);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.UserName).HasMaxLength(100);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_User_Role");
            });

            modelBuilder.Entity<Vendor>(entity =>
            {
                entity.ToTable("Vendor");

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.Contact).HasMaxLength(50);

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.VendorBussinessDetails).HasMaxLength(500);

                entity.Property(e => e.VendorDescription).HasMaxLength(500);

                entity.Property(e => e.VendorName).HasMaxLength(500);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
