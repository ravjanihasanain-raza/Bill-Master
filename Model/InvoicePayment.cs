namespace Bill_Master.Model
{
    public class InvoicePayment
    {
        public int Id { get; set; }

        // 🔑 FK → InvoiceMaster (REQUIRED)
        public int InvoiceMasterId { get; set; }
        public virtual InvoiceMaster? InvoiceMaster { get; set; }

        // ⭐ REQUIRED
        public decimal Amount { get; set; }

        // ⭐ REQUIRED
        public DateTime PaymentDate { get; set; }

        // ⭐ OPTIONAL
        public string? ReferenceNo { get; set; }

        // 🔑 FK → StaffMaster (REQUIRED)
        public int StaffMasterId { get; set; }
        public virtual StaffMaster? StaffMaster { get; set; }

        // ⭐ REQUIRED (Auto)
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}