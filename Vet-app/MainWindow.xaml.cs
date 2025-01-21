using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using VeterinaryManagementSystem.Services;
using VeterinaryManagementSystem.Views;
using System.Windows.Navigation;
using System.Collections.Generic;
using VeterinaryManagementSystem.ViewModels;
using VeterinaryManagementSystem.Views.Inventory;
using VeterinaryManagementSystem.Helpers;
using VeterinaryManagementSystem.Config;
using VeterinaryManagementSystem.Views.Proveedores;

namespace VeterinaryManagementSystem
{
    public partial class MainWindow : Window
    {
        private bool isMenuExpanded = true;
        private readonly MainViewModel _viewModel;
        private DatabaseHelper _dbHelper;


        public MainWindow(IAuthenticationService authService, INavigationService navigationService)
        {
            InitializeComponent();
            _viewModel = new MainViewModel(authService, navigationService);
            DataContext = _viewModel;
            _dbHelper = new DatabaseHelper(DatabaseConfig.ConnectionString);

            // Llamada para navegar al Dashboard después de la inicialización
            NavigateToDashboard(null, null);
        }

        private void NavigateToDashboard(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DashboardPage());
        }

        private void NavigateToPointOfSale(object sender, RoutedEventArgs e)
        {
            // Navega a la página de Punto de Venta
            MainFrame.Navigate(new PointOfSalePage());
        }

      
        private void NavigateToConfig(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SettingsWindow());
        }

        private void NavigateToInventory(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new InventoryPage(_dbHelper));
        }

        private void NavigateToProviders(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SupplierView(_dbHelper));
        }


        private void NavigateToClients(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SupplierView(_dbHelper));
        }


        private void NavigateToCommunication(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CommunicationPage());
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Método para actualizar el botón activo
        //private void UpdateActiveButton(Button clickedButton)
        //{
        //    foreach (var button in FindVisualChildren<Button>(this))
        //    {
        //        if (button.Style == FindResource("ModernMenuButton"))
        //        {
        //            button.Background = Brushes.Transparent;
        //            button.Foreground = Brushes.Black;
        //        }
        //    }

        //    if (clickedButton != null)
        //    {
        //        clickedButton.Background = new SolidColorBrush(Color.FromRgb(74, 144, 226));
        //        clickedButton.Foreground = Brushes.White;
        //    }
        //}

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield break;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                if (child != null && child is T)
                    yield return (T)child;

                foreach (T childOfChild in FindVisualChildren<T>(child))
                    yield return childOfChild;
            }
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

            foreach (var textBlock in FindVisualChildren<TextBlock>(SideMenu))
            {
                textBlock.Visibility = isMenuExpanded ? Visibility.Visible : Visibility.Collapsed;
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
            WindowState = (WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }
    }
}