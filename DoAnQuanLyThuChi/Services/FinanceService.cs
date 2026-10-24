using System;
using System.Collections.Generic;
using System.Linq;
using DoAnQuanLyThuChi.Models;

namespace DoAnQuanLyThuChi.Services
{
    public class CategoryService
    {
        private static List<Category> _categories = new List<Category>
        {
            new Category { CategoryId = 1, UserId = 1, Name = "Ăn uống", Type = "Expense" },
            new Category { CategoryId = 2, UserId = 1, Name = "Lương", Type = "Income" },
            new Category { CategoryId = 3, UserId = 1, Name = "Mua sắm", Type = "Expense" }
        };

        public List<Category> GetAll() => _categories;

        public void Add(Category category)
        {
            category.CategoryId = _categories.Count > 0 ? _categories.Max(c => c.CategoryId) + 1 : 1;
            _categories.Add(category);
        }

        public void Update(Category category)
        {
            var item = _categories.FirstOrDefault(c => c.CategoryId == category.CategoryId);
            if (item != null)
            {
                item.Name = category.Name;
                item.Type = category.Type;
            }
        }

        public void Delete(int categoryId)
        {
            var item = _categories.FirstOrDefault(c => c.CategoryId == categoryId);
            if (item != null) _categories.Remove(item);
        }
    }

    public class BudgetService
    {
        private static List<Budget> _budgets = new List<Budget>();
        private static List<GiaoDich> _transactions = new List<GiaoDich>();

        public List<Budget> GetByMonth(int month, int year)
        {
            return _budgets.Where(b => b.Month == month && b.Year == year).ToList();
        }

        public decimal GetSpentAmount(int categoryId, int month, int year)
        {
            return _transactions
                .Where(t => t.CategoryId == categoryId
                         && t.TransactionDate.Month == month
                         && t.TransactionDate.Year == year)
                .Sum(t => t.Amount);
        }

        public void Save(Budget budget)
        {
            var existing = _budgets.FirstOrDefault(b => b.CategoryId == budget.CategoryId && b.Month == budget.Month && b.Year == budget.Year);
            if (existing != null)
            {
                existing.AmountLimit = budget.AmountLimit;
            }
            else
            {
                budget.BudgetId = _budgets.Count > 0 ? _budgets.Max(b => b.BudgetId) + 1 : 1;
                _budgets.Add(budget);
            }
        }
    }
}