namespace Bill_Master.Model
{
    public class  PurchasePayment

    {
        public int Id { get; set; }

        // 🔑 FK → PurchaseMaster (REQUIRED)
        public int PurchaseMasterId { get; set; }
        public virtual PurchaseMaster? PurchaseMaster { get; set; }

        // ⭐ REQUIRED
        public decimal Amount { get; set; }

        // ⭐ REQUIRED
        public DateTime PaymentDate { get; set; }

        // ⭐ OPTIONAL
        public string? ReferenceNo { get; set; }

        public string? Remarks { get; set; }

        // 🔑 FK → StaffMaster (REQUIRED)
        public int StaffMasterId { get; set; }
        public virtual StaffMaster? StaffMaster { get; set; }

        // ⭐ REQUIRED (Auto)
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}