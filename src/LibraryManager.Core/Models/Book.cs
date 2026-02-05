namespace LibraryManager.Core.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int Stock { get; set; }
        public int PublishedYear { get; set; }
        public bool IsAvailable => Stock > 0;
        
        // Navegación
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}