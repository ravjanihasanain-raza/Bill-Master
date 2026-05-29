namespace Bill_Master.Model
{
    public class ExpenseMaster
    {
        public int Id { get; set; }

        public DateTime ExpenseDate { get; set; } = DateTime.Now;

        public decimal Amount { get; set; }

        public int ExpenseCategoryId { get; set; }

        public string Description { get; set; } = string.Empty;

        public string? PaymentMode { get; set; }

        public string? PaidTo { get; set; }
        public bool IsPaid { get; set; } = false;

        public DateTime? PaidDate { get; set; }

        public string? PaidBy { get; set; }

        public string? ReferenceNo { get; set; }

        public bool IsApproved { get; set; } = true;

        public string? AttachmentURL { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}