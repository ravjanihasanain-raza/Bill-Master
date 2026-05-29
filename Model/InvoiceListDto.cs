public class InvoiceListDto
{
    public int Id { get; set; }

    public int ClientMasterId { get; set; }

    public string InvoiceNo { get; set; }

    public DateTime InvoiceDate { get; set; }

    public decimal Total { get; set; }

    public decimal GrossAmount { get; set; }

    public decimal GstAmount { get; set; }

    public string ClientName { get; set; }

    public string StaffName { get; set; }

    public decimal PaidAmount { get; set; }

    public decimal PendingAmount { get; set; }

    public string Status { get; set; }
}