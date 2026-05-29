using StoreManagementSystem.Commands;
using StoreManagementSystem.Models;
using StoreManagementSystem.Repositories;
using StoreManagementSystem.Views;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace StoreManagementSystem.ViewModels
{
    public class TransactionHistoryViewModel
        : BaseViewModel
    {
        // =========================
        // REPOSITORY
        // =========================

        private readonly TransactionRepository
            _transactionRepository;

        // =========================
        // COLLECTIONS
        // =========================

        public ObservableCollection<Transaction>
            Transactions
        {
            get;
            set;
        }

        public ObservableCollection<Transaction>
            AllTransactions
        {
            get;
            set;
        }

        // =========================
        // SELECTED TRANSACTION
        // =========================

        private Transaction?
            _selectedTransaction;

        public Transaction?
            SelectedTransaction
        {
            get => _selectedTransaction;

            set
            {
                _selectedTransaction = value;

                OnPropertyChanged();
            }
        }

        // =========================
        // SEARCH
        // =========================

        private string _searchText =
            string.Empty;

        public string SearchText
        {
            get => _searchText;

            set
            {
                _searchText = value;

                OnPropertyChanged();

                FilterTransactions();
            }
        }

        // =========================
        // DASHBOARD STATS
        // =========================

        public int TotalTransactions =>
            Transactions.Count;

        public decimal TotalRevenue =>
            Transactions.Sum(t => t.FinalTotal);

        // =========================
        // COMMANDS
        // =========================

        public ICommand ViewDetailsCommand
        {
            get;
            set;
        }

        // =========================
        // CONSTRUCTOR
        // =========================

        public TransactionHistoryViewModel()
        {
            _transactionRepository =
                new TransactionRepository();

            Transactions =
                new ObservableCollection<Transaction>();

            AllTransactions =
                new ObservableCollection<Transaction>();

            LoadTransactionsAsync();

            ViewDetailsCommand =
                new RelayCommand(ViewDetails);
        }

        // =========================
        // LOAD TRANSACTIONS
        // =========================

        private async void LoadTransactionsAsync()
        {
            Transactions.Clear();

            AllTransactions.Clear();

            var transactions =
                await _transactionRepository
                .GetAllTransactionsAsync();

            foreach (var transaction
                in transactions)
            {
                var items =
                    await _transactionRepository
                    .GetTransactionItemsAsync(
                        transaction.TransactionId);

                foreach (var item in items)
                {
                    transaction.Items.Add(item);
                }

                Transactions.Add(transaction);

                AllTransactions.Add(transaction);
            }

            RefreshDashboard();
        }

        // =========================
        // SEARCH FILTER
        // =========================

        private void FilterTransactions()
        {
            Transactions.Clear();

            var filteredTransactions =
                string.IsNullOrWhiteSpace(
                    SearchText)
                ? AllTransactions
                : new ObservableCollection<Transaction>(
                    AllTransactions.Where(t =>
                        t.TransactionId
                        .ToString()
                        .Contains(SearchText)
                        ||
                        t.Status.Contains(
                            SearchText,
                            System.StringComparison
                            .OrdinalIgnoreCase)));

            foreach (var transaction
                in filteredTransactions)
            {
                Transactions.Add(transaction);
            }

            RefreshDashboard();
        }

        // =========================
        // VIEW DETAILS
        // =========================

        private void ViewDetails(
            object? parameter)
        {
            if (SelectedTransaction == null)
            {
                MessageBox.Show(
                    "Please select a transaction.");

                return;
            }

            TransactionDetailsViewModel
                detailsViewModel =
                    new TransactionDetailsViewModel(
                        SelectedTransaction);

            TransactionDetailsWindow
                detailsWindow =
                    new TransactionDetailsWindow();

            detailsWindow.DataContext =
                detailsViewModel;

            detailsWindow.ShowDialog();
        }

        // =========================
        // REFRESH DASHBOARD
        // =========================

        private void RefreshDashboard()
        {
            OnPropertyChanged(
                nameof(TotalTransactions));

            OnPropertyChanged(
                nameof(TotalRevenue));
        }
    }
}