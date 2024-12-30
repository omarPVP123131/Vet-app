using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using VeterinaryManagementSystem.Views;
using VeterinaryManagementSystem.ViewModels;
using VeterinaryManagementSystem.Services;

namespace VeterinaryManagementSystem
{
    public partial class MainWindow : Window
    {
        private bool isMenuExpanded = true;
        private readonly MainViewModel _viewModel;

        public MainWindow(IAuthenticationService authService)
        {
            InitializeComponent();
            _viewModel = new MainViewModel(authService);
            DataContext = _viewModel;
            // Llamada para navegar al Dashboard después de la inicialización
            NavigateToDashboard(null, null);
        
        }

        private void NavigateToDashboard(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DashboardPage());
        }
        private void NavigateToConfig(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SettingsWindow());
        }

        private void NavigateToPatients(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PatientsPage());
        }

        private void NavigateToAppointments(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AppointmentsPage());
        }

        private void NavigateToInventory(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new InventoryPage());
        }

        private void NavigateToMedicalRecords(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MedicalRecordsPage());
        }

        private void NavigateToBilling(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new BillingPage());
        }

        private void NavigateToVaccination(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new VaccinationPage());
        }

        private void NavigateToCommunication(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CommunicationPage());
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SeeAllNotifications_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ver todas las notificaciones: Esta funcionalidad se implementará próximamente.",
                            "Notificaciones",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }
        private void ToggleMenu_Click(object sender, RoutedEventArgs e)
        {
            var menuStoryboard = (Storyboard)FindResource(isMenuExpanded ? "MenuClose" : "MenuOpen");
            menuStoryboard.Begin(SideMenu);
            isMenuExpanded = !isMenuExpanded;

            // Ocultar/Mostrar textos del menú
            var textBlocks = FindVisualChildren<TextBlock>(SideMenu);
            foreach (var textBlock in textBlocks)
            {
                textBlock.Visibility = isMenuExpanded ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        // Helper method to find all children of a specific type
        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void ShowNotifications_Click(object sender, RoutedEventArgs e)
        {
            NotificationsPopup.IsOpen = !NotificationsPopup.IsOpen;
        }

        private void MinimizeWindow_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeWindow_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }
    }
}