namespace DoAnQuanLyThuChi.Models
{
    public class Budget
    {
        public int BudgetId { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public decimal AmountLimit { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}