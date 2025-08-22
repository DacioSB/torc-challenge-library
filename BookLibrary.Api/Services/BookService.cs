using System;
using BookLibrary.Api.Repositories;
using BookLibrary.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Api.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<PagedResult<Book>> GetBooks(string? searchBy, string? searchValue, string? sortBy, string? sortDirection, int page, int pageSize)
        {
            var query = (await _bookRepository.GetAllBooks()).AsQueryable();

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
                    case "ownership":
                        var ownershipValues = searchValue.Split(',')
                                                         .Select(s => s.Trim())
                                                         .Where(s => System.Enum.TryParse<Ownership>(s, true, out _))
                                                         .Select(s => System.Enum.Parse<Ownership>(s, true))
                                                         .ToList();
                        if (ownershipValues.Any())
                        {
                            query = query.Where(b => ownershipValues.Contains(b.Ownership));
                        }
                        break;
                }
            }

            // Apply sorting if specified
            if (!string.IsNullOrEmpty(sortBy))
            {
                var isDescending = sortDirection?.ToLower() == "desc";

                query = sortBy.ToLower() switch
                {
                    "title" => isDescending ? query.OrderByDescending(b => b.Title) : query.OrderBy(b => b.Title),
                    "author" => isDescending ? query.OrderByDescending(b => b.FirstName + " " + b.LastName) : query.OrderBy(b => b.FirstName + " " + b.LastName),
                    "isbn" => isDescending ? query.OrderByDescending(b => b.Isbn) : query.OrderBy(b => b.Isbn),
                    "category" => isDescending ? query.OrderByDescending(b => b.Category) : query.OrderBy(b => b.Category),
                    _ => query.OrderBy(b => b.Title) // Default sort by title
                };
            }
            else
            {
                // Default sorting by title if no sort specified
                query = query.OrderBy(b => b.Title);
            }

            var totalCount = query.Count();
            
            var books = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var hasNextPage = page < totalPages;
            var hasPreviousPage = page > 1;

            return new PagedResult<Book>
            {
                Data = books,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                HasNextPage = hasNextPage,
                HasPreviousPage = hasPreviousPage
            };
        }
    }
}