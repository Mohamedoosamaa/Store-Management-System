using StoreManagementSystem.Commands;
using StoreManagementSystem.Services;
using StoreManagementSystem.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StoreManagementSystem.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        // =========================
        // CURRENT VIEW
        // =========================

        private UserControl? _currentView;

        public UserControl? CurrentView
        {
            get => _currentView;

            set
            {
                _currentView = value;

                OnPropertyChanged();
            }
        }

        // =========================
        // ROLE-BASED VISIBILITY
        // =========================

        public bool IsCashier =>
            CurrentUserService.CurrentUser?.Role
            == "Cashier";

        public Visibility InventoryVisibility =>
            IsCashier
            ? Visibility.Collapsed
            : Visibility.Visible;

        public Visibility ProductsVisibility =>
            IsCashier
            ? Visibility.Collapsed
            : Visibility.Visible;

        // =========================
        // COMMANDS
        // =========================

        public ICommand OpenDashboardCommand
        {
            get;
        }

        public ICommand OpenInventoryCommand
        {
            get;
        }

        public ICommand OpenProductsCommand
        {
            get;
        }

        public ICommand OpenPOSCommand
        {
            get;
        }

        public ICommand OpenTransactionsCommand
        {
            get;
        }

        public ICommand LogoutCommand
        {
            get;
        }

        // =========================
        // CONSTRUCTOR
        // =========================

        public MainViewModel()
        {
            OpenDashboardCommand =
                new RelayCommand(OpenDashboard);

            OpenInventoryCommand =
                new RelayCommand(OpenInventory);

            OpenProductsCommand =
                new RelayCommand(OpenProducts);

            OpenPOSCommand =
                new RelayCommand(OpenPOS);

            OpenTransactionsCommand =
                new RelayCommand(OpenTransactions);

            LogoutCommand =
                new RelayCommand(Logout);

            CurrentView =
                new DashboardView();
        }

        // =========================
        // DASHBOARD
        // =========================

        private void OpenDashboard(
            object? parameter)
        {
            CurrentView =
                new DashboardView();
        }

        // =========================
        // INVENTORY
        // =========================

        private void OpenInventory(
            object? parameter)
        {
            CurrentView =
                new InventoryView();
        }

        // =========================
        // PRODUCTS
        // =========================

        private void OpenProducts(
            object? parameter)
        {
            CurrentView =
                new ProductsView();
        }

        // =========================
        // POS
        // =========================

        private void OpenPOS(
            object? parameter)
        {
            CurrentView =
                new POSView();
        }

        // =========================
        // TRANSACTIONS
        // =========================

        private void OpenTransactions(
            object? parameter)
        {
            TransactionHistoryWindow
                transactionWindow =
                    new TransactionHistoryWindow();

            transactionWindow.ShowDialog();
        }

        // =========================
        // LOGOUT
        // =========================

        private void Logout(
            object? parameter)
        {
            CurrentUserService.CurrentUser =
                null;

            LoginWindow loginWindow =
                new LoginWindow();

            loginWindow.Show();

            foreach (Window window
                in Application.Current.Windows)
            {
                if (window is MainWindow)
                {
                    window.Close();

                    break;
                }
            }
        }
    }
}