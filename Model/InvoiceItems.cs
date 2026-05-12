using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bill_Master.Model
{
    public class InvoiceItems
    {
        [Key]
        public int Id { get; set; }

        // 🔥 FK → InvoiceMaster
        [Required]
        public int InvoiceMasterId { get; set; }

        public InvoiceMaster? InvoiceMaster { get; set; }

        // 🔥 FK → ProductMaster
        [Required]
        public int ProductMasterId { get; set; }

        public ProductMaster? ProductMaster { get; set; }

        // Optional
        public string? HSNCode { get; set; }

        // 🔥 REQUIRED
        [Required]
        public decimal Qty { get; set; }

        public string? Unit { get; set; }

        // 🔥 REQUIRED
        [Required]
        public decimal Rate { get; set; }

        // 🔥 REQUIRED
        [Required]
        public decimal TaxableValue { get; set; }

        // 🔥 REQUIRED (Total Gst)
        [Required]
        public decimal GstAmount { get; set; }

        // 🔥 REQUIRED (Final Line Total)
        [Required]
        public decimal Total { get; set; }
    }
}