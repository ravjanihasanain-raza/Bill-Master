namespace Bill_Master.Model
{
    public class ExpenseListDto
    {
        public int Id { get; set; }

        public DateTime ExpenseDate { get; set; }
        public int ExpenseCategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string Description { get; set; } = string.Empty;

        public string? PaymentMode { get; set; }

        public string? PaidTo { get; set; }

        public bool IsPaid { get; set; }

        public DateTime? PaidDate { get; set; }

        public string? PaidBy { get; set; }
    }
}