using System.Windows;
namespace VeterinaryManagementSystem.Views.Inventory
{
    public partial class EditCategoryDialog : Window
    {
        public string CategoryName { get; private set; }

        public EditCategoryDialog(string currentName)
        {
            InitializeComponent();
            CategoryNameTextBox.Text = currentName;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            CategoryName = CategoryNameTextBox.Text;
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}