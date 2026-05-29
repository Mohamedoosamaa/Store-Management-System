using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StoreManagementSystem.Models
{
    public class Product : INotifyPropertyChanged
    {
        private string _name = string.Empty;

        private string _sku = string.Empty;

        private decimal _price;

        private int _stockQuantity;

        private string _category = string.Empty;

        public int Id { get; set; }

        public string Name
        {
            get => _name;

            set
            {
                _name = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalValue));
            }
        }

        public string SKU
        {
            get => _sku;

            set
            {
                _sku = value;

                OnPropertyChanged();
            }
        }

        public decimal Price
        {
            get => _price;

            set
            {
                _price = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalValue));
            }
        }

        public int StockQuantity
        {
            get => _stockQuantity;

            set
            {
                _stockQuantity = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(IsLowStock));
                OnPropertyChanged(nameof(TotalValue));
            }
        }

        public string Category
        {
            get => _category;

            set
            {
                _category = value;

                OnPropertyChanged();
            }
        }

        public bool IsLowStock =>
            StockQuantity <= 5;

        public decimal TotalValue =>
            Price * StockQuantity;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(
            [CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}