using BookLibrary.Api.Data;
using BookLibrary.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookLibrary.Api.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookLibraryDbContext _context;

        public BookRepository(BookLibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _context.Books.ToListAsync();
        }
    }
}