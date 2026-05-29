using StoreManagementSystem.Commands;
using StoreManagementSystem.Services;
using StoreManagementSystem.Views;
using System.Windows;
using System.Windows.Input;

namespace StoreManagementSystem.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        // =========================
        // SERVICES
        // =========================

        private readonly AuthService _authService;

        // =========================
        // USERNAME
        // =========================

        private string _username =
            string.Empty;

        public string Username
        {
            get => _username;

            set
            {
                _username = value;

                OnPropertyChanged();
            }
        }

        // =========================
        // PASSWORD
        // =========================

        private string _password =
            string.Empty;

        public string Password
        {
            get => _password;

            set
            {
                _password = value;

                OnPropertyChanged();
            }
        }

        // =========================
        // COMMANDS
        // =========================

        public ICommand LoginCommand
        {
            get;
            set;
        }

        // =========================
        // CONSTRUCTOR
        // =========================

        public LoginViewModel()
        {
            _authService =
                new AuthService();

            LoginCommand =
                new RelayCommand(async _ =>
                    await LoginAsync());
        }

        // =========================
        // LOGIN
        // =========================

        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(
                Username))
            {
                MessageBox.Show(
                    "Please enter username.");

                return;
            }

            if (string.IsNullOrWhiteSpace(
                Password))
            {
                MessageBox.Show(
                    "Please enter password.");

                return;
            }

            var user =
                await _authService.LoginAsync(
                    Username,
                    Password);

            if (user == null)
            {
                MessageBox.Show(
                    "Invalid username or password.");

                return;
            }

            CurrentUserService.CurrentUser =
                user;

            MainWindow mainWindow =
                new MainWindow();

            mainWindow.Show();

            foreach (Window window
                in Application.Current.Windows)
            {
                if (window is LoginWindow)
                {
                    window.Close();

                    break;
                }
            }
        }
    }
}