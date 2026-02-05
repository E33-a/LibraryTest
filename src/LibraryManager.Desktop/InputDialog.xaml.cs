using LibraryManager.Core.Models;
using LibraryManager.Data;
using System.Windows;

namespace LibraryManager.Desktop
{
    public partial class InputDialog : Window
    {
        private readonly LibraryDbContext _context;
        public int SelectedUserId { get; set; }
        public int SelectedBookId { get; set; }

        public InputDialog(LibraryDbContext context)
        {
            InitializeComponent();
            _context = context;
            LoadData();
        }

        private void LoadData()
        {
            cmbUsers.ItemsSource = _context.Users.Where(u => u.IsActive).ToList();
            cmbBooks.ItemsSource = _context.Books.ToList();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (cmbUsers.SelectedValue == null || cmbBooks.SelectedValue == null)
            {
                txtWarning.Text = "Seleccione usuario y libro.";
                return;
            }

            SelectedUserId = (int)cmbUsers.SelectedValue;
            SelectedBookId = (int)cmbBooks.SelectedValue;
            
            // Verificar stock
            var book = _context.Books.Find(SelectedBookId);
            if (book?.Stock == 0)
            {
                txtWarning.Text = "⚠️ ADVERTENCIA: Este libro NO tiene stock disponible.";
                // Permitimos continuar para que la validación falle después (mejor para demo)
            }

            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}