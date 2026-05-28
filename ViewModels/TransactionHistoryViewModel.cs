using StoreManagementSystem.Commands;
using StoreManagementSystem.Models;
using StoreManagementSystem.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace StoreManagementSystem.ViewModels
{
    public class TransactionHistoryViewModel : BaseViewModel
    {
        public ObservableCollection<Transaction> Transactions { get; set; }
        private ObservableCollection<Transaction> AllTransactions { get; set; }

        private Transaction? _selectedTransaction;
        public Transaction? SelectedTransaction
        {
            get => _selectedTransaction;
            set { _selectedTransaction = value; OnPropertyChanged(); }
        }

        private string _searchText = "";
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); FilterTransactions(); }
        }

        public int TotalTransactions => AllTransactions.Count;
        public decimal TotalRevenue => AllTransactions.Sum(t => t.Total);

        public ICommand ViewDetailsCommand { get; set; }

        public TransactionHistoryViewModel()
        {
            AllTransactions = new ObservableCollection<Transaction>
            {
                new Transaction
                {
                    Id = 1001,
                    Date = DateTime.Now.AddDays(-1),
                    Status = "Completed",
                    Items = new()
                    {
                        new TransactionItem { Name = "Keyboard", Quantity = 1, UnitPrice = 500 },
                        new TransactionItem { Name = "Mouse", Quantity = 2, UnitPrice = 250 }
                    }
                },
                new Transaction
                {
                    Id = 1002,
                    Date = DateTime.Now.AddDays(-2),
                    Status = "Completed",
                    Items = new()
                    {
                        new TransactionItem { Name = "Monitor", Quantity = 1, UnitPrice = 2500 }
                    }
                },
                new Transaction
                {
                    Id = 1003,
                    Date = DateTime.Now.AddDays(-3),
                    Status = "Refunded",
                    Items = new()
                    {
                        new TransactionItem { Name = "Headset", Quantity = 1, UnitPrice = 800 }
                    }
                },
                new Transaction
                {
                    Id = 1004,
                    Date = DateTime.Now.AddDays(-4),
                    Status = "Completed",
                    Items = new()
                    {
                        new TransactionItem { Name = "Keyboard", Quantity = 2, UnitPrice = 500 },
                        new TransactionItem { Name = "Mouse", Quantity = 1, UnitPrice = 250 }
                    }
                }
            };

            Transactions = new ObservableCollection<Transaction>(AllTransactions);
            ViewDetailsCommand = new RelayCommand(ViewDetails);
        }

        private void FilterTransactions()
        {
            Transactions.Clear();

            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? AllTransactions
                : new ObservableCollection<Transaction>(
                    AllTransactions.Where(t =>
                        t.Id.ToString().Contains(SearchText) ||
                        t.Status.Contains(SearchText, StringComparison.OrdinalIgnoreCase)));

            foreach (var t in filtered)
                Transactions.Add(t);

            OnPropertyChanged(nameof(TotalTransactions));
            OnPropertyChanged(nameof(TotalRevenue));
        }

        private void ViewDetails(object? parameter)
        {
            if (SelectedTransaction != null)
            {
                var window = new TransactionDetailsWindow(SelectedTransaction);
                window.Show();
            }
        }
    }
}