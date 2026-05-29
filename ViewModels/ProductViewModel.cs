using StoreManagementSystem.Commands;
using StoreManagementSystem.Models;
using StoreManagementSystem.Repositories;
using StoreManagementSystem.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace StoreManagementSystem.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        // =========================
        // REPOSITORIES
        // =========================

        private readonly ProductRepository _productRepository;

        private readonly TransactionRepository _transactionRepository;

        // =========================
        // COLLECTIONS
        // =========================

        public ObservableCollection<Product> Products
        {
            get;
            set;
        }

        public ObservableCollection<Product> AllProducts
        {
            get;
            set;
        }

        public ObservableCollection<CartItem> CartItems
        {
            get;
            set;
        }

        // =========================
        // SELECTED PRODUCT
        // =========================

        private Product _selectedProduct = new();

        public Product SelectedProduct
        {
            get => _selectedProduct;

            set
            {
                _selectedProduct = value;

                OnPropertyChanged();
            }
        }

        // =========================
        // SEARCH
        // =========================

        private string _searchText = string.Empty;

        public string SearchText
        {
            get => _searchText;

            set
            {
                _searchText = value;

                OnPropertyChanged();

                FilterProducts();
            }
        }

        // =========================
        // DASHBOARD
        // =========================

        public int TotalProducts =>
            Products.Count;

        public int LowStockCount =>
            Products.Count(p => p.StockQuantity <= 5);

        public decimal InventoryValue =>
            Products.Sum(p => p.TotalValue);

        // =========================
        // POS TOTALS
        // =========================

        public decimal SubTotal =>
            CartItems.Sum(c => c.LineTotal);

        public decimal VAT =>
            SubTotal * 0.14m;

        public decimal FinalTotal =>
            SubTotal + VAT;

        // =========================
        // COMMANDS
        // =========================

        public ICommand AddCommand
        {
            get;
            set;
        }

        public ICommand UpdateCommand
        {
            get;
            set;
        }

        public ICommand DeleteCommand
        {
            get;
            set;
        }

        public ICommand AddToCartCommand
        {
            get;
            set;
        }

        public ICommand CheckoutCommand
        {
            get;
            set;
        }

        // =========================
        // CONSTRUCTOR
        // =========================

        public ProductViewModel()
        {
            _productRepository =
                new ProductRepository();

            _transactionRepository =
                new TransactionRepository();

            Products =
                new ObservableCollection<Product>();

            AllProducts =
                new ObservableCollection<Product>();

            CartItems =
                new ObservableCollection<CartItem>();

            LoadProductsAsync();

            AddCommand =
                new RelayCommand(async _ =>
                    await AddProductAsync());

            UpdateCommand =
                new RelayCommand(async _ =>
                    await UpdateProductAsync());

            DeleteCommand =
                new RelayCommand(async _ =>
                    await DeleteProductAsync());

            AddToCartCommand =
                new RelayCommand(AddToCart);

            CheckoutCommand =
                new RelayCommand(async _ =>
                    await CheckoutAsync());
        }

        // =========================
        // LOAD PRODUCTS
        // =========================

        private async void LoadProductsAsync()
        {
            Products.Clear();

            AllProducts.Clear();

            var products =
                await _productRepository
                .GetAllProductsAsync();

            foreach (var product in products)
            {
                Products.Add(product);

                AllProducts.Add(product);
            }

            RefreshDashboard();
        }

        // =========================
        // ADD PRODUCT
        // =========================

        private async Task AddProductAsync()
        {
            if (!ValidateProduct())
                return;

            await _productRepository
                .AddProductAsync(SelectedProduct);

            LoadProductsAsync();

            SelectedProduct = new Product();

            MessageBox.Show(
                "Product added successfully.");
        }

        // =========================
        // UPDATE PRODUCT
        // =========================

        private async Task UpdateProductAsync()
        {
            if (SelectedProduct == null)
                return;

            if (!ValidateProduct())
                return;

            await _productRepository
                .UpdateProductAsync(SelectedProduct);

            LoadProductsAsync();

            MessageBox.Show(
                "Product updated successfully.");
        }

        // =========================
        // DELETE PRODUCT
        // =========================

        private async Task DeleteProductAsync()
        {
            if (SelectedProduct == null)
                return;

            await _productRepository
                .DeleteProductAsync(
                    SelectedProduct.Id);

            LoadProductsAsync();

            SelectedProduct = new Product();

            MessageBox.Show(
                "Product deleted successfully.");
        }

        // =========================
        // VALIDATION
        // =========================

        private bool ValidateProduct()
        {
            if (string.IsNullOrWhiteSpace(
                SelectedProduct.Name))
            {
                MessageBox.Show(
                    "Product name is required.");

                return false;
            }

            if (string.IsNullOrWhiteSpace(
                SelectedProduct.SKU))
            {
                MessageBox.Show(
                    "SKU is required.");

                return false;
            }

            if (SelectedProduct.Price <= 0)
            {
                MessageBox.Show(
                    "Price must be greater than zero.");

                return false;
            }

            if (SelectedProduct.StockQuantity < 0)
            {
                MessageBox.Show(
                    "Invalid stock quantity.");

                return false;
            }

            return true;
        }

        // =========================
        // SEARCH
        // =========================

        private void FilterProducts()
        {
            Products.Clear();

            var filteredProducts =
                string.IsNullOrWhiteSpace(SearchText)
                ? AllProducts
                : new ObservableCollection<Product>(
                    AllProducts.Where(p =>
                        p.Name.Contains(
                            SearchText,
                            System.StringComparison
                            .OrdinalIgnoreCase)));

            foreach (var product in filteredProducts)
            {
                Products.Add(product);
            }

            RefreshDashboard();
        }

        // =========================
        // ADD TO CART
        // =========================

        private void AddToCart(object? parameter)
        {
            if (SelectedProduct == null)
                return;

            if (SelectedProduct.StockQuantity <= 0)
            {
                MessageBox.Show(
                    "Product out of stock.");

                return;
            }

            var existingItem =
                CartItems.FirstOrDefault(c =>
                    c.Product.Id ==
                    SelectedProduct.Id);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                CartItems.Add(new CartItem
                {
                    Product = SelectedProduct,
                    Quantity = 1
                });
            }

            RefreshCart();
        }

        // =========================
        // CHECKOUT
        // =========================

        private async Task CheckoutAsync()
        {
            if (CartItems.Count == 0)
            {
                MessageBox.Show(
                    "Cart is empty.");

                return;
            }

            Transaction transaction =
                new Transaction
                {
                    UserId =
                        CurrentUserService
                        .CurrentUser!.UserId,

                    TransactionDate =
                        DateTime.Now
                };

            foreach (var cartItem in CartItems)
            {
                transaction.Items.Add(
                    new TransactionItem
                    {
                        ProductId =
                            cartItem.Product.Id,

                        ProductName =
                            cartItem.Product.Name,

                        Quantity =
                            cartItem.Quantity,

                        UnitPrice =
                            cartItem.Product.Price
                    });
            }

            await _transactionRepository
                .SaveTransactionAsync(
                    transaction);

            CartItems.Clear();

            RefreshCart();

            LoadProductsAsync();

            MessageBox.Show(
                "Checkout completed successfully.");
        }

        // =========================
        // REFRESH DASHBOARD
        // =========================

        private void RefreshDashboard()
        {
            OnPropertyChanged(
                nameof(TotalProducts));

            OnPropertyChanged(
                nameof(LowStockCount));

            OnPropertyChanged(
                nameof(InventoryValue));
        }

        // =========================
        // REFRESH CART
        // =========================

        private void RefreshCart()
        {
            OnPropertyChanged(
                nameof(SubTotal));

            OnPropertyChanged(
                nameof(VAT));

            OnPropertyChanged(
                nameof(FinalTotal));
        }
    }
}