public class InvoiceListDto
{
    public int Id { get; set; }
    public string InvoiceNo { get; set; }
    public DateTime InvoiceDate { get; set; }
    public decimal Total { get; set; }

    public string ClientName { get; set; }
    public string StaffName { get; set; }

    public decimal PaidAmount { get; set; }
    public decimal PendingAmount { get; set; }
    public string Status { get; set; }
}