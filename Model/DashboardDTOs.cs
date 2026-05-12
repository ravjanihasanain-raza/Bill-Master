namespace Bill_Master.Model
{
    // ════════════════════════════════════════════════════
    //  DASHBOARD FILTER REQUEST
    // ════════════════════════════════════════════════════

    public class DashboardFilter
    {
        // FilterType: "today" | "month" | "year" | "custom"
        public string FilterType { get; set; } = "year";

        // For year + month dropdown
        public int? Year { get; set; }
        public int? Month { get; set; }   // 1–12, null = all months

        // For custom date range (optional premium)
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    // ════════════════════════════════════════════════════
    //  DASHBOARD RESULT
    // ════════════════════════════════════════════════════

    public class DashboardResult
    {
        // ── KPI Cards ──────────────────────────────────
        public decimal TotalRevenue { get; set; }
        public int TotalInvoices { get; set; }
        public decimal TotalPurchase { get; set; }
        public decimal TotalOutstanding { get; set; }   // invoiced − collected
        public decimal TotalCollected { get; set; }   // sum of payments
        public int TotalClients { get; set; }
        public int TotalProducts { get; set; }

        // ── Bar Charts ─────────────────────────────────
        public List<MonthlyPoint> MonthlyRevenue { get; set; } = new();
        public List<MonthlyPoint> MonthlyPurchase { get; set; } = new();

        // ── Pie / Donut Charts ─────────────────────────
        public List<ChartSlice> SalesByCategory { get; set; } = new();
        public List<ChartSlice> InvoicePaymentStatus { get; set; } = new();
        public List<ChartSlice> TopClientsByRevenue { get; set; } = new();

        // ── Recent Invoices Table ──────────────────────
        public List<RecentInvoiceDto> RecentInvoices { get; set; } = new();

        // ── Filter Echo (so frontend knows what was applied) ──
        public string AppliedFilter { get; set; } = string.Empty;
        public DateTime FilterFrom { get; set; }
        public DateTime FilterTo { get; set; }
    }

    // ════════════════════════════════════════════════════
    //  SUPPORTING DTOs
    // ════════════════════════════════════════════════════

    public class MonthlyPoint
    {
        public string Month { get; set; } = string.Empty;   // "Jan 2025"
        public int Year { get; set; }
        public int MonthNo { get; set; }
        public decimal Amount { get; set; }
    }

    public class ChartSlice
    {
        public string Label { get; set; } = string.Empty;
        public decimal Value { get; set; }
    }

    public class RecentInvoiceDto
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public decimal Total { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal Outstanding { get; set; }
        public string Status { get; set; } = string.Empty;  // Paid/Partial/Unpaid
    }
}
