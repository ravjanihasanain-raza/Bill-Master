namespace Bill_Master.Model
{
    public class InvoiceDto
    {
        public int InvoiceId { get; set; }
        public int StaffId { get; set; }
        public string InvoiceNo { get; set; } = "";

        public List<InvoiceItemDto> Items { get; set; } = new();
    }

    public class InvoiceItemDto
    {
        public int ProductId { get; set; }
        public decimal Qty { get; set; }
    }
}