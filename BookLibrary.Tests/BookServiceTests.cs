using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLibrary.Api.Repositories;
using BookLibrary.Api.Services;
using BookLibrary.Domain;

namespace BookLibrary.Tests
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _mockBookRepository;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _mockBookRepository = new Mock<IBookRepository>();
            _bookService = new BookService(_mockBookRepository.Object);
        }

        [Fact]
        public async Task GetBooks_ReturnsAllBooks_WhenNoSearchCriteria()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { BookId = 1, Title = "Book 1", FirstName = "Author", LastName = "One", Isbn = "111", Category = "Fiction", Ownership = Ownership.Own },
                new Book { BookId = 2, Title = "Book 2", FirstName = "Author", LastName = "Two", Isbn = "222", Category = "Non-Fiction", Ownership = Ownership.Love }
            };
            _mockBookRepository.Setup(repo => repo.GetAllBooks()).ReturnsAsync(books);

            // Act
            var result = await _bookService.GetBooks(null, null, null, null, 1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalCount);
            Assert.Equal(books.Count, result.Data.Count());
        }

        [Theory]
        [InlineData("author", "Author One", 1)]
        [InlineData("title", "Book 2", 1)]
        [InlineData("isbn", "111", 1)]
        public async Task GetBooks_FiltersBySearchCriteria(string searchBy, string searchValue, int expectedCount)
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { BookId = 1, Title = "Book 1", FirstName = "Author", LastName = "One", Isbn = "111", Category = "Fiction", Ownership = Ownership.Own },
                new Book { BookId = 2, Title = "Book 2", FirstName = "Author", LastName = "Two", Isbn = "222", Category = "Non-Fiction", Ownership = Ownership.Love }
            };
            _mockBookRepository.Setup(repo => repo.GetAllBooks()).ReturnsAsync(books);

            // Act
            var result = await _bookService.GetBooks(searchBy, searchValue, null, null, 1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCount, result.TotalCount);
        }

        [Theory]
        [InlineData("Own", 1)]
        [InlineData("Love", 1)]
        [InlineData("WantToRead", 0)]
        [InlineData("Own,Love", 2)]
        public async Task GetBooks_FiltersByOwnership(string searchValue, int expectedCount)
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { BookId = 1, Title = "Book 1", FirstName = "Author", LastName = "One", Isbn = "111", Category = "Fiction", Ownership = Ownership.Own },
                new Book { BookId = 2, Title = "Book 2", FirstName = "Author", LastName = "Two", Isbn = "222", Category = "Non-Fiction", Ownership = Ownership.Love }
            };
            _mockBookRepository.Setup(repo => repo.GetAllBooks()).ReturnsAsync(books);

            // Act
            var result = await _bookService.GetBooks("ownership", searchValue, null, null, 1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCount, result.TotalCount);
        }

        [Theory]
        [InlineData(1, 1, 1, 2, true, false)] // Page 1, PageSize 1, Expected 1 book, TotalPages 2, HasNext true, HasPrevious false
        [InlineData(2, 1, 1, 2, false, true)] // Page 2, PageSize 1, Expected 1 book, TotalPages 2, HasNext false, HasPrevious true
        [InlineData(1, 2, 2, 1, false, false)] // Page 1, PageSize 2, Expected 2 books, TotalPages 1, HasNext false, HasPrevious false
        public async Task GetBooks_AppliesPagination(int page, int pageSize, int expectedCount, int expectedTotalPages, bool expectedHasNextPage, bool expectedHasPreviousPage)
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { BookId = 1, Title = "Book 1", FirstName = "Author", LastName = "One", Isbn = "111", Category = "Fiction", Ownership = Ownership.Own },
                new Book { BookId = 2, Title = "Book 2", FirstName = "Author", LastName = "Two", Isbn = "222", Category = "Non-Fiction", Ownership = Ownership.Love }
            };
            _mockBookRepository.Setup(repo => repo.GetAllBooks()).ReturnsAsync(books);

            // Act
            var result = await _bookService.GetBooks(null, null, null, null, page, pageSize);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCount, result.Data.Count());
            Assert.Equal(books.Count, result.TotalCount);
            Assert.Equal(expectedTotalPages, result.TotalPages);
            Assert.Equal(expectedHasNextPage, result.HasNextPage);
            Assert.Equal(expectedHasPreviousPage, result.HasPreviousPage);
        }

        [Theory]
        [InlineData("title", "asc", "Book 1")]
        [InlineData("title", "desc", "Book 2")]
        [InlineData("author", "asc", "Book 1")]
        [InlineData("author", "desc", "Book 2")]
        public async Task GetBooks_AppliesSorting(string sortBy, string sortDirection, string expectedFirstTitle)
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { BookId = 1, Title = "Book 1", FirstName = "Author", LastName = "One", Isbn = "111", Category = "Fiction", Ownership = Ownership.Own },
                new Book { BookId = 2, Title = "Book 2", FirstName = "Author", LastName = "Two", Isbn = "222", Category = "Non-Fiction", Ownership = Ownership.Love }
            };
            _mockBookRepository.Setup(repo => repo.GetAllBooks()).ReturnsAsync(books);

            // Act
            var result = await _bookService.GetBooks(null, null, sortBy, sortDirection, 1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedFirstTitle, result.Data.First().Title);
        }
    }
}