using System.Windows.Controls;
using StoreManagementSystem.ViewModels;

namespace StoreManagementSystem.Views
{
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();

            DataContext =
                new DashboardViewModel();
        }
    }
}