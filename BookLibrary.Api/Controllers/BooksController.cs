
using BookLibrary.Api.Services;
using BookLibrary.Domain;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Api.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<Book>>> GetBooks(
        [FromQuery] string? searchBy,
        [FromQuery] string? searchValue,
        [FromQuery] string? sortBy,
        [FromQuery] string? sortDirection = "asc",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 5)
    {
        Console.WriteLine($"SearchBy: {searchBy}, SearchValue: {searchValue}, Page: {page}, PageSize: {pageSize}, SortBy: {sortBy}, SortDirection: {sortDirection}");
        
        var result = await _bookService.GetBooks(searchBy, searchValue, sortBy, sortDirection, page, pageSize);
        
        return Ok(result);
    }
}
