using LiveCharts;
using LiveCharts.Wpf;
using StoreManagementSystem.Repositories;
using System.Windows;
using StoreManagementSystem.Services;

namespace StoreManagementSystem.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        // =========================
        // REPOSITORIES
        // =========================

        private readonly ProductRepository
            _productRepository;

        private readonly TransactionRepository
            _transactionRepository;

        // =========================
        // USER INFO
        // =========================

        public bool IsCashier =>
            CurrentUserService.CurrentUser?.Role
            == "Cashier";

        public string WelcomeMessage =>
            CurrentUserService.CurrentUser != null
                ? $"Welcome Back, {CurrentUserService.CurrentUser.FullName}"
                : "Welcome";

        public Visibility RevenueVisibility =>
            IsCashier
                ? Visibility.Collapsed
                : Visibility.Visible;

        public Visibility AnalyticsVisibility =>
            IsCashier
                ? Visibility.Collapsed
                : Visibility.Visible;


        // =========================
        // KPI
        // =========================

        private int _productsCount;

        public int ProductsCount
        {
            get => _productsCount;

            set
            {
                _productsCount = value;

                OnPropertyChanged();
            }
        }

        private int _lowStockCount;

        public int LowStockCount
        {
            get => _lowStockCount;

            set
            {
                _lowStockCount = value;

                OnPropertyChanged();
            }
        }

        private decimal _todaySales;

        public decimal TodaySales
        {
            get => _todaySales;

            set
            {
                _todaySales = value;

                OnPropertyChanged();
            }
        }

        private decimal _monthlyRevenue;

        public decimal MonthlyRevenue
        {
            get => _monthlyRevenue;

            set
            {
                _monthlyRevenue = value;

                OnPropertyChanged();
            }
        }

        public string GrowthRate =>
            "Dynamic";

        // =========================
        // CHARTS
        // =========================

        public SeriesCollection SalesSeries
        {
            get;
            set;
        }

        public string[] SalesLabels
        {
            get;
            set;
        }

        public SeriesCollection TopProductsSeries
        {
            get;
            set;
        }

        public string[] TopProductsLabels
        {
            get;
            set;
        }

        public SeriesCollection InventorySeries
        {
            get;
            set;
        }

        // =========================
        // CONSTRUCTOR
        // =========================

        public DashboardViewModel()
        {
            _productRepository =
                new ProductRepository();

            _transactionRepository =
                new TransactionRepository();

            LoadDashboardData();

            LoadCharts();

            LoadWeeklySalesChartAsync();

            LoadTopProductsChartAsync();
        }
        // =========================
        // LOAD DASHBOARD DATA
        // =========================

        private async void LoadDashboardData()
        {
            ProductsCount =
                await _productRepository
                .GetProductsCountAsync();

            LowStockCount =
                await _productRepository
                .GetLowStockCountAsync();

            TodaySales =
                await _transactionRepository
                .GetTodaySalesAsync();

            MonthlyRevenue =
                await _transactionRepository
                .GetMonthlyRevenueAsync();
        }

        // =========================
        // TOP PRODUCTS CHART
        // =========================

        private async void LoadTopProductsChartAsync()
        {
            var products =
                await _transactionRepository
                .GetTopSellingProductsAsync();

            TopProductsLabels =
                products.Keys.ToArray();

            TopProductsSeries =
                new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Sold",

                        Values =
                            new ChartValues<int>(
                                products.Values)
                    }
                };

            OnPropertyChanged(
                nameof(TopProductsSeries));

            OnPropertyChanged(
                nameof(TopProductsLabels));
        }

        // =========================
        // WEEKLY SALES CHART
        // =========================

        private async void LoadWeeklySalesChartAsync()
        {
            var sales =
                await _transactionRepository
                .GetWeeklySalesAsync();

            SalesSeries =
                new SeriesCollection
                {
                    new LineSeries
                    {
                        Title = "Sales",

                        Values =
                            new ChartValues<double>(
                                sales)
                    }
                };

            SalesLabels =
                new[]
                {
                    "6 Days",
                    "5 Days",
                    "4 Days",
                    "3 Days",
                    "2 Days",
                    "Yesterday",
                    "Today"
                };

            OnPropertyChanged(
                nameof(SalesSeries));

            OnPropertyChanged(
                nameof(SalesLabels));
        }

        // =========================
        // LOAD CHARTS
        // =========================

        // =========================
        // LOAD CHARTS
        // =========================

        private async void LoadCharts()
        {
            int inStock =
                await _productRepository
                .GetInStockCountAsync();

            int lowStock =
                await _productRepository
                .GetLowStockCountAsync();

            int outOfStock =
                await _productRepository
                .GetOutOfStockCountAsync();

            InventorySeries =
                new SeriesCollection
                {
            new PieSeries
            {
                Title = "In Stock",

                Values =
                    new ChartValues<int>
                    {
                        inStock
                    },

                DataLabels = true
            },

            new PieSeries
            {
                Title = "Low Stock",

                Values =
                    new ChartValues<int>
                    {
                        lowStock
                    },

                DataLabels = true
            },

            new PieSeries
            {
                Title = "Out Of Stock",

                Values =
                    new ChartValues<int>
                    {
                        outOfStock
                    },

                DataLabels = true
            }
                };

            OnPropertyChanged(
                nameof(InventorySeries));
        }
    }
}