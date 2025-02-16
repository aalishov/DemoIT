using Books.ViewModels.Book;
using Books.ViewModels.Books;

namespace Books.Services.Contracts
{
    public interface IBookService
    {
        public Task<IndexBooksViewModel> GetWorstBookAsync();
        public Task<IndexBookViewModel> GetBestBookAsync();
        public Task DeleteBookByIdAsync(string BookId);

        public Task<DeleteBookViewModel> GetBookToDeleteByIdAsync(string BookId);

        public Task<DetailsBookViewModel> GetBookDetailsByIdAsync(string BookId);

        public Task<string> CreateBookAsync(CreateBookViewModel model);

        public Task<IndexBooksViewModel> GetBooksAsync(IndexBooksViewModel model);

        public Task<string> UpdateBookAsync(EditBookViewModel model);

        public Task<EditBookViewModel> GetBookToEditAsync(string BookId);
        Task<IndexBookViewModel> GetBooksAsync(IndexBookViewModel books);
    }
}
