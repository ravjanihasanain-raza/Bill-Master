namespace Bill_Master.Model
    
{
    using System.Text.Json.Serialization;
    public class PurchaseItems
    {
        public PurchaseItems()
        {
            InwardStocks = new HashSet<InwardStock>();
        }

        public int Id { get; set; }

        // 🔑 FK → ProductMaster (REQUIRED)
        public int ProductMasterId { get; set; }
        public virtual ProductMaster? ProductMaster { get; set; }

        // ⭐ OPTIONAL
        public string? HSNCode { get; set; }

        // ⭐ REQUIRED
        public decimal Rate { get; set; }

        // ⭐ OPTIONAL
        public decimal? Discount { get; set; }

        // ⭐ REQUIRED (Taxable Amount)
        public decimal Amount { get; set; }

        // ⭐ REQUIRED
        public decimal Qty { get; set; }

        // ⭐ REQUIRED (Final Line Total)
        public decimal Total { get; set; }

        // 🔑 FK → PurchaseMaster (REQUIRED)
        public int PurchaseMasterId { get; set; }

        [JsonIgnore]   // 🔥 ADD THIS LINE
        public virtual PurchaseMaster? PurchaseMaster { get; set; }
        public ICollection<InwardStock> InwardStocks { get; set; }
    }
}