using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.EntityFrameworkCore;


namespace Bill_Master.ApplicationContext
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<SoftwareSettings> SoftwareSettings { get; set; }
        public DbSet<FinancialYear> FinancialYears { get; set; }
        public DbSet<StaffMaster> StaffMasters { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<ClientMaster> ClientMasters { get; set; }
        public DbSet<ProductMaster> ProductMasters { get; set; }
        public DbSet<PurchaseMaster> PurchaseMasters { get; set; }
        public DbSet<PurchaseItems> PurchaseItems { get; set; }
        public DbSet<PurchasePayment> PurchasePayments { get; set; }
        public DbSet<InwardStock> InwardStocks { get; set; }
        public DbSet<Outward> Outwards { get; set; }
        public DbSet<StockUsed> StockUseds { get; set; }
        public DbSet<InvoiceMaster> InvoiceMasters { get; set; }
        public DbSet<InvoiceItems> InvoiceItems { get; set; }
        public DbSet<InvoicePayment> InvoicePayments { get; set; }
        public DbSet<Stock> Stocks { get; set; }


    }
}