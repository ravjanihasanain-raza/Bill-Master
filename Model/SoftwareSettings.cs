using System.ComponentModel.DataAnnotations;

namespace Bill_Master.Model
{
    public class SoftwareSettings
    {
        public int Id { get; set; }

        [Required]
        public string BusinessName { get; set; } = string.Empty;

        public string AddressLine1 { get; set; } = string.Empty;

        public string? AddressLine2 { get; set; }

        public string? AddressLine3 { get; set; }

        [Required]
        public string ContactNo { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string GstIN { get; set; } = string.Empty;

        [Required]
        public string PAN { get; set; } = string.Empty;

        [Required]
        public string BankName { get; set; } = string.Empty;

        [Required]
        public string AccountHolderName { get; set; } = string.Empty;

        [Required]
        public string AccountNumber { get; set; } = string.Empty;

        [Required]
        public string BankIFSC { get; set; } = string.Empty;

        [Required]
        public string SignatureURL { get; set; } = string.Empty;

        public string? LogoURL { get; set; }
    }
}

