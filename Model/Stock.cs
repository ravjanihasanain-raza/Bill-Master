namespace Bill_Master.Model
{
    using Bill_Master.Model;
    public class Stock
    {
        public int Id { get; set; }

        public int ProductMasterId { get; set; }

        public decimal Qty { get; set; }
    }
}