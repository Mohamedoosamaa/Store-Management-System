namespace StoreManagementSystem.Helpers
{
    public static class ValidationHelper
    {
        public static bool IsValidPrice(decimal price)
        {
            return price > 0;
        }

        public static bool IsValidStock(int stock)
        {
            return stock >= 0;
        }

        public static bool IsValidText(string? text)
        {
            return !string.IsNullOrWhiteSpace(text);
        }
    }
}