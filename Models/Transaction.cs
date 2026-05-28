using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreManagementSystem.Models
{
    public class TransactionItem
    {
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total => UnitPrice * Quantity;
    }

    public class Transaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<TransactionItem> Items { get; set; } = new();
        public string Status { get; set; } = "Completed";

        public int ItemCount => Items.Sum(i => i.Quantity);
        public decimal SubTotal => Items.Sum(i => i.Total);
        public decimal VAT => SubTotal * 0.14m;
        public decimal Total => SubTotal + VAT;
    }
}