using Microsoft.EntityFrameworkCore;
using LibraryManager.Core.Models;

namespace LibraryManager.Data
{
    public class LibraryDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Loan> Loans { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // La base de datos se creará en la carpeta de salida del .exe
            optionsBuilder.UseSqlite("Data Source=library.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Datos iniciales (seed) para pruebas
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "admin", Password = "pass123", FullName = "Administrador", Role = "Admin" },
                new User { Id = 2, Username = "bibliotecario", Password = "biblio123", FullName = "Juan Pérez", Role = "User" },
                new User { Id = 3, Username = "maria", Password = "maria123", FullName = "María García", Role = "User", IsActive = false }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "Cien años de soledad", Author = "Gabriel García Márquez", ISBN = "978-0307474728", Stock = 3, PublishedYear = 1967 },
                new Book { Id = 2, Title = "El Quijote", Author = "Miguel de Cervantes", ISBN = "978-8420412146", Stock = 0, PublishedYear = 1605 },
                new Book { Id = 3, Title = "1984", Author = "George Orwell", ISBN = "978-0451524935", Stock = 5, PublishedYear = 1949 },
                new Book { Id = 4, Title = "Donde los árboles cantan", Author = "Laura Gallego", ISBN = "978-8467550030", Stock = 2, PublishedYear = 2011 }
            );
        }
    }
}