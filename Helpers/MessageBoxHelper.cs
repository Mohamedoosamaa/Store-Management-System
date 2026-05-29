using System.Windows;

namespace StoreManagementSystem.Helpers
{
    public static class MessageBoxHelper
    {
        public static void ShowInfo(string message)
        {
            MessageBox.Show(
                message,
                "Information",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        public static void ShowError(string message)
        {
            MessageBox.Show(
                message,
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        public static bool ShowConfirmation(string message)
        {
            return MessageBox.Show(
                message,
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question)
                == MessageBoxResult.Yes;
        }
    }
}