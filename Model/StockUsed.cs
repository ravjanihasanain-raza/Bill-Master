using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Bill_Master.Model
{
    public class StockUsed
    {
        [Key]
        public int Id { get; set; }

        // FK → InwardStock
        [Required]
        public int InwardStockId { get; set; }


        public InwardStock? InwardStock { get; set; }

        // Quantity Used
        [Required]
        public decimal Qty { get; set; }

        // Date of stock usage
        [Required]
        public DateTime OutwardDate { get; set; }

        // FK → OutwardMaster
        
        public int? OutwardMasterId { get; set; }
        public virtual Outward? OutwardMaster { get; set; }
        public int InvoiceMasterId { get; set; }



    }
}
