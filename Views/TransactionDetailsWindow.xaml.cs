using StoreManagementSystem.Models;
using System.Windows;

namespace StoreManagementSystem.Views
{
    public partial class TransactionDetailsWindow : Window
    {
        public TransactionDetailsWindow(Transaction? transaction)
        {
            InitializeComponent();
            if (transaction != null)
                DataContext = transaction;
        }

        private void TransactionHistoryBtn_Click(object sender, RoutedEventArgs e)
        {
            new TransactionHistoryWindow().Show();
        }
    }
}