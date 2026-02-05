using LibraryManager.Core.Models;
using LibraryManager.Data;
using System.Linq;
using System.Windows;

namespace LibraryManager.Desktop
{
    public partial class MainWindow : Window
    {
        private readonly LibraryDbContext _context;
        private User? _currentUser;

        public MainWindow()
        {
            InitializeComponent();
            _context = new LibraryDbContext();
            _context.Database.EnsureCreated();
        }

        // ============== LOGIN ==============
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                txtMessage.Text = "Por favor ingrese usuario y contraseña";
                return;
            }

            var user = _context.Users.FirstOrDefault(u => 
                u.Username == username && 
                u.Password == password &&
                u.IsActive);

            if (user != null)
            {
                _currentUser = user;
                txtUserInfo.Text = $"Conectado: {user.FullName}\n({user.Role})";
                ShowMainView();
            }
            else
            {
                var inactiveUser = _context.Users.FirstOrDefault(u => 
                    u.Username == username && !u.IsActive);
                
                txtMessage.Text = inactiveUser != null 
                    ? "Usuario inactivo" 
                    : "Usuario o contraseña incorrectos";
                
                txtPassword.Clear();
            }
        }

        private void ShowMainView()
        {
            LoginView.Visibility = Visibility.Collapsed;
            MainView.Visibility = Visibility.Visible;
        }

        private void ShowLoginView()
        {
            MainView.Visibility = Visibility.Collapsed;
            LoginView.Visibility = Visibility.Visible;
            txtUsername.Clear();
            txtPassword.Clear();
            txtMessage.Text = "";
            _currentUser = null;
        }

        // ============== MENÚ ==============
        private void btnBooks_Click(object sender, RoutedEventArgs e)
        {
            var books = _context.Books.ToList();
            string content = "📚 LISTA DE LIBROS\n\n";
            foreach (var book in books)
            {
                content += $"• {book.Title} - {book.Author}\n";
                content += $"  ISBN: {book.ISBN} | Stock: {book.Stock}\n\n";
            }
            txtContent.Text = content;
        }

        private void btnUsers_Click(object sender, RoutedEventArgs e)
        {
            var users = _context.Users.Where(u => u.IsActive).ToList();
            string content = "👥 USUARIOS ACTIVOS\n\n";
            foreach (var user in users)
            {
                content += $"• {user.FullName} ({user.Username})\n";
                content += $"  Rol: {user.Role}\n\n";
            }
            txtContent.Text = content;
        }

        private void btnLoans_Click(object sender, RoutedEventArgs e)
        {
            ShowLoansList();
            ActionPanel.Visibility = Visibility.Visible;
        }

        private void ShowLoansList()
        {
            var loans = _context.Loans
                .Where(l => l.Status == "Active")
                .ToList();
            
            string content = "🔄 PRÉSTAMOS ACTIVOS\n\n";
            
            if (!loans.Any())
            {
                content += "No hay préstamos activos.\n\n";
            }
            else
            {
                foreach (var loan in loans)
                {
                    var user = _context.Users.Find(loan.UserId);
                    var book = _context.Books.Find(loan.BookId);
                    content += $"• {book?.Title} → {user?.FullName}\n";
                    content += $"  Vence: {loan.DueDate:dd/MM/yyyy}\n\n";
                }
            }
            
            // Mostrar libros disponibles para préstamo
            content += "\n📚 LIBROS DISPONIBLES:\n\n";
            var availableBooks = _context.Books.Where(b => b.Stock > 0).ToList();
            foreach (var book in availableBooks)
            {
                content += $"• {book.Title} (Stock: {book.Stock})\n";
            }
            
            // Mostrar libros SIN stock (para prueba de validación de negocio)
            content += "\n❌ LIBROS SIN STOCK:\n\n";
            var outOfStock = _context.Books.Where(b => b.Stock == 0).ToList();
            foreach (var book in outOfStock)
            {
                content += $"• {book.Title} (NO DISPONIBLE)\n";
            }
            
            txtContent.Text = content;
        }

        // NUEVO: Crear préstamo
        private void btnNewLoan_Click(object sender, RoutedEventArgs e)
        {
            // Ventana simple de diálogo para crear préstamo
            var inputDialog = new InputDialog(_context);
            if (inputDialog.ShowDialog() == true)
            {
                int bookId = inputDialog.SelectedBookId;
                int userId = inputDialog.SelectedUserId;
                
                var book = _context.Books.Find(bookId);
                
                // VALIDACIÓN DE NEGOCIO: No prestar si no hay stock
                if (book == null || book.Stock <= 0)
                {
                    MessageBox.Show("No se puede realizar el préstamo. El libro no tiene stock disponible.", 
                                "Error de validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                // Crear préstamo
                var loan = new Loan
                {
                    UserId = userId,
                    BookId = bookId,
                    LoanDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(14),
                    Status = "Active"
                };
                
                // Actualizar stock
                book.Stock--;
                
                _context.Loans.Add(loan);
                _context.SaveChanges();
                
                MessageBox.Show("Préstamo registrado exitosamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                ShowLoansList(); // Refrescar lista
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            ShowLoginView();
        }
    }
}