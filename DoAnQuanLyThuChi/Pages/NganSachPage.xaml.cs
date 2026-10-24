using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DoAnQuanLyThuChi.Models;
using DoAnQuanLyThuChi.Services;

namespace DoAnQuanLyThuChi.Pages
{
    public class BudgetViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public decimal AmountLimit { get; set; }
        public decimal SpentAmount { get; set; }
        public decimal RemainingAmount => AmountLimit - SpentAmount;
    }

    public partial class NganSachPage : Page
    {
        private CategoryService _categoryService = new CategoryService();
        private BudgetService _budgetService = new BudgetService();
        private bool _isInitialized = false; // Cờ kiểm tra để tránh gọi LoadData 2 lần lúc mở trang

        public NganSachPage()
        {
            InitializeComponent();
            InitFilter();
            _isInitialized = true;
            LoadData();
        }

        private void InitFilter()
        {
            // Nạp tháng từ 1 đến 12
            cboMonth.Items.Clear();
            for (int i = 1; i <= 12; i++)
            {
                cboMonth.Items.Add(i);
            }
            cboMonth.SelectedItem = DateTime.Now.Month;

            // Nạp năm (Năm trước, Năm hiện tại, Năm sau)
            cboYear.Items.Clear();
            int currentYear = DateTime.Now.Year;

            // ✅ SỬA LẠI THÀNH DÒNG NÀY:
            for (int i = currentYear - 1; i <= currentYear + 1; i++)
            {
                cboYear.Items.Add(i);
            }
            cboYear.SelectedItem = currentYear;
        }

        private void LoadData()
        {
            if (!_isInitialized) return;
            if (cboMonth.SelectedItem == null || cboYear.SelectedItem == null) return;

            int month = (int)cboMonth.SelectedItem;
            int year = (int)cboYear.SelectedItem;

            // Lấy danh sách danh mục chi tiêu
            var categories = _categoryService.GetAll().Where(c => c.Type == "Expense").ToList();
            var budgets = _budgetService.GetByMonth(month, year);

            var list = new List<BudgetViewModel>();
            foreach (var cat in categories)
            {
                var b = budgets.FirstOrDefault(x => x.CategoryId == cat.CategoryId);
                decimal limit = b?.AmountLimit ?? 0;
                decimal spent = _budgetService.GetSpentAmount(cat.CategoryId, month, year);

                list.Add(new BudgetViewModel
                {
                    CategoryId = cat.CategoryId,
                    CategoryName = cat.Name,
                    AmountLimit = limit,
                    SpentAmount = spent
                });
            }

            dgBudget.ItemsSource = list;
        }

        private void Filter_Changed(object sender, SelectionChangedEventArgs e)
        {
            LoadData();
        }

        private void BtnSetLimit_Click(object sender, RoutedEventArgs e)
        {
            if (dgBudget.SelectedItem is BudgetViewModel selected)
            {
                int month = (int)cboMonth.SelectedItem;
                int year = (int)cboYear.SelectedItem;

                string input = Microsoft.VisualBasic.Interaction.InputBox(
                    $"Nhập hạn mức chi tiêu cho [{selected.CategoryName}]:",
                    "Thiết lập hạn mức ngân sách",
                    selected.AmountLimit.ToString("F0"));

                if (decimal.TryParse(input, out decimal limit) && limit >= 0)
                {
                    _budgetService.Save(new Budget
                    {
                        CategoryId = selected.CategoryId,
                        Month = month,
                        Year = year,
                        AmountLimit = limit
                    });

                    // Reload lại bảng dữ liệu ngay sau khi lưu thành công
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn 1 danh mục trong bảng để thiết lập hạn mức!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}