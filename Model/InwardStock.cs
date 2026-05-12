namespace Bill_Master.Model
{
    public class InwardStock
    {
        public int Id { get; set; }

        // 🔑 FK → PurchaseItems (REQUIRED)
        public int PurchaseItemId { get; set; }
        public virtual PurchaseItems? PurchaseItem { get; set; }

        // ⭐ OPTIONAL (batch products only)
        public string? BatchNo { get; set; }

        // ⭐ REQUIRED
        public decimal Qty { get; set; }

        // ⭐ REQUIRED
        public DateTime InwardDate { get; set; }

        // 🔑 FK → StaffMaster (REQUIRED)
        public int StaffUserId { get; set; }
        public virtual StaffMaster? StaffUser { get; set; }
        public int ProductMasterId { get; set; }
        public ProductMaster ProductMaster { get; set; }

        // 🔥 Relation → StockUsed
        public virtual ICollection<StockUsed> StockUseds { get; set; } = new HashSet<StockUsed>();

        // ⭐ OPTIONAL
        public string? Remark { get; set; }
      
    }
}