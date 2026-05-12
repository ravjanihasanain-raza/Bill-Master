using Bill_Master.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bill_Master.Model
{
    public class Outward
    {
        [Key]
        public int Id { get; set; }

        // 🔹 FK — StaffMaster Table
        [Required]
        public int StaffMasterId { get; set; }

        public StaffMaster? StaffMaster { get; set; }

        // 🔹 Optional Remark
        [MaxLength(500)]
        public string? Remark { get; set; }

        // 🔹 Required Outward Date
        [Required]
        public DateTime OutwardDate { get; set; }

        // 🔹 Created At (Auto Set)
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        // 🔥 Relation → StockUsed
        public ICollection<StockUsed> StockUseds { get; set; }
            = new HashSet<StockUsed>();
    }
}