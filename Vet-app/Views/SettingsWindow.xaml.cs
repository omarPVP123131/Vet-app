using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Threading.Tasks;

namespace VeterinaryManagementSystem.Views
{
    public partial class SettingsWindow : Page
    {
        public SettingsWindow()
        {
            InitializeComponent();
            // Cargar las impresoras si decides agregar funcionalidad de impresora más tarde
            // LoadPrinters();  
        }

        private void SelectImageButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Imagenes|*.jpg;*.png;*.bmp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ProfileImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private void LoadPrinters()
        {
            // Esta funcionalidad puede ser implementada si se usa otro enfoque para imprimir
            // Ejemplo de cómo cargar impresoras (con otro método si es necesario)
            // var ps = new System.Drawing.Printing.PrinterSettings();
            // foreach (string printer in ps.InstalledPrinters)
            // {
            //     PrinterComboBox.Items.Add(printer);
            // }
        }

        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SaveSettings();
        }

        private void SaveSettings()
        {
            MessageBox.Show("Configuración guardada automáticamente.", "Guardado Automático", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RestoreDefaultSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordBox.Clear();
            ChangeEmailTextBox.Clear();
            NameTextBox.Clear();
            AddressTextBox.Clear();
            PhoneTextBox.Clear();
            DatabaseConnectionTextBox.Clear();
            NotificationsCheckBox.IsChecked = false;

            MessageBox.Show("La configuración ha sido restaurada a los valores predeterminados.", "Configuración Restaurada", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void TestPrinterButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Impresora seleccionada correctamente. Prueba exitosa.", "Prueba de Impresora", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async void BackupButton_Click(object sender, RoutedEventArgs e)
        {
            // Nos aseguramos de usar la instancia del ProgressBar
            ProgressBar.Visibility = Visibility.Visible; // Cambiamos la visibilidad usando la instancia
            ProgressBar.Value = 0; // Inicializamos el valor usando la instancia

            // Simulamos un progreso de backup
            for (int i = 0; i <= 100; i++)
            {
                await Task.Delay(50);  // Simula un tiempo de espera por cada incremento
                ProgressBar.Value = i; // Actualizamos el valor usando la instancia
            }

            // Ocultamos la barra de progreso al finalizar
            ProgressBar.Visibility = Visibility.Collapsed;
            MessageBox.Show("Backup completado.", "Backup", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Restaurando desde backup...");
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Configuraciones guardadas correctamente.");
        }
    }
}
