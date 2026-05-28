using StoreManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace StoreManagementSystem.Views
{
    public partial class TransactionHistoryWindow : Window
    {
        public TransactionHistoryWindow()
        {
            InitializeComponent();
        }

        private void TransactionDetailsBtn_Click(object sender, RoutedEventArgs e)
        {
            new TransactionDetailsWindow(null).Show();
        }
    }
}