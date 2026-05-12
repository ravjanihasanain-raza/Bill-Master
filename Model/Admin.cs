using System;
using System.ComponentModel.DataAnnotations;

namespace Bill_Master.Model
{
    public class Admin
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string ContactNo { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

      
        public string Password { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public String Status { get; set; } = "Active";

        [Required]
        public string Role { get; set; } = string.Empty;
    }
}
