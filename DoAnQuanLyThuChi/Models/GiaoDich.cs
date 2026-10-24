using System;

namespace DoAnQuanLyThuChi.Models
{
    public class GiaoDich
    {
        public int TransactionId { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }       // Tương đương DanhMucId
        public decimal Amount { get; set; }         // Số tiền
        public string Type { get; set; } = "Expense";
        public DateTime TransactionDate { get; set; } = DateTime.Now; // Tương đương NgayTao
        public string Note { get; set; } = string.Empty;

        // Alias để hỗ trợ cả code cũ nếu có
        public int DanhMucId => CategoryId;
        public DateTime NgayTao => TransactionDate;
        public decimal SoTien => Amount;
    }
}
