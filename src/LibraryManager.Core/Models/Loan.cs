namespace LibraryManager.Core.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime LoanDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; } = "Active"; // Active, Returned, Overdue
        
        // Navegación
        public User User { get; set; } = null!;
        public Book Book { get; set; } = null!;
        
        public bool IsOverdue => Status == "Active" && DateTime.Now > DueDate;
    }
}