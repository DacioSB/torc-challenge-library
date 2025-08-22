using BookLibrary.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookLibrary.Api.Services
{
    public interface IBookService
    {
        Task<PagedResult<Book>> GetBooks(string? searchBy, string? searchValue, string? sortBy, string? sortDirection, int page, int pageSize);
    }
}