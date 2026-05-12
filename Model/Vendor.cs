using Bill_Master.Model;

public class Vendor
{
    public int Id { get; set; }

    public string BusinessName { get; set; } = string.Empty;

    public string? ContactPerson { get; set; }

    public string GstIN { get; set; } = string.Empty;

    public string PAN { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string StateCode { get; set; } = string.Empty;

    public string BankName { get; set; } = string.Empty;

    public string AccountHolder { get; set; } = string.Empty;

    public string IFSC { get; set; } = string.Empty;

    public string AccountNumber { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string ContactNo { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // 🔥 FK RELATION → PurchaseMaster
    public ICollection<PurchaseMaster> Purchases { get; set; }
        = new HashSet<PurchaseMaster>();
}