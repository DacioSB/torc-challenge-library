using BookLibrary.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookLibrary.Api.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooks();
    }
}