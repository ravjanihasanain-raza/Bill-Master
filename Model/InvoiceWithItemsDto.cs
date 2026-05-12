using Bill_Master.Model;

public class InvoiceWithItemsDto
{
    public InvoiceMaster Invoice { get; set; }
    public List<InvoiceItems> Items { get; set; }
}