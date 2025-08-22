
using BookLibrary.Api.Data;
using BookLibrary.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BooksController : ControllerBase
{
    private readonly BookLibraryDbContext _context;

    public BooksController(BookLibraryDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks(
        [FromQuery] string? searchBy,
        [FromQuery] string? searchValue)
    {
        Console.WriteLine($"SearchBy: {searchBy}, SearchValue: {searchValue}");
        var query = _context.Books.AsQueryable();

        if (!string.IsNullOrEmpty(searchBy) && !string.IsNullOrEmpty(searchValue))
        {
            switch (searchBy.ToLower())
            {
                case "author":
                    query = query.Where(b => (b.FirstName + " " + b.LastName).ToLower().Contains(searchValue.ToLower()));
                    break;
                case "isbn":
                    query = query.Where(b => b.Isbn.ToLower().Contains(searchValue.ToLower()));
                    break;
                case "title":
                    query = query.Where(b => b.Title.ToLower().Contains(searchValue.ToLower()));
                    break;
            }
        }

        return await query.ToListAsync();
    }
}
