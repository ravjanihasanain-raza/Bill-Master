namespace Bill_Master.Model
{
    public class PurchaseMaster
    {
        public int Id { get; set; }

        // 🔑 FK → FinancialYear (REQUIRED)
        public int FinancialYearId { get; set; }
        public virtual FinancialYear? FinancialYear { get; set; }

        // ⭐ REQUIRED
        public string BillNo { get; set; } = string.Empty;

        // ⭐ REQUIRED
        public DateTime BillDate { get; set; }

        // ⭐ REQUIRED
        public decimal GrossAmount { get; set; }

        // ⭐ REQUIRED
        public decimal GstAmount { get; set; }

        // ⭐ REQUIRED
        public decimal Total { get; set; }

        // ⭐ REQUIRED
        public string GstType { get; set; } = string.Empty;

        // ⭐ OPTIONAL
        public string? EwayBillNo { get; set; }

        public string? PlaceOfSupply { get; set; }

        public string? TransportName { get; set; }

        public string? TransportMobile { get; set; }

        public string? VehicleNo { get; set; }

        // 🔑 FK → Vendor (REQUIRED)
        public int VendorId { get; set; }
        public virtual Vendor? Vendor { get; set; }

        // 🔑 FK → StaffMaster (REQUIRED)
        public int? StaffMasterId { get; set; }
        public virtual StaffMaster? StaffMaster { get; set; }

        // ⭐ REQUIRED (auto)
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        // 🔥 FK RELATION → PurchaseItems
        public ICollection<PurchaseItems> PurchaseItems { get; set; }
            = new HashSet<PurchaseItems>();
        public ICollection<PurchasePayment> PurchasePayments { get; set; }
             = new HashSet<PurchasePayment>();
    }
}