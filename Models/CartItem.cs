using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StoreManagementSystem.Models
{
    public class CartItem : INotifyPropertyChanged
    {
        private int _quantity = 1;

        public Product Product { get; set; } = new();

        public int Quantity
        {
            get => _quantity;

            set
            {
                _quantity = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(LineTotal));
            }
        }

        public decimal LineTotal =>
            Product.Price * Quantity;

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