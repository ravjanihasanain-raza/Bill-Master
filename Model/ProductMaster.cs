namespace Bill_Master.Model
{
    public class ProductMaster
    {
        // 🔥 FK RELATION → PurchaseItems
        public ICollection<PurchaseItems> PurchaseItems { get; set; }
            = new HashSet<PurchaseItems>();
        public int Id { get; set; }

        // ⭐ REQUIRED
        public string Code { get; set; } = string.Empty;

        // ⭐ REQUIRED
        public string Name { get; set; } = string.Empty;

        // 🔑 FOREIGN KEY COLUMN
        public int ProductCategoryId { get; set; }

        // 🔗 NAVIGATION PROPERTY (FK relation)
        public virtual ProductCategory? ProductCategory { get; set; }

        // ⭐ OPTIONAL
        public string? Unit { get; set; }

        // ⭐ REQUIRED
        public decimal Price { get; set; }

        // ⭐ REQUIRED
        public decimal CostPrice { get; set; }

        // ⭐ REQUIRED
        public decimal Gst { get; set; }

        // ⭐ REQUIRED
        public string HSN { get; set; } = string.Empty;
        public decimal MinimumStock { get; set; } = 0;
        // 🔥 Relation → InvoiceItems
        public ICollection<InvoiceItems> InvoiceItems { get; set; }
            = new HashSet<InvoiceItems>();
    }
}
