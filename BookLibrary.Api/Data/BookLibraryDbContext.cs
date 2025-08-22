
using BookLibrary.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Api.Data;

public class BookLibraryDbContext : DbContext
{
    public BookLibraryDbContext(DbContextOptions<BookLibraryDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
}
