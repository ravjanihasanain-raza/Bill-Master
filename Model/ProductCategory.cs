namespace Bill_Master.Model
{
    public class ProductCategory
    {
        public ProductCategory()
        {
            products = new HashSet<ProductMaster>();

        }
        public int Id { get; set; }

        // ⭐ REQUIRED + UNIQUE (check repository me)
        public string CategoryName { get; set; } = string.Empty;

        public string? Description { get; set; }

        // ⭐ REQUIRED
        public bool IsDelete { get; set; } = false;

        // ⭐ REQUIRED
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<ProductMaster> products { get; set; }
    }
}
