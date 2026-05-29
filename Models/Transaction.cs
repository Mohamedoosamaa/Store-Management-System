using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace StoreManagementSystem.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        public int UserId { get; set; }

        public DateTime TransactionDate { get; set; }

        public ObservableCollection<TransactionItem> Items
        {
            get;
            set;
        } = new();

        public string Status { get; set; } = "Completed";

        public decimal SubTotal =>
            Items.Sum(i => i.LineTotal);

        public decimal VAT =>
            SubTotal * 0.14m;

        public decimal FinalTotal =>
            SubTotal + VAT;

        public int ItemCount =>
            Items.Sum(i => i.Quantity);
    }
}