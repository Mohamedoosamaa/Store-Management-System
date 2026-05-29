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
    public class POSViewModel : BaseViewModel
    {
        // =========================
        // REPOSITORIES
        // =========================

        private readonly ProductRepository
            _productRepository;

        private readonly TransactionRepository
            _transactionRepository;

        // =========================
        // COLLECTIONS
        // =========================

        public ObservableCollection<Product>
            Products
        {
            get;
            set;
        }

        public ObservableCollection<Product>
            AllProducts
        {
            get;
            set;
        }

        public ObservableCollection<CartItem>
            CartItems
        {
            get;
            set;
        }

        // =========================
        // SELECTED PRODUCT
        // =========================

        private Product _selectedProduct =
            new();

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

        private string _searchText =
            string.Empty;

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
        // TOTALS
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

        public POSViewModel()
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

            foreach (var product
                in products)
            {
                Products.Add(product);

                AllProducts.Add(product);
            }
        }

        // =========================
        // FILTER PRODUCTS
        // =========================

        private void FilterProducts()
        {
            Products.Clear();

            var filteredProducts =
                string.IsNullOrWhiteSpace(
                    SearchText)
                ? AllProducts
                : new ObservableCollection<Product>(
                    AllProducts.Where(p =>
                        p.Name.Contains(
                            SearchText,
                            System.StringComparison
                            .OrdinalIgnoreCase)));

            foreach (var product
                in filteredProducts)
            {
                Products.Add(product);
            }
        }

        // =========================
        // ADD TO CART
        // =========================

        private void AddToCart(
            object? parameter)
        {
            if (SelectedProduct == null)
                return;

            if (SelectedProduct.StockQuantity
                <= 0)
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
                if (existingItem.Quantity
                    >= SelectedProduct
                    .StockQuantity)
                {
                    MessageBox.Show(
                        "No more stock available.");

                    return;
                }

                existingItem.Quantity++;
            }
            else
            {
                CartItems.Add(new CartItem
                {
                    Product =
                        SelectedProduct,

                    Quantity = 1
                });
            }

            RefreshTotals();
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

            foreach (var cartItem
                in CartItems)
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

            RefreshTotals();

            LoadProductsAsync();

            MessageBox.Show(
                "Checkout completed successfully.");
        }

        // =========================
        // REFRESH TOTALS
        // =========================

        private void RefreshTotals()
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