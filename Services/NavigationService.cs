using System.Windows.Controls;

namespace StoreManagementSystem.Services
{
    public static class NavigationService
    {
        public static ContentControl? MainContentControl
        {
            get;
            set;
        }

        public static void Navigate(UserControl view)
        {
            if (MainContentControl != null)
            {
                MainContentControl.Content = view;
            }
        }
    }
}