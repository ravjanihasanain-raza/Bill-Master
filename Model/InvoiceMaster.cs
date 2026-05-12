using System;
using System.ComponentModel.DataAnnotations;

namespace Bill_Master.Model
{
    public class InvoiceMaster
    {
        public int Id { get; set; }

        // ⭐ Invoice Number (should be unique)
        public string InvoiceNo { get; set; } = string.Empty;

        // 🔑 FK → ClientMaster
        [Required]
        public int ClientMasterId { get; set; }

        public ClientMaster? ClientMaster { get; set; }

        // ⭐ Invoice Date
        [Required]
        public DateTime InvoiceDate { get; set; }

        // ⭐ Amounts
        [Required]
        public decimal GrossAmount { get; set; }

        [Required]
        public decimal GstAmount { get; set; }

        [Required]
        public decimal Total { get; set; }

        // 🔑 FK → StaffMaster (Created By)
        [Required]
        public int StaffMasterId { get; set; }

        public StaffMaster? StaffMaster { get; set; }

        // ⭐ Entry Timestamp
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // 🔥 Relation → InvoiceItems
        public ICollection<InvoiceItems> InvoiceItems { get; set; }
            = new HashSet<InvoiceItems>();

        // 🔥 Relation → InvoicePayments
        public ICollection<InvoicePayment> InvoicePayments { get; set; }
            = new HashSet<InvoicePayment>();


    }
}