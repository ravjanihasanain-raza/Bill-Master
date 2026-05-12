using Bill_Master.Model;

public class ClientMaster
{
    public ICollection<InvoiceMaster> InvoiceMasters { get; set; }
        = new HashSet<InvoiceMaster>();
    public int Id { get; set; }

    // ⭐ REQUIRED
    public string BusinessName { get; set; } = string.Empty;

    // ⭐ REQUIRED
    public string Address { get; set; } = string.Empty;

    // ⭐ REQUIRED
    public string Email { get; set; } = string.Empty;

    // ⭐ REQUIRED
    public string ContactNo { get; set; } = string.Empty;

    // ⭐ REQUIRED
    public string GstIN { get; set; } = string.Empty;

    // ⭐ REQUIRED
    public string StateCode { get; set; } = string.Empty;

    // ⭐ REQUIRED
    public string State { get; set; } = string.Empty;

    public string? ContactPerson { get; set; }

    // 🔑 FK (Required)
    public int StaffMasterId { get; set; }

    //public StaffMaster StaffMaster { get; set; }

    // ⭐ REQUIRED
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    // 🔥 Relation → InvoiceMaster
    
}
