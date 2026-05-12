using System.ComponentModel.DataAnnotations;

namespace Bill_Master.Model
{
    public class FinancialYear
    {
        public int Id { get; set; }

        [Required]
        public string YearName { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsDelete { get; set; } = false;
        public bool IsActive { get; set; } = false;

        // NEW FIELD
        public bool IsClosed { get; set; } = false;

        // 🔥 FK RELATION → PurchaseMaster
        public ICollection<PurchaseMaster> Purchases { get; set; }
            = new HashSet<PurchaseMaster>();
    }
}