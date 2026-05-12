using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;



namespace Bill_Master.Model
{
    public class StaffMaster
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string? Address { get; set; }

        public string? AadharNo { get; set; }

        public string Email { get; set; } = string.Empty;

        public string ContactNo { get; set; } = string.Empty;

        public string? AadharCopyURL { get; set; }

        public DateTime? DOJ { get; set; }

        public string? Gender { get; set; }

        public DateTime? DOB { get; set; }

        public string Status { get; set; } = "Active";
        public string? Password { get; set; }

        public string Role { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // 🔥 Invoice relations
        public ICollection<InvoiceMaster> InvoiceMasters { get; set; }
            = new HashSet<InvoiceMaster>();

        public ICollection<InvoicePayment> InvoicePayments { get; set; }
            = new HashSet<InvoicePayment>();

        // 🔥 Purchase relations
        public ICollection<PurchaseMaster> Purchases { get; set; }
            = new HashSet<PurchaseMaster>();

        public ICollection<PurchasePayment> PurchasePayments { get; set; }
            = new HashSet<PurchasePayment>();

        // 🔥 Stock relations
        public ICollection<InwardStock> InwardStocks { get; set; }
            = new HashSet<InwardStock>();

        public ICollection<Outward> Outwards { get; set; }
            = new HashSet<Outward>();

        // ⭐ OPTIONAL — Client handled by staff (recommended)
        public ICollection<ClientMaster> Clients { get; set; }
            = new HashSet<ClientMaster>();
    }
}