using System.Windows;
using System.Windows.Controls;
using DoAnQuanLyThuChi.Models;
using DoAnQuanLyThuChi.Services;

namespace DoAnQuanLyThuChi.Pages
{
    public partial class CategoryPage : Page
    {
        private CategoryService _categoryService = new CategoryService();
        private Category? _selectedCategory;

        public CategoryPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            dgCategory.ItemsSource = null;
            dgCategory.ItemsSource = _categoryService.GetAll();
            txtName.Text = string.Empty;
            _selectedCategory = null;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text)) return;

            var category = new Category
            {
                Name = txtName.Text.Trim(),
                Type = (cboType.SelectedItem as ComboBoxItem)?.Tag.ToString() ?? "Expense"
            };

            _categoryService.Add(category);
            LoadData();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedCategory == null) return;

            _selectedCategory.Name = txtName.Text.Trim();
            _selectedCategory.Type = (cboType.SelectedItem as ComboBoxItem)?.Tag.ToString() ?? "Expense";

            _categoryService.Update(_selectedCategory);
            LoadData();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedCategory == null) return;
            _categoryService.Delete(_selectedCategory.CategoryId);
            LoadData();
        }

        private void DgCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCategory.SelectedItem is Category selected)
            {
                _selectedCategory = selected;
                txtName.Text = selected.Name;
            }
        }
    }
}